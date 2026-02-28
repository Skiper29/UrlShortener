namespace UrlShortener.DAL.Entities;

public class AboutContent
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
