using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using ErgoNames.Api.Data;
using ErgoNames.Api.Models.Configuration;
using ErgoNames.Api.Models.Responses;
using ErgoNames.Api.Security;
using ErgoNames.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Pinata.Client;
using Encoder = System.Drawing.Imaging.Encoder;

namespace ErgoNames.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ErgoNamesController : ControllerBase
    {
        private readonly ITokenResolver tokenResolver;
        private readonly ITableRepository repository;
        private readonly ErgoNameApiConfiguration configuration;
        private readonly ILogger<ErgoNamesController> logger;
        
        public ErgoNamesController(ITableRepository repository, ITokenResolver tokenResolver, ErgoNameApiConfiguration configuration, ILogger<ErgoNamesController> logger)
        {
            this.repository = repository;
            this.tokenResolver = tokenResolver;
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpGet, Route("resolve/{name}")]
        public async Task<IActionResult> Resolve(string name)
        {
            logger.LogDebug("Resolving name {name}", name);

            try
            {
                var token = await tokenResolver.ResolveTokenNameToAddressAsync(name);
                ErgoWalletResponse response = new();
                response.ErgoName = name.ToLowerInvariant();
                response.ErgoWalletAddress = token;

                return Ok(response);
            }
            catch (Exception e)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                Error error = new();
                error.Status = "400";
                error.Title = "Bad Request";
                error.Detail = e.Message;
                errorResponse.Errors.Add(error);

                return BadRequest(errorResponse);
            }
        }

        [BasicAuthorization]
        [HttpPost, Route("reserve/{name}")]
        public async Task<IActionResult> Reserve(string name)
        {
            try
            {
                await repository.ReserveName(name);
                return Ok();
            }
            catch (Azure.RequestFailedException e)
            {
                logger.LogError(e, "Error reserving name {name}", name);
                ErrorResponse errorResponse = new ErrorResponse();
                Error error = new();
                error.Status = e.Status.ToString();
                error.Title = "Bad Request";
                error.Detail = e.ErrorCode;
                errorResponse.Errors.Add(error);

                return BadRequest(errorResponse);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error reserving name {name}", name);
                ErrorResponse errorResponse = new ErrorResponse();
                Error error = new();
                error.Status = "400";
                error.Title = "Bad Request";
                error.Detail = e.Message;
                errorResponse.Errors.Add(error);

                return BadRequest(errorResponse);
            }
        }

        [BasicAuthorization]
        [HttpDelete, Route("release/{name}")]
        public async Task<IActionResult> Release(string name)
        {
            logger.LogDebug("Releasing name {name}", name);

            try
            {
                await repository.ReleaseName(name);
                return NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error releasing name {name}", name);
                ErrorResponse errorResponse = new ErrorResponse();
                Error error = new();
                error.Status = "400";
                error.Title = "Bad Request";
                error.Detail = e.Message;
                errorResponse.Errors.Add(error);

                return BadRequest(errorResponse);
            }
        }

        [BasicAuthorization]
        [HttpPost, Route("create-nft-resource/{name}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "App targeted for Windows")]
        public async Task<IActionResult> CreateResource(string name)
        {

            var fileName = "ErgoNames.Api.Templates.nft_background.jpg";
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream? stream = assembly.GetManifestResourceStream(fileName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("Cannot find template file.", fileName);
                }

                using (Image image = Image.FromStream(stream))
                {
                    using (Bitmap b = new Bitmap(image))
                    {
                        using (Graphics graphics = Graphics.FromImage(b))
                        {
                            Font font = new Font(FontFamily.GenericSansSerif, 32, FontStyle.Bold);
                            graphics.DrawString(name.ToLowerInvariant(), font, Brushes.White, 70, 70);

                            EncoderParameter parameter = new EncoderParameter(Encoder.Quality, 50L);
                            EncoderParameters parameters = new EncoderParameters(1);
                            parameters.Param = new EncoderParameter[] { parameter };

                            ImageCodecInfo? codecInfo = GetEncoder(ImageFormat.Jpeg);
                            if (codecInfo == null) throw new Exception("Could not located jpeg ImageCodecInfo");

                            //b.Save(@"D:\temp\temp-image.jpg", codecInfo, parameters);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                b.Save(ms, codecInfo, parameters);
                                ms.Seek(0, SeekOrigin.Begin);

                                var config = new Config
                                {
                                    ApiKey = configuration.PinataKey,
                                    ApiSecret = configuration.PinataSecret
                                };

                                PinataClient client = new PinataClient(config);

                                var metadata = new PinataMetadata
                                {
                                    KeyValues =
                                    {
                                        {"Author", "ErgoNames"},
                                        {"Name", name.ToLowerInvariant()}
                                    }
                                };
                                
                                var response = await client.Pinning.PinFileToIpfsAsync(content =>
                                {
                                    HttpContent c = new StreamContent(ms);

                                    content.AddPinataFile(c, name.ToLowerInvariant());
                                }, metadata);

                                if (response.IsSuccess)
                                {
                                    IpfsResponse ipfsResponse = new IpfsResponse(response.IpfsHash);
                                    return Ok(ipfsResponse);
                                }
                            }
                        }
                    }
                }
            }


            return Ok();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "App targeted for Windows")]
        private ImageCodecInfo? GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}
