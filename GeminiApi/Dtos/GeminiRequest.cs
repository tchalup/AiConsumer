using System.Text.Json.Serialization;

namespace GeminiApi.Dtos
{
    public class GeminiRequest
    {
        [JsonPropertyName("contents")]
        public List<Content> Contents { get; set; } = new();
    }

    public class Content
    {
        [JsonPropertyName("parts")]
        public List<Part> Parts { get; set; } = new();
    }

    public class Part
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("inlineData")]
        public InlineData? InlineData { get; set; }
    }

    public class InlineData
    {
        [JsonPropertyName("mimeType")]
        public string MimeType { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public string Data { get; set; } = string.Empty;
    }
}