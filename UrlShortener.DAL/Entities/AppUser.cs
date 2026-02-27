using Microsoft.AspNetCore.Identity;

namespace UrlShortener.DAL.Entities;

public class AppUser : IdentityUser
{
    public DateTime CreatedAt { get; set; }

    public ICollection<ShortenedUrl> ShortenedUrls { get; set; } = new List<ShortenedUrl>();
}
