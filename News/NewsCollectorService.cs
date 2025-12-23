// Services/NewsCollectorService.cs
using System.ServiceModel.Syndication;
using System.Xml;

public interface INewsCollectorService
{
    Task<List<NewsArticle>> CollectAsync();
}

public class NewsCollectorService : INewsCollectorService
{
    private readonly HttpClient _http;

    public NewsCollectorService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<NewsArticle>> CollectAsync()
    {
        var sources = new[]
        {
            "https://www.coindesk.com/arc/outboundfeeds/rss/",
            "https://cointelegraph.com/rss",
            //"https://www.reuters.com/markets/crypto/rss/",
        };

        var result = new List<NewsArticle>();

        foreach (var url in sources)
        {
            using var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);

            foreach (var item in feed.Items.Take(5))
            {
                result.Add(new NewsArticle
                {
                    Source = feed.Title?.Text ?? "Unknown",
                    Title = item.Title?.Text ?? "",
                    Url = item.Links.FirstOrDefault()?.Uri.ToString() ?? "",
                    Content = item.Summary?.Text ?? "",
                    PublishedAt = item.PublishDate.UtcDateTime
                });
            }
        }

        return result;
    }
}
