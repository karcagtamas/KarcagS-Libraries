using KarcagS.Common.Tools.Entities;
using System.Linq.Expressions;

namespace KarcagS.Common.Tools.Repository;
public interface IPersistence
{
    T Get<TKey, T>(TKey id) where T : class, IEntity<TKey>;
    T? GetOptional<TKey, T>(TKey id) where T : class, IEntity<TKey>;
    IEnumerable<T> GetAll<TKey, T>() where T : class, IEntity<TKey>;
    IEnumerable<T> GetList<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : class, IEntity<TKey>;
    IEnumerable<T> GetAllAsOrdered<TKey, T>(string orderBy, string direction) where T : class, IEntity<TKey>;
    IEnumerable<T> GetOrderedList<TKey, T>(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null) where T : class, IEntity<TKey>;
    TKey Create<TKey, T>(T entity, bool doPersist = true) where T : class, IEntity<TKey>;
    void CreateRange<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : class, IEntity<TKey>;
    void Update<TKey, T>(T entity, bool doPersist = true) where T : class, IEntity<TKey>;
    void UpdateRange<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : class, IEntity<TKey>;
    void Delete<TKey, T>(T entity, bool doPersist = true) where T : class, IEntity<TKey>;
    void DeleteById<TKey, T>(TKey id, bool doPersist = true) where T : class, IEntity<TKey>;
    void DeleteRange<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : class, IEntity<TKey>;
    void Persist();
}
