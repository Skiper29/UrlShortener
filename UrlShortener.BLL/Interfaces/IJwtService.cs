using UrlShortener.DAL.Entities;

namespace UrlShortener.BLL.Interfaces;

public interface IJwtService
{
    string GenerateToken(AppUser user, IList<string> roles);
}
