using System.ComponentModel.DataAnnotations;

namespace UrlShortener.BLL.DTOs.Auth;

public record RegisterRequest(
    [Required] string UserName,
    [Required][EmailAddress] string Email,
    [Required][MinLength(6)] string Password
);
