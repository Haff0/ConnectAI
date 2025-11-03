// Models/NewsArticle.cs
public class NewsArticle
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Source { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; }
}

// Models/NewsInsight.cs
public class NewsInsight
{
    public string ArticleId { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public List<string> Entities { get; set; } = [];
    public string Sentiment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
