// Services/NewsSummarizerService.cs
using System.Text.Json;
using System.Net.Http.Json;

public interface INewsSummarizerService
{
    Task<NewsInsight> SummarizeAsync(NewsArticle article);
}

public class NewsSummarizerService : INewsSummarizerService
{
    private readonly HttpClient _http;

    public NewsSummarizerService(HttpClient http)
    {
        _http = http;
    }

    public async Task<NewsInsight> SummarizeAsync(NewsArticle article)
    {
        var prompt = $$$"""
        You are an AI financial news analyst.
        Summarize the following article and extract key crypto-related insights.

        Title: {{{article.Title}}}
        Content: {{{article.Content}}}

        Output in JSON:
        {{
            "summary": "...",
            "entities": ["BTC", "ETH", ...],
            "sentiment": "positive|negative|neutral",
            "impactAssets": ["BTC", "ETH"]
        }}
        """;

        var body = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var res = await _http.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", body);
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        var insight = JsonSerializer.Deserialize<NewsInsight>(content!);
        insight!.ArticleId = article.Id;
        return insight;
    }
}
