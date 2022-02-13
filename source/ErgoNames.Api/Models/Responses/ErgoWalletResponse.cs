using System.Text.Json.Serialization;

namespace ErgoNames.Api.Models.Responses
{
    public class ErgoWalletResponse
    {
        [JsonPropertyName("ergo_name")]
        public string? ErgoName { get; set; }

        [JsonPropertyName("ergo")]
        public string? ErgoWalletAddress { get; set; }
    }
}
