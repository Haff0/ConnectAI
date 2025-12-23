using System.Text;
using System.Text.Json;
using static ConnectAI.Enum;

namespace ConnectAI.Gemini
{
    public interface IAIService
    {
        Task<AiGenerateResult> GenerateContentAsync(string prompt);
        Task<AiStandardResponse<T>> GenerateRawAsync<T>(string prompt);
    }
    internal class GeminiAiService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly string _url;

        public GeminiAiService( string apiKey, string? model = default)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
            _model = model ?? "gemini-2.5-flash";
            _url = "https://generativelanguage.googleapis.com/v1beta/models/";
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
                _url + _model + ":generateContent"
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
            prompt = AiPromptBuilder.BuildPrompt<AiStandardResponse<T>>(prompt, AiPromptType.JsonWithConfidence);

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
                _url + _model + ":generateContent"
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

        //Task<AiGenerateResult> IAIService.GenerateContentAsync(string prompt)
        //{
        //    return GenerateContentAsync(prompt);
        //}

        //Task<AiStandardResponse<T>> IAIService.GenerateRawAsync<T>(string prompt)
        //{
        //    return GenerateRawAsync<T>(prompt);
        //}
    }

}
