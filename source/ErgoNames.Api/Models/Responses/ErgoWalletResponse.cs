using System.Text.Json.Serialization;

namespace ErgoNames.Api.Models.Responses
{
    public class ErgoWalletResponse
    {
        [JsonPropertyName("source_name")]
        public string? SourceName { get; set; }

        [JsonPropertyName("ergo")]
        public string? Ergo { get; set; }
    }
}
