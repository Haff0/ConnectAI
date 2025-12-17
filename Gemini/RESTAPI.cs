using System.Text;
using System.Text.Json;

namespace ConnectAI.Gemini
{

    public class GeminiAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiAiService( string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey; 
        }

        public async Task<AiGenerateResult> GenerateContentAsync(string prompt)
        {
            var request = new GeminiRequest
            {
                contents =
                {
                    new Content
                    {
                        parts =
                        {
                            new Part { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(request);

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-3-pro-preview:generateContent"
            );

            httpRequest.Headers.Add("x-goog-api-key", _apiKey);
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            return responseJson.ToAiGenerateResult();
        }

        public async Task<AiStandardResponse<T>> GenerateRawAsync<T>(string prompt)
        {
            prompt = AiPromptBuilder.BuildPrompt<AiStandardResponse<T>>(prompt);

            var request = new GeminiRequest
            {
                contents =
                {
                    new Content
                    {
                        parts =
                        {
                            new Part { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(request);

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent"
            );

            httpRequest.Headers.Add("x-goog-api-key", _apiKey);
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            var aiGenerateResult = responseJson.ToAiGenerateResult();
            var aiGenerateResultParse = aiGenerateResult.Text.Parse<T>();

            return responseJson.ToAiGenerateResult().Text.Parse<T>();
        }
    }

}
