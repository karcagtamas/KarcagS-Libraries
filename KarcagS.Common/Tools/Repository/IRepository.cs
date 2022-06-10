using System.Linq.Expressions;
using KarcagS.Common.Tools.Entities;

namespace KarcagS.Common.Tools.Repository;

public interface IRepository<T, TKey> where T : class, IEntity<TKey>
{
    IEnumerable<T> GetAll();
    IEnumerable<T> GetList(Expression<Func<T, bool>> predicate);
    IEnumerable<T> GetList(Expression<Func<T, bool>> predicate, int? count);
    IEnumerable<T> GetList(Expression<Func<T, bool>> predicate, int? count, int? skip);
    IEnumerable<T> GetAllAsOrdered(string orderBy, string direction);
    IEnumerable<T> GetOrderedList(Expression<Func<T, bool>> predicate, string orderBy, string direction);
    IEnumerable<T> GetOrderedList(Expression<Func<T, bool>> predicate, int? count, string orderBy, string direction);
    IEnumerable<T> GetOrderedList(Expression<Func<T, bool>> predicate, int? count, int? skip, string orderBy, string direction);
    T Get(TKey id);
    void Update(T entity, bool doPersist = true);
    void UpdateRange(IEnumerable<T> entities, bool doPersist = true);
    TKey Create(T entity, bool doPersist = true);
    void CreateRange(IEnumerable<T> entities, bool doPersist = true);
    void Delete(T entity, bool doPersist = true);
    void DeleteRange(IEnumerable<T> entities, bool doPersist = true);
    void DeleteById(TKey id, bool doPersist = true);
    void Persist();
}
