using System.Linq.Expressions;
using KarcagS.API.Data;
using KarcagS.API.Data.Entities;

namespace KarcagS.API.Repository;

public interface IMapperRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    IEnumerable<T> GetAllMapped<T>();
    IEnumerable<T> GetMappedList<T>(Expression<Func<TEntity, bool>> expression, int? count = null, int? skip = null);
    IEnumerable<T> GetAllMappedAsOrdered<T>(string orderBy, string direction);
    IEnumerable<T> GetMappedOrderedList<T>(Expression<Func<TEntity, bool>> expression, string orderBy, string direction, int? count = null, int? skip = null);
    IEnumerable<T> MapFromQuery<T>(IQueryable<TEntity> queryable);
    T GetMapped<T>(TKey id);
    T? GetOptionalMapped<T>(TKey id);
    TKey CreateFromModel<TModel>(TModel model, bool doPersist = true);
    void UpdateByModel<TModel>(TKey id, TModel model, bool doPersist = true);
}