using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ErgoNames.Api.Models.Responses
{
    public class ExplorerOutputResponse
    {
        [JsonPropertyName("boxId")]
        public string? BoxId { get; set; }

        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }

        [JsonPropertyName("blockId")]
        public string? BlockId { get; set; }

        [JsonPropertyName("value")]
        public long? Value { get; set; }

        [JsonPropertyName("index")]
        public long? Index { get; set; }

        [JsonPropertyName("globalIndex")]
        public long? GlobalIndex { get; set; }

        [JsonPropertyName("creationHeight")]
        public long? CreationHeight { get; set; }

        [JsonPropertyName("settlementHeight")]
        public long? SettlementHeight { get; set; }

        [JsonPropertyName("ergoTree")]
        public string? ErgoTree { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("assets")]
        public IEnumerable<ExplorerAssetResponse>? Assets { get; set; }

        [JsonPropertyName("additionalRegisters")]
        public ExplorerAdditionalRegistersResponse? AdditionalRegisters { get; set; }

        [JsonPropertyName("spentTransactionId")]
        public string? SpentTransactionId { get; set; }

        [JsonPropertyName("mainChain")]
        public bool? MainChain { get; set; }
    }
}
