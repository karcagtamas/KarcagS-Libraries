using System.Linq.Expressions;
using KarcagS.API.Data.Entities;

namespace KarcagS.API.Repository;

public interface IRepository<T, TKey> where T : Entity<TKey>
{
    IEnumerable<T> GetAll();
    IEnumerable<T> GetList(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null);
    IEnumerable<T> GetAllAsOrdered(string orderBy, string direction);
    IEnumerable<T> GetOrderedList(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null);
    IQueryable<T> GetAllAsQuery();
    IQueryable<T> GetListAsQuery(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null);
    int Count();
    int Count(Expression<Func<T, bool>> predicate);
    T Get(TKey id);
    T? GetOptional(TKey id);
    void Update(T entity, bool doPersist = true);
    Task UpdateAsync(T entity, bool doPersist = true);
    void UpdateRange(IEnumerable<T> entities, bool doPersist = true);
    Task UpdateRangeAsync(IEnumerable<T> entities, bool doPersist = true);
    TKey Create(T entity, bool doPersist = true);
    Task<TKey> CreateAsync(T entity, bool doPersist = true);
    void CreateRange(IEnumerable<T> entities, bool doPersist = true);
    Task CreateRangeAsync(IEnumerable<T> entities, bool doPersist = true);
    void Delete(T entity, bool doPersist = true);
    Task DeleteAsync(T entity, bool doPersist = true);
    void DeleteRange(IEnumerable<T> entities, bool doPersist = true);
    Task DeleteRangeAsync(IEnumerable<T> entities, bool doPersist = true);
    void DeleteById(TKey id, bool doPersist = true);
    Task DeleteByIdAsync(TKey id, bool doPersist = true);
    void Persist();
    Task PersistAsync();
}
