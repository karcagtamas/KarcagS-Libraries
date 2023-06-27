using System.Linq.Expressions;
using KarcagS.API.Data.Entities;

namespace KarcagS.API.Repository;

public interface IPersistence
{
    Task<T> GetAsync<TKey, T>(TKey id) where T : Entity<TKey>;
    Task<T?> GetOptionalAsync<TKey, T>(TKey id) where T : Entity<TKey>;
    Task<IEnumerable<T>> GetAllAsync<TKey, T>() where T : Entity<TKey>;
    Task<IEnumerable<T>> GetListAsync<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : Entity<TKey>;
    Task<IEnumerable<T>> GetAllAsOrderedAsync<TKey, T>(string orderBy, string direction) where T : Entity<TKey>;
    Task<IEnumerable<T>> GetOrderedListAsync<TKey, T>(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null) where T : Entity<TKey>;
    Task<IEnumerable<T>> GetOrderedListByQueryAsync<TKey, T>(IQueryable<T> queryable, string orderBy, string direction) where T : Entity<TKey>;
    Task<IQueryable<T>> GetAllAsQueryAsync<TKey, T>() where T : Entity<TKey>;
    Task<IQueryable<T>> GetListAsQueryAsync<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : Entity<TKey>;
    Task<long> CountAsync<TKey, T>() where T : Entity<TKey>;
    Task<long> CountAsync<TKey, T>(Expression<Func<T, bool>> predicate) where T : Entity<TKey>;
    Task<TKey> CreateAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>;
    Task CreateRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>;
    Task UpdateAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>;
    Task UpdateRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>;
    Task DeleteAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>;
    Task DeleteByIdAsync<TKey, T>(TKey id, bool doPersist = true) where T : Entity<TKey>;
    Task DeleteRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>;
    Task PersistAsync();
}
