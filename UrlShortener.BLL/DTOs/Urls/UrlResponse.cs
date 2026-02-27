namespace UrlShortener.BLL.DTOs.Urls;

public record UrlResponse(
    int Id,
    string OriginalUrl,
    string ShortCode,
    string ShortUrl,
    string CreatedBy,
    DateTime CreatedAt
);
