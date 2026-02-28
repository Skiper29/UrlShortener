using System.Transactions;
using UrlShortener.DAL.Data;
using UrlShortener.DAL.Repositories.Interfaces;
using UrlShortener.DAL.Repositories.Interfaces.Base;

namespace UrlShortener.DAL.Repositories.Realizations.Base;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly AppDbContext _dbContext;
    private IUrlRepository? _urlRepository;
    private IAboutContentRepository? _aboutContentRepository;

    public RepositoryWrapper(AppDbContext context)
    {
        _dbContext = context;
    }

    public IUrlRepository UrlRepository => _urlRepository ??= new UrlRepository(_dbContext);
    public IAboutContentRepository AboutContentRepository => _aboutContentRepository ??= new AboutContentRepository(_dbContext);

    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public TransactionScope BeginTransaction()
    {
        return new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    }
}
