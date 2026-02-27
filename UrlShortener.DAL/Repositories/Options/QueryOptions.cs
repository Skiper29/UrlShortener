using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace UrlShortener.DAL.Repositories.Options;

public record QueryOptions<T>
{
    public Expression<Func<T, bool>>? Filter { get; set; }
    public Func<IQueryable<T>, IIncludableQueryable<T, object>>? Include { get; set; }
    public int Offset { get; set; }
    public int Limit { get; set; }
    public Expression<Func<T, object>>? OrderByAsc { get; set; }
    public Expression<Func<T, object>>? OrderByDesc { get; set; }
    public bool AsNoTracking { get; set; } = true;
}
