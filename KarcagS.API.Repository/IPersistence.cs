using System.Linq.Expressions;
using KarcagS.API.Data.Entities;

namespace KarcagS.API.Repository;

public interface IPersistence
{
    T Get<TKey, T>(TKey id) where T : Entity<TKey>;
    T? GetOptional<TKey, T>(TKey id) where T : Entity<TKey>;
    IEnumerable<T> GetAll<TKey, T>() where T : Entity<TKey>;
    IEnumerable<T> GetList<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : Entity<TKey>;
    IEnumerable<T> GetAllAsOrdered<TKey, T>(string orderBy, string direction) where T : Entity<TKey>;
    IEnumerable<T> GetOrderedList<TKey, T>(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null) where T : Entity<TKey>;
    IEnumerable<T> GetOrderedListByQuery<TKey, T>(IQueryable<T> queryable, string orderBy, string direction) where T : Entity<TKey>;
    IQueryable<T> GetAllAsQuery<TKey, T>() where T : Entity<TKey>;
    IQueryable<T> GetListAsQuery<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : Entity<TKey>;
    int Count<TKey, T>() where T : Entity<TKey>;
    int Count<TKey, T>(Expression<Func<T, bool>> predicate) where T : Entity<TKey>;
    TKey Create<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>;
    Task<TKey> CreateAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>;
    void CreateRange<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>;
    Task CreateRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>;
    void Update<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>;
    Task UpdateAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>;
    void UpdateRange<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>;
    Task UpdateRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>;
    void Delete<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>;
    Task DeleteAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>;
    void DeleteById<TKey, T>(TKey id, bool doPersist = true) where T : Entity<TKey>;
    Task DeleteByIdAsync<TKey, T>(TKey id, bool doPersist = true) where T : Entity<TKey>;
    void DeleteRange<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>;
    Task DeleteRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>;
    void Persist();
    Task PersistAsync();
}
