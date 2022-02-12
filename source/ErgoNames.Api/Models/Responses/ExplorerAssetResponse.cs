using System.Text.Json.Serialization;

namespace ErgoNames.Api.Models.Responses
{
    public class ExplorerAssetResponse
    {
        [JsonPropertyName("id")]

        public string Id { get; set; }

        [JsonPropertyName("tokenId")]
        public string TokenId { get; set; }

        [JsonPropertyName("boxId")]
        public string BoxId { get; set; }

        [JsonPropertyName("emissionAmount")]
        public long EmissionAmount { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("decimals")]
        public long Decimals { get; set; }
    }
}
