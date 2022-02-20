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
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Create(T entity);
    void CreateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    void DeleteById(TKey id);
    void Persist();
}
