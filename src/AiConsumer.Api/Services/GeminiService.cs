using GemiNet.Extensions.AI;
using Microsoft.Extensions.AI;

namespace AiConsumer.Api.Services
{
    public class GeminiService
    {
        private readonly GemiNetClient _client;

        public GeminiService(GemiNetClient client)
        {
            _client = client;
        }

        public async Task<string> GenerateContentAsync(string prompt, byte[] fileContent, string mimeType)
        {
            var geminiPrompt = new ChatPrompt
            {
                new TextContent(prompt),
                new ImageContent(fileContent, mimeType)
            };

            var response = await _client.GenerateTextAsync(geminiPrompt);
            return response;
        }
    }
}