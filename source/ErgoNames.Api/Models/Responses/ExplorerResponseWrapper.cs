using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ErgoNames.Api.Models.Responses
{
    public class ExplorerResponseWrapper<T>
    {
        [JsonPropertyName("items")]
        public IEnumerable<T>? Items { get; set; }

        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }
}
