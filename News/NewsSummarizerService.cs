// Services/NewsSummarizerService.cs
using ConnectAI.Gemini;

public interface INewsSummarizerService
{
    Task<NewsInsight> SummarizeAsync(NewsArticle article);
}

public class NewsSummarizerService : INewsSummarizerService
{
    private readonly IAIService geminiAiService ;

    public NewsSummarizerService(HttpClient http, string _aiKey)
    {
        geminiAiService = new GeminiAiService(_aiKey);
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

        var res = await geminiAiService.GenerateRawAsync<NewsInsight>(prompt);

        return res.Data ?? new NewsInsight();
    }
}
