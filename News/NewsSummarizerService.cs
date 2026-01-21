// Services/NewsSummarizerService.cs
using ConnectAI;
using ConnectAI.Gemini;
using static ConnectAI.Enum;

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
        var userRequest = $$$"""
        Title: {{{article.Title}}}
        Content: {{{article.Content}}}
        """;

        var prompt = AiPromptBuilder.BuildPrompt<AiStandardResponse<NewsInsight>>(userRequest, AiPromptType.News);
        var res = await geminiAiService.GenerateRawAsync<NewsInsight>(prompt);

        return res.Data ?? new NewsInsight();
    }
}
