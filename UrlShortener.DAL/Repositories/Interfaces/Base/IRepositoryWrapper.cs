using System.Transactions;

namespace UrlShortener.DAL.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    int SaveChanges();

    Task<int> SaveChangesAsync();

    TransactionScope BeginTransaction();
}
