using System.ComponentModel.DataAnnotations;

namespace UrlShortener.BLL.DTOs.Auth;

public record LoginRequest(
    [Required] string Login,
    [Required] string Password
);
