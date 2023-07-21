using System.Collections.Immutable;
using System.Linq.Expressions;
using KarcagS.API.Data.Entities;
using KarcagS.API.Mongo.Configurations;
using KarcagS.API.Repository;
using KarcagS.API.Shared.Helpers;
using KarcagS.Shared.Helpers;
using MongoDB.Driver;

namespace KarcagS.API.Mongo;

public class MongoPersistence<Configuration> : IPersistence where Configuration : MongoCollectionConfiguration
{
    private readonly IMongoCollectionProvider<Configuration> collectionProvider;
    private readonly IMongoService<Configuration> mongoService;

    public MongoPersistence(IMongoCollectionProvider<Configuration> collectionProvider, IMongoService<Configuration> mongoService)
    {
        this.collectionProvider = collectionProvider;
        this.mongoService = mongoService;
    }

    public Task<long> CountAsync<TKey, T>() where T : Entity<TKey> => ConstructCollection<TKey, T>().CountDocumentsAsync(p => true);

    public Task<long> CountAsync<TKey, T>(Expression<Func<T, bool>> predicate) where T : Entity<TKey> => ConstructCollection<TKey, T>().CountDocumentsAsync(predicate);

    public async Task<TKey> CreateAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>
    {
        await ConstructCollection<TKey, T>().InsertOneAsync(entity);

        return entity.Id;
    }

    public Task CreateRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey> => ConstructCollection<TKey, T>().InsertManyAsync(entities);

    public Task DeleteAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey> => DeleteByIdAsync<TKey, T>(entity.Id, doPersist);

    public Task DeleteByIdAsync<TKey, T>(TKey id, bool doPersist = true) where T : Entity<TKey> => ConstructCollection<TKey, T>().DeleteOneAsync(Builders<T>.Filter.Eq(x => x.Id, id));

    public Task DeleteRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey> =>
        ConstructCollection<TKey, T>().DeleteManyAsync(Builders<T>.Filter.In(x => x.Id, entities.Select(x => x.Id).ToList()));

    public async Task<IEnumerable<T>> GetAllAsOrderedAsync<TKey, T>(string orderBy, string direction) where T : Entity<TKey> =>
        await GetOrderedListByQueryAsync<TKey, T>(await GetAllAsQueryAsync<TKey, T>(), orderBy, direction);

    public async Task<IQueryable<T>> GetAllAsQueryAsync<TKey, T>() where T : Entity<TKey> => (await ConstructCollection<TKey, T>().Find(x => true).ToListAsync()).AsQueryable();

    public async Task<IEnumerable<T>> GetAllAsync<TKey, T>() where T : Entity<TKey> => (await GetAllAsQueryAsync<TKey, T>()).AsEnumerable();

    public Task<T> GetAsync<TKey, T>(TKey id) where T : Entity<TKey> => ConstructCollection<TKey, T>().Find(Builders<T>.Filter.Eq(x => x.Id, id)).FirstAsync();

    public async Task<IQueryable<T>> GetListAsQueryAsync<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : Entity<TKey>
    {
        var queryable = (await ConstructCollection<TKey, T>().Find(predicate).ToListAsync()).AsQueryable();

        if (count != null)
        {
            queryable = queryable.Take((int)count);
        }

        if (skip != null)
        {
            queryable = queryable.Skip((int)skip);
        }

        return queryable;
    }

    public async Task<IEnumerable<T>> GetListAsync<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : Entity<TKey> =>
        (await GetListAsQueryAsync<TKey, T>(predicate, count, skip)).AsEnumerable();

    public async Task<T?> GetOptionalAsync<TKey, T>(TKey id) where T : Entity<TKey> => await ConstructCollection<TKey, T>().Find(Builders<T>.Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();

    public async Task<IEnumerable<T>> GetOrderedListAsync<TKey, T>(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null) where T : Entity<TKey> =>
        await GetOrderedListByQueryAsync<TKey, T>(await GetListAsQueryAsync<TKey, T>(predicate, count, skip), orderBy, direction);

    public Task<IEnumerable<T>> GetOrderedListByQueryAsync<TKey, T>(IQueryable<T> queryable, string orderBy, string direction) where T : Entity<TKey>
    {
        ExceptionHelper.Throw(string.IsNullOrEmpty(orderBy), () => new ArgumentException("Order by value is empty or null"));

        if (direction != "asc" && direction != "desc")
        {
            return Task.FromResult(queryable.ToList().AsEnumerable());
        }

        var entityType = typeof(T);
        var propertyInfo = entityType.GetProperty(orderBy);

        if (ObjectHelper.IsNull(propertyInfo))
        {
            throw new ArgumentException("Invalid property name");
        }

        var param = Expression.Parameter(entityType);
        Expression body = Expression.Property(param, orderBy);
        var lambda = Expression.Lambda(body, param);

        var queryExpr = Expression.Call(typeof(Queryable), direction == "asc" ? "OrderBy" : "OrderByDescending", new[] { typeof(T), lambda.ReturnType }, queryable.Expression, lambda);

        return Task.FromResult(queryable.Provider.CreateQuery<T>(queryExpr).ToList().AsEnumerable());
    }

    public Task PersistAsync() => Task.CompletedTask;

    public Task UpdateAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey> => ConstructCollection<TKey, T>().ReplaceOneAsync(Builders<T>.Filter.Eq(x => x.Id, entity.Id), entity);

    public async Task UpdateRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>
    {
        foreach (var entity in entities)
        {
            await UpdateAsync<TKey, T>(entity);
        }
    }

    private IMongoCollection<T> ConstructCollection<TKey, T>() where T : Entity<TKey>
    {
        if (typeof(MongoEntity).IsAssignableFrom(typeof(T)))
        {
            return mongoService.GetDatabase().GetCollection<T>(collectionProvider.GetCollectionName<TKey, T>(mongoService.GetConfiguration().CollectionNames));
        }

        throw new Exception("Invalid entity type");
    }
}