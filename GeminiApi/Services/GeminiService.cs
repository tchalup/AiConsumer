using System.Text;
using System.Text.Json;
using GeminiApi.Dtos;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeminiApi.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GenerateContentAsync(string prompt, byte[] fileContent, string mimeType)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            var baseUrl = _configuration["Gemini:BaseUrl"];
            var requestUrl = $"{baseUrl}v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

            var requestBody = new GeminiRequest
            {
                Contents = new List<Content>
                {
                    new Content
                    {
                        Parts = new List<Part>
                        {
                            new Part { Text = prompt },
                            new Part
                            {
                                InlineData = new InlineData
                                {
                                    MimeType = mimeType,
                                    Data = Convert.ToBase64String(fileContent)
                                }
                            }
                        }
                    }
                }
            };

            var jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error calling Gemini API: {response.StatusCode}, {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseJson);

            // Extrai e retorna o texto da primeira parte do primeiro candidato.
            // Adicione tratamento de erro mais robusto conforme necessário.
            return geminiResponse?.Candidates.FirstOrDefault()?.Content.Parts.FirstOrDefault()?.Text ?? "No content returned.";
        }
    }
}