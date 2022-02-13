using System.Text.Json.Serialization;

namespace ErgoNames.Api.Models.Responses
{
    public class ExplorerAdditionalRegistersResponse
    {
        [JsonPropertyName("R4")]
        public ExplorerRegisterResponse? R4 { get; set; }

        [JsonPropertyName("R5")]
        public ExplorerRegisterResponse? R5 { get; set; }

        [JsonPropertyName("R6")]
        public ExplorerRegisterResponse? R6 { get; set; }
    }
}
