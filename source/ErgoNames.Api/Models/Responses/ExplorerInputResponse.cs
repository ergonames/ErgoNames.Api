using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ErgoNames.Api.Models.Responses
{
    public class ExplorerInputResponse
    {
        [JsonPropertyName("boxId")]
        public string BoxId { get; set; }

        [JsonPropertyName("value")]
        public long Value { get; set; }

        [JsonPropertyName("index")]
        public long Index { get; set; }

        [JsonPropertyName("spendingProof")]
        public string SpendingProof { get; set; }

        [JsonPropertyName("outputBlockId")]
        public string OutputBlockId { get; set; }

        [JsonPropertyName("outputTransactionId")]
        public string OutputTransactionId { get; set; }

        [JsonPropertyName("outputIndex")]
        public long OutputIndex { get; set; }

        [JsonPropertyName("outputGlobalIndex")]
        public long OutputGlobalIndex { get; set; }

        [JsonPropertyName("outputCreatedAt")]
        public long OutputCreatedAt { get; set; }

        [JsonPropertyName("outputSettledAt")]
        public long OutputSettledAt { get; set; }

        [JsonPropertyName("ergoTree")]
        public string ErgoTree { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("assets")]
        public IEnumerable<ExplorerAssetResponse> Assets { get; set; }

        [JsonPropertyName("additionalRegisters")]
        public ExplorerAdditionalRegistersResponse AdditionalRegisters { get; set; }
    }
}
