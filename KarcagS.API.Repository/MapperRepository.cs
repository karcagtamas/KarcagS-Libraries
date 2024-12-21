using System.Linq.Expressions;
using AutoMapper;
using KarcagS.API.Data.Entities;
using KarcagS.API.Shared.Helpers;
using KarcagS.API.Shared.Services;
using KarcagS.Shared.Helpers;

namespace KarcagS.API.Repository;

public abstract class MapperRepository<TEntity, TKey>(ILoggerService loggerService, IMapper mapper, IPersistence persistence, string entity) 
    : Repository<TEntity, TKey>(loggerService, persistence, entity), IMapperRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    protected readonly IMapper Mapper = mapper;

    public virtual Task<TKey> CreateFromModelAsync<TModel>(TModel model, bool doPersist = true) => CreateAsync(Mapper.Map<TEntity>(model), doPersist);

    public virtual async Task<IEnumerable<T>> GetAllMappedAsync<T>() => Mapper.Map<List<T>>(await GetAllAsync());

    public virtual async Task<T> GetMappedAsync<T>(TKey id) => Mapper.Map<T>(await GetAsync(id));

    public virtual async Task<T?> GetOptionalMappedAsync<T>(TKey id) => ObjectHelper.MapOrDefault(await GetOptionalAsync(id), Mapper.Map<T>);

    public virtual async Task<IEnumerable<T>> GetMappedListAsync<T>(Expression<Func<TEntity, bool>> expression, int? count = null, int? skip = null) => Mapper.Map<List<T>>(await GetListAsync(expression, count, skip));

    public virtual async Task<IEnumerable<T>> GetAllMappedAsOrderedAsync<T>(string orderBy, string direction) => Mapper.Map<List<T>>(await GetAllAsOrderedAsync(orderBy, direction));

    public virtual async Task UpdateByModelAsync<TModel>(TKey id, TModel model, bool doPersist = true)
    {
        ExceptionHelper.ThrowIfIsNull<TModel, ArgumentException>(model, "Model cannot be null");

        await UpdateAsync(Mapper.Map(model, await GetAsync(id)), doPersist);
    }

    public virtual async Task<IEnumerable<T>> GetMappedOrderedListAsync<T>(Expression<Func<TEntity, bool>> expression, string orderBy, string direction, int? count = null, int? skip = null) =>
        Mapper.Map<List<T>>(await GetOrderedListAsync(expression, orderBy, direction, count, skip));

    public virtual Task<IEnumerable<T>> MapFromQueryAsync<T>(IQueryable<TEntity> queryable) => Task.FromResult(Mapper.Map<List<T>>(queryable.ToList()).AsEnumerable());
}