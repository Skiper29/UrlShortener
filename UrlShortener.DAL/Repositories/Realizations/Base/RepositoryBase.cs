using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using UrlShortener.DAL.Data;
using UrlShortener.DAL.Repositories.Interfaces.Base;
using UrlShortener.DAL.Repositories.Options;

namespace UrlShortener.DAL.Repositories.Realizations.Base;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly AppDbContext DbContext;

    public RepositoryBase(AppDbContext context)
    {
        DbContext = context;
    }

    public async Task<T> CreateAsync(T entity)
    {
        return (await DbContext.Set<T>().AddAsync(entity)).Entity;
    }

    public void Delete(T entity)
    {
        DbContext.Set<T>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        DbContext.Set<T>().RemoveRange(entities);
    }

    public async Task<IEnumerable<T>> GetAllAsync(QueryOptions<T>? queryOptions = null)
    {
        IQueryable<T> query = DbContext.Set<T>();

        if (queryOptions is not null)
        {
            query = ApplyQueryOptions(query, queryOptions);
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetFirstOrDefaultAsync(QueryOptions<T>? queryOptions = null)
    {
        IQueryable<T> query = DbContext.Set<T>();

        if (queryOptions is not null)
        {
            query = ApplyTracking(query, queryOptions.AsNoTracking);
            query = ApplyInclude(query, queryOptions.Include);
            query = ApplyFilter(query, queryOptions.Filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public EntityEntry<T> Update(T entity)
    {
        return DbContext.Set<T>().Update(entity);
    }

    private static IQueryable<T> ApplyQueryOptions(IQueryable<T> query, QueryOptions<T> queryOptions)
    {
        query = ApplyFilter(query, queryOptions.Filter);
        query = ApplyInclude(query, queryOptions.Include);
        query = ApplyTracking(query, queryOptions.AsNoTracking);
        query = ApplyOrdering(query, queryOptions.OrderByAsc, queryOptions.OrderByDesc);
        query = ApplyPagination(query, queryOptions.Offset, queryOptions.Limit);

        return query;
    }


    private static IQueryable<T> ApplyFilter(IQueryable<T> query, Expression<Func<T, bool>>? filter)
    {
        return filter is not null ? query.Where(filter) : query;
    }

    private static IQueryable<T> ApplyInclude(IQueryable<T> query,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include)
    {
        return include is not null ? include(query) : query;
    }

    private static IQueryable<T> ApplyTracking(IQueryable<T> query, bool asNoTracking)
    {
        return asNoTracking ? query.AsNoTracking() : query;
    }

    private static IQueryable<T> ApplyOrdering(
        IQueryable<T> query,
        Expression<Func<T, object>>? orderByAsc,
        Expression<Func<T, object>>? orderByDesc)
    {
        if (orderByAsc is not null)
        {
            query = query.OrderBy(orderByAsc);
        }

        if (orderByDesc is not null)
        {
            query = query.OrderByDescending(orderByDesc);
        }

        return query;
    }

    private static IQueryable<T> ApplyPagination(IQueryable<T> query, int offset, int limit)
    {
        if (offset > 0)
        {
            query = query.Skip(offset);
        }

        if (limit > 0)
        {
            query = query.Take(limit);
        }

        return query;
    }
}
