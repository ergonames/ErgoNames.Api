using System.Text.Json.Serialization;

namespace ErgoNames.Api.Models.Responses
{
    public class ErgoWalletResponse
    {
        [JsonPropertyName("ergo")]
        public string? Ergo { get; set; }
    }
}
