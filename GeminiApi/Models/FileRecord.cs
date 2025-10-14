namespace GeminiApi.Models
{
    public class FileRecord
    {
        public Guid Id { get; set; }
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string? Prompt { get; set; }
    }
}