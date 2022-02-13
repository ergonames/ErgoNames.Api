using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ErgoNames.Api.Models.Responses
{
    public class ExplorerTransactionResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("blockId")]
        public string? BlockId { get; set; }

        [JsonPropertyName("inclusionHeight")]
        public long? InclusionHeight { get; set; }

        [JsonPropertyName("timestamp")]
        public long? Timestamp { get; set; }

        [JsonPropertyName("index")]
        public long? Index { get; set; }

        [JsonPropertyName("globalIndex")]
        public long? GlobalIndex { get; set; }

        [JsonPropertyName("numConfirmations")]
        public long? NumConfirmations { get; set; }

        [JsonPropertyName("inputs")]
        public IEnumerable<ExplorerInputResponse>? Inputs { get; set; }

        [JsonPropertyName("dataInputs")]
        public object[]? DataInputs { get; set; }

        [JsonPropertyName("outputs")]
        public IEnumerable<ExplorerOutputResponse>? Outputs { get; set; }

        [JsonPropertyName("size")]
        public long? Size { get; set; }
    }
}
