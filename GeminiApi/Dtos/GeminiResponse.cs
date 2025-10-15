using System.Text.Json.Serialization;

namespace GeminiApi.Dtos
{
    public class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<Candidate> Candidates { get; set; } = new();
    }

    public class Candidate
    {
        [JsonPropertyName("content")]
        public Content Content { get; set; } = new();
    }
}