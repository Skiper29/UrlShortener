namespace UrlShortener.BLL.DTOs.Urls;

public record UrlDetailResponse(
    int Id,
    string OriginalUrl,
    string ShortCode,
    string ShortUrl,
    string CreatedById,
    string CreatedByUserName,
    string CreatedByEmail,
    DateTime CreatedAt
);
