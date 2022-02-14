using System.Text.Json.Serialization;

namespace ErgoNames.Api.Models.Responses
{
    public class IpfsResponse
    {
        private readonly string hash;
        public IpfsResponse(string hash)
        {
            this.hash = hash;
        }

        [JsonPropertyName("ipfs_url")]
        public string IpfsUrl => $"ipfs://{hash}";

        [JsonPropertyName("gateway_url")]
        public string GatewayUrl => $"https://cloudflare-ipfs.com/ipfs/{hash}";
    }
}
