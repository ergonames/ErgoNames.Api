using ErgoNames.Api.Data;
using ErgoNames.Api.Models.Responses;
using ErgoNames.Api.Security;
using ErgoNames.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ErgoNames.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ErgoNamesController : ControllerBase
    {
        private readonly ITokenResolver tokenResolver;
        private readonly ITableRepository repository;
        private readonly ILogger<ErgoNamesController> logger;

        public ErgoNamesController(ITableRepository repository, ITokenResolver tokenResolver, ILogger<ErgoNamesController> logger)
        {
            this.repository = repository;
            this.tokenResolver = tokenResolver;
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
                response.Ergo = token;

                return Ok(response);
            }
            catch (Exception e)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                Error error = new Error();
                error.Status = "400";
                error.Title = "Bad Request";
                error.Detail = e.Message;

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
            catch (Exception e)
            {
                logger.LogError(e, "Error reserving name {name}", name);
                return BadRequest();
            }
        }

        [BasicAuthorization]
        [HttpDelete, Route("release/{name}")]
        public async Task<IActionResult> Release(string name)
        {
            logger.LogDebug("Releasing name {name}", name);
            await repository.ReleaseName(name);
            return NoContent();
        }
    }
}
