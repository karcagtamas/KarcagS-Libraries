using System.Linq.Expressions;
using KarcagS.API.Shared.Helpers;
using KarcagS.API.Shared.Services;
using KarcagS.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace KarcagS.API.Repository;

public class EFPersistence<TDatabaseContext, TUserKey> : AbstractPersistence<TUserKey> where TDatabaseContext : DbContext
{
    private readonly TDatabaseContext context;

    public EFPersistence(TDatabaseContext context, IUserProvider<TUserKey> userProvider) : base(userProvider)
    {
        this.context = context;
    }

    /// <summary>
    /// Get entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="id">Identity id of entity</param>
    /// <returns>Entity with the given key</returns>
    public override Task<T> GetAsync<TKey, T>(TKey id) => Task.FromResult(ObjectHelper.OrElseThrow(context.Set<T>().Find(id), () => new ArgumentException($"Element not found with id: {id}")));

    /// <summary>
    /// Get entity as optional value
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="id">Identity id of entity</param>
    /// <returns>Entity with the given key or default</returns>
    // public override Task<T?> GetOptionalAsync<TKey, T>(TKey id) => Task.FromResult(context.Find<T>(id));
    public override Task<T?> GetOptionalAsync<TKey, T>(TKey id) where T : class => Task.FromResult(context.Find<T>(id));

    /// <summary>
    /// Get all entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <returns>All existing entity</returns>
    public override Task<IEnumerable<T>> GetAllAsync<TKey, T>() => Task.FromResult(context.Set<T>().ToList().AsEnumerable());

    /// <summary>
    /// Get list of entities.
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Filtered list of entities with max count and first skip.</returns>
    public override async Task<IEnumerable<T>> GetListAsync<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) =>
        (await GetListAsQueryAsync<TKey, T>(predicate, count, skip)).ToList();

    /// <summary>
    /// Get ordered list
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="orderBy">Ordering by</param>
    /// <param name="direction">Order direction</param>
    /// <returns>Ordered all list</returns>
    public override async Task<IEnumerable<T>> GetAllAsOrderedAsync<TKey, T>(string orderBy, string direction) =>
        await GetOrderedListByQueryAsync<TKey, T>(await GetAllAsQueryAsync<TKey, T>(), orderBy, direction);

    /// <summary>
    /// Get ordered list
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="orderBy">Ordering by</param>
    /// <param name="direction">Order direction</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Ordered list</returns>
    public override async Task<IEnumerable<T>> GetOrderedListAsync<TKey, T>(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null) =>
        await GetOrderedListByQueryAsync<TKey, T>(await GetListAsQueryAsync<TKey, T>(predicate, count, skip), orderBy, direction);

    /// <summary>
    /// Get all entities as query
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <returns>Queryable object</returns>
    public override Task<IQueryable<T>> GetAllAsQueryAsync<TKey, T>() => Task.FromResult(context.Set<T>().AsQueryable());

    /// <summary>
    /// Get count of entries
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <returns>Count of entries</returns>
    public override Task<long> CountAsync<TKey, T>() => Task.FromResult((long)context.Set<T>().Count());

    /// <summary>
    /// Get count of entries
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>Count of entries</returns>
    public override Task<long> CountAsync<TKey, T>(Expression<Func<T, bool>> predicate) => Task.FromResult((long)context.Set<T>().Count(predicate));

    protected override async Task<TKey> CreateActionAsync<TKey, T>(T entity, bool retrieveId)
    {
        context.Set<T>().Add(entity);

        if (retrieveId)
        {
            await PersistAsync();
        }

        return entity.Id;
    }

    protected override Task CreateRangeActionAsync<TKey, T>(IEnumerable<T> entities)
    {
        context.Set<T>().AddRange(entities);

        return Task.CompletedTask;
    }

    protected override Task UpdateActionAsync<TKey, T>(T entity)
    {
        context.Set<T>().Update(entity);

        return Task.CompletedTask;
    }

    protected override Task UpdateRangeActionAsync<TKey, T>(IEnumerable<T> entities)
    {
        context.Set<T>().UpdateRange(entities);

        return Task.CompletedTask;
    }

    protected override Task DeleteActionAsync<TKey, T>(T entity)
    {
        context.Set<T>().Remove(entity);

        return Task.CompletedTask;
    }

    protected override async Task DeleteByIdActionAsync<TKey, T>(TKey id)
    {
        // Get entity
        var entity = await GetAsync<TKey, T>(id);

        ExceptionHelper.ThrowIfIsNull<T, ArgumentException>(entity, $"Element not found with id: {id}");

        // Remove
        await DeleteActionAsync<TKey, T>(entity);
    }

    protected override Task DeleteRangeActionAsync<TKey, T>(IEnumerable<T> entities)
    {
        // Remove range
        context.Set<T>().RemoveRange(entities);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Save changes
    /// </summary>
    public override async Task PersistAsync() => await context.SaveChangesAsync();
}