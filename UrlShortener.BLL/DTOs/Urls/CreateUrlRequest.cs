using System.ComponentModel.DataAnnotations;

namespace UrlShortener.BLL.DTOs.Urls;

public record CreateUrlRequest(
    [Required][Url] string OriginalUrl
);
