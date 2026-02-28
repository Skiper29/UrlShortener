using System.Transactions;

namespace UrlShortener.DAL.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    IUrlRepository UrlRepository { get; }
    IAboutContentRepository AboutContentRepository { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync();

    TransactionScope BeginTransaction();
}
