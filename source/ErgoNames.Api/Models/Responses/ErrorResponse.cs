using System.Text.Json.Serialization;

namespace ErgoNames.Api.Models.Responses
{
    public class ErrorResponse
    {
        [JsonPropertyName("errors")]
        public List<Error> Errors { get; set; }

        public ErrorResponse()
        {
            Errors = new List<Error>();
        }
    }

    public class Error
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("detail")]
        public string? Detail { get; set; }
    }
}
