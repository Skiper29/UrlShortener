using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Repositories.Interfaces.Base;

namespace UrlShortener.DAL.Repositories.Interfaces;

public interface IUrlRepository : IRepositoryBase<ShortenedUrl>
{
}
