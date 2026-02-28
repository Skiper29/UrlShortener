using UrlShortener.DAL.Data;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Repositories.Interfaces;
using UrlShortener.DAL.Repositories.Realizations.Base;

namespace UrlShortener.DAL.Repositories.Realizations;

public class AboutContentRepository : RepositoryBase<AboutContent>, IAboutContentRepository
{
    public AboutContentRepository(AppDbContext context) : base(context)
    {
    }
}
