using UrlShortener.DAL.Data;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Repositories.Interfaces;
using UrlShortener.DAL.Repositories.Realizations.Base;

namespace UrlShortener.DAL.Repositories.Realizations;

public class UrlRepository : RepositoryBase<ShortenedUrl>, IUrlRepository
{
    public UrlRepository(AppDbContext context) : base(context)
    {
    }
}
