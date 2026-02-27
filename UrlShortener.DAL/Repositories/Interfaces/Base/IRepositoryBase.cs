using Microsoft.EntityFrameworkCore.ChangeTracking;
using UrlShortener.DAL.Repositories.Options;

namespace UrlShortener.DAL.Repositories.Interfaces.Base;

public interface IRepositoryBase<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(QueryOptions<T>? queryOptions = null);

    Task<T?> GetFirstOrDefaultAsync(QueryOptions<T>? queryOptions = null);

    Task<T> CreateAsync(T entity);

    EntityEntry<T> Update(T entity);

    void Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);
}
