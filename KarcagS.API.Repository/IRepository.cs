using System.Linq.Expressions;
using KarcagS.API.Data.Entities;

namespace KarcagS.API.Repository;

public interface IRepository<T, TKey> where T : Entity<TKey>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null);
    Task<IEnumerable<T>> GetAllAsOrderedAsync(string orderBy, string direction);
    Task<IEnumerable<T>> GetOrderedListAsync(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null);
    Task<IQueryable<T>> GetAllAsQueryAsync();
    Task<IQueryable<T>> GetListAsQueryAsync(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null);
    Task<long> CountAsync();
    Task<long> CountAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetAsync(TKey id);
    Task<T?> GetOptionalAsync(TKey id);
    Task UpdateAsync(T entity, bool doPersist = true);
    Task UpdateRangeAsync(IEnumerable<T> entities, bool doPersist = true);
    Task<TKey> CreateAsync(T entity, bool doPersist = true);
    Task CreateRangeAsync(IEnumerable<T> entities, bool doPersist = true);
    Task DeleteAsync(T entity, bool doPersist = true);
    Task DeleteRangeAsync(IEnumerable<T> entities, bool doPersist = true);
    Task DeleteByIdAsync(TKey id, bool doPersist = true);
    Task PersistAsync();
}
