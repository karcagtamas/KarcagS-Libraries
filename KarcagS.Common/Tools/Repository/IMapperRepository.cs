using System.Linq.Expressions;
using KarcagS.Common.Tools.Entities;

namespace KarcagS.Common.Tools.Repository;

public interface IMapperRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    IEnumerable<T> GetAllMapped<T>();
    IEnumerable<T> GetMappedList<T>(Expression<Func<TEntity, bool>> expression);
    IEnumerable<T> GetMappedList<T>(Expression<Func<TEntity, bool>> expression, int? count);
    IEnumerable<T> GetMappedList<T>(Expression<Func<TEntity, bool>> expression, int? count, int? skip);
    IEnumerable<T> GetAllMappedAsOrdered<T>(string orderBy, string direction);
    IEnumerable<T> GetMappedOrderedList<T>(Expression<Func<TEntity, bool>> expression, string orderBy, string direction);
    IEnumerable<T> GetMappedOrderedList<T>(Expression<Func<TEntity, bool>> expression, int? count, string orderBy, string direction);
    IEnumerable<T> GetMappedOrderedList<T>(Expression<Func<TEntity, bool>> expression, int? count, int? skip, string orderBy, string direction);
    T GetMapped<T>(TKey id);
    TKey CreateFromModel<TModel>(TModel model, bool doPersist = true);
    void UpdateByModel<TModel>(TKey id, TModel model, bool doPersist = true);
}