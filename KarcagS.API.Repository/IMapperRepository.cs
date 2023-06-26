using System.Linq.Expressions;
using KarcagS.API.Data.Entities;

namespace KarcagS.API.Repository;

public interface IMapperRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    Task<IEnumerable<T>> GetAllMappedAsync<T>();
    Task<IEnumerable<T>> GetMappedListAsync<T>(Expression<Func<TEntity, bool>> expression, int? count = null, int? skip = null);
    Task<IEnumerable<T>> GetAllMappedAsOrderedAsync<T>(string orderBy, string direction);
    Task<IEnumerable<T>> GetMappedOrderedListAsync<T>(Expression<Func<TEntity, bool>> expression, string orderBy, string direction, int? count = null, int? skip = null);
    Task<IEnumerable<T>> MapFromQueryAsync<T>(IQueryable<TEntity> queryable);
    Task<T> GetMappedAsync<T>(TKey id);
    Task<T?> GetOptionalMappedAsync<T>(TKey id);
    Task<TKey> CreateFromModelAsync<TModel>(TModel model, bool doPersist = true);
    Task UpdateByModelAsync<TModel>(TKey id, TModel model, bool doPersist = true);
}