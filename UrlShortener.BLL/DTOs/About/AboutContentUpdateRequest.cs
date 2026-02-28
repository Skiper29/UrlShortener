using System.ComponentModel.DataAnnotations;

namespace UrlShortener.BLL.DTOs.About;

public record AboutContentUpdateRequest([Required][MaxLength(5000)] string Content);

