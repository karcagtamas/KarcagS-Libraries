using System.Linq.Expressions;
using KarcagS.API.Data.Entities;
using KarcagS.API.Mongo.Configurations;
using KarcagS.API.Repository;
using KarcagS.API.Shared.Services;
using MongoDB.Driver;

namespace KarcagS.API.Mongo;

public class MongoPersistence<Configuration, TUserKey>(IMongoCollectionProvider<Configuration> collectionProvider, IMongoService<Configuration> mongoService, IUserProvider<TUserKey> userProvider)
    : AbstractPersistence<TUserKey>(userProvider) where Configuration : MongoCollectionConfiguration
{
    public override Task<long> CountAsync<TKey, T>() => ConstructCollection<TKey, T>().CountDocumentsAsync(p => true);

    public override Task<long> CountAsync<TKey, T>(Expression<Func<T, bool>> predicate) => ConstructCollection<TKey, T>().CountDocumentsAsync(predicate);

    protected override async Task<TKey> CreateActionAsync<TKey, T>(T entity, bool retrieveId)
    {
        await ConstructCollection<TKey, T>().InsertOneAsync(entity);

        return entity.Id;
    }

    protected override Task CreateRangeActionAsync<TKey, T>(IEnumerable<T> entities) => ConstructCollection<TKey, T>().InsertManyAsync(entities);

    protected override Task DeleteActionAsync<TKey, T>(T entity) => DeleteByIdAsync<TKey, T>(entity.Id);

    protected override Task DeleteByIdActionAsync<TKey, T>(TKey id) => ConstructCollection<TKey, T>().DeleteOneAsync(Builders<T>.Filter.Eq(x => x.Id, id));

    protected override Task DeleteRangeActionAsync<TKey, T>(IEnumerable<T> entities) => ConstructCollection<TKey, T>().DeleteManyAsync(Builders<T>.Filter.In(x => x.Id, entities.Select(x => x.Id).ToList()));

    public override async Task<IEnumerable<T>> GetAllAsOrderedAsync<TKey, T>(string orderBy, string direction) =>
        await GetOrderedListByQueryAsync<TKey, T>(await GetAllAsQueryAsync<TKey, T>(), orderBy, direction);

    public override async Task<IQueryable<T>> GetAllAsQueryAsync<TKey, T>() => (await ConstructCollection<TKey, T>().Find(x => true).ToListAsync()).AsQueryable();

    public override async Task<IEnumerable<T>> GetAllAsync<TKey, T>() => (await GetAllAsQueryAsync<TKey, T>()).AsEnumerable();

    public override Task<T> GetAsync<TKey, T>(TKey id) => ConstructCollection<TKey, T>().Find(Builders<T>.Filter.Eq(x => x.Id, id)).FirstAsync();

    public override async Task<IEnumerable<T>> GetListAsync<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) =>
        (await GetListAsQueryAsync<TKey, T>(predicate, count, skip)).AsEnumerable();

    public override async Task<T?> GetOptionalAsync<TKey, T>(TKey id) where T : class => await ConstructCollection<TKey, T>().Find(Builders<T>.Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();

    public override async Task<IEnumerable<T>> GetOrderedListAsync<TKey, T>(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null) =>
        await GetOrderedListByQueryAsync<TKey, T>(await GetListAsQueryAsync<TKey, T>(predicate, count, skip), orderBy, direction);

    public override Task PersistAsync() => Task.CompletedTask;

    protected override Task UpdateActionAsync<TKey, T>(T entity) => ConstructCollection<TKey, T>().ReplaceOneAsync(Builders<T>.Filter.Eq(x => x.Id, entity.Id), entity);

    protected override async Task UpdateRangeActionAsync<TKey, T>(IEnumerable<T> entities)
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

    protected override void HandleMissingId<TKey, T>(T entity)
    {
    }
}