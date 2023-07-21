using System.Linq.Expressions;
using KarcagS.API.Data.Entities;
using KarcagS.API.Repository.Attributes;
using KarcagS.API.Shared.Helpers;
using KarcagS.API.Shared.Services;
using KarcagS.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace KarcagS.API.Repository;

public class EFPersistence<TDatabaseContext, TUserKey> : IPersistence where TDatabaseContext : DbContext
{
    private readonly TDatabaseContext context;
    private readonly IUserProvider<TUserKey> userProvider;

    public EFPersistence(TDatabaseContext context, IUserProvider<TUserKey> userProvider)
    {
        this.context = context;
        this.userProvider = userProvider;
    }

    /// <summary>
    /// Get entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="id">Identity id of entity</param>
    /// <returns>Entity with the given key</returns>
    public Task<T> GetAsync<TKey, T>(TKey id) where T : Entity<TKey> => Task.FromResult(ObjectHelper.OrElseThrow(context.Set<T>().Find(id), () => new ArgumentException($"Element not found with id: {id}")));

    /// <summary>
    /// Get entity as optional value
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="id">Identity id of entity</param>
    /// <returns>Entity with the given key or default</returns>
    public Task<T?> GetOptionalAsync<TKey, T>(TKey id) where T : Entity<TKey> => Task.FromResult(context.Set<T>().Find(id));

    /// <summary>
    /// Get all entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <returns>All existing entity</returns>
    public Task<IEnumerable<T>> GetAllAsync<TKey, T>() where T : Entity<TKey> => Task.FromResult(context.Set<T>().ToList().AsEnumerable());

    /// <summary>
    /// Get list of entities.
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Filtered list of entities with max count and first skip.</returns>
    public async Task<IEnumerable<T>> GetListAsync<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : Entity<TKey> =>
        (await GetListAsQueryAsync<TKey, T>(predicate, count, skip)).ToList();

    /// <summary>
    /// Get ordered list
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="orderBy">Ordering by</param>
    /// <param name="direction">Order direction</param>
    /// <returns>Ordered all list</returns>
    public async Task<IEnumerable<T>> GetAllAsOrderedAsync<TKey, T>(string orderBy, string direction) where T : Entity<TKey> =>
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

    /// <summary>
    /// Get all entities as query
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <returns>Queryable object</returns>
    public Task<IQueryable<T>> GetAllAsQueryAsync<TKey, T>() where T : Entity<TKey> => Task.FromResult(context.Set<T>().AsQueryable());

    /// <summary>
    /// Get list of entities as query
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Queryable object</returns>
    public async Task<IQueryable<T>> GetListAsQueryAsync<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : Entity<TKey>
    {
        // Get
        var query = (await GetAllAsQueryAsync<TKey, T>()).Where(predicate);

        // Skip
        if (skip is not null)
        {
            query = query.Skip((int)skip);
        }

        // Count
        if (count is not null)
        {
            query = query.Take((int)count);
        }

        return query;
    }

    /// <summary>
    /// Get count of entries
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <returns>Count of entries</returns>
    public Task<long> CountAsync<TKey, T>() where T : Entity<TKey> => Task.FromResult((long)context.Set<T>().Count());

    /// <summary>
    /// Get count of entries
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>Count of entries</returns>
    public Task<long> CountAsync<TKey, T>(Expression<Func<T, bool>> predicate) where T : Entity<TKey> => Task.FromResult((long)context.Set<T>().Count(predicate));

    /// <summary>
    /// Add entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="entity">Entity object</param>
    /// <param name="doPersist">Do object persist</param>
    /// <returns>Newly created key</returns>
    public async Task<TKey> CreateAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>
    {
        ExceptionHelper.ThrowIfIsNull<T, ArgumentException>(entity, "Entity cannot be null");

        await ApplyCreateModification<TKey, T>(entity);

        context.Set<T>().Add(entity);

        if (doPersist)
        {
            await PersistAsync();
        }

        return entity.Id;
    }

    /// <summary>
    /// Add multiple entity.
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="entities">Entity objects</param>
    /// <param name="doPersist">Do object persist</param>
    public async Task CreateRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>
    {
        var list = entities.Where(ObjectHelper.IsNotNull).ToList();

        foreach (var item in list)
        {
            await ApplyCreateModification<TKey, T>(item);
        }

        // Add
        context.Set<T>().AddRange(list);

        if (doPersist)
        {
            // Save
            await PersistAsync();
        }
    }

    /// <summary>
    /// Update entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="entity">Entity object</param>
    /// <param name="doPersist">Do object persist</param>
    public async Task UpdateAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>
    {
        ExceptionHelper.ThrowIfIsNull<T, ArgumentException>(entity, "Entity cannot be null");

        await ApplyUpdateModification<TKey, T>(entity);

        context.Set<T>().Update(entity);

        if (doPersist)
        {
            // Save
            await PersistAsync();
        }
    }

    /// <summary>
    /// Update multiple entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="entities">Entity objects</param>
    /// <param name="doPersist">Do object persist</param>
    public async Task UpdateRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>
    {
        var list = entities.Where(ObjectHelper.IsNotNull).ToList();

        foreach (var item in list)
        {
            await ApplyUpdateModification<TKey, T>(item);
        }

        // Update 
        context.Set<T>().UpdateRange(list);

        if (doPersist)
        {
            // Save
            await PersistAsync();
        }
    }

    /// <summary>
    /// Remove entity.
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="entity">Entity object</param>
    /// <param name="doPersist">Do object persist</param>
    public async Task DeleteAsync<TKey, T>(T entity, bool doPersist = true) where T : Entity<TKey>
    {
        ExceptionHelper.ThrowIfIsNull<T, ArgumentException>(entity, "Entity cannot be null");

        context.Set<T>().Remove(entity);

        if (doPersist)
        {
            await PersistAsync();
        }
    }

    /// <summary>
    /// Remove by Id
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="id">Id of entity</param>
    /// <param name="doPersist">Do object persist</param>
    public async Task DeleteByIdAsync<TKey, T>(TKey id, bool doPersist = true) where T : Entity<TKey>
    {
        // Get entity
        var entity = await GetAsync<TKey, T>(id);

        ExceptionHelper.ThrowIfIsNull<T, ArgumentException>(entity, $"Element not found with id: {id}");

        // Remove
        await DeleteAsync<TKey, T>(entity, doPersist);
    }

    /// <summary>
    /// Remove range
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="entities">Entity objects</param>
    /// <param name="doPersist">Do object persist</param>
    public async Task DeleteRangeAsync<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : Entity<TKey>
    {
        // Remove range
        context.Set<T>().RemoveRange(entities.Where(ObjectHelper.IsNotNull).ToList());

        if (doPersist)
        {
            // Save
            await PersistAsync();
        }
    }

    /// <summary>
    /// Save changes
    /// </summary>
    public async Task PersistAsync() => await context.SaveChangesAsync();

    private async Task ApplyCreateModification<TKey, T>(T entity) where T : Entity<TKey>
    {
        if (entity is Entity<string> e)
        {
            if (string.IsNullOrEmpty(e.Id))
            {
                e.Id = Guid.NewGuid().ToString();
            }
        }

        if (entity is Entity<Guid> eg)
        {
            if (eg.Id != Guid.Empty)
            {
                eg.Id = Guid.NewGuid();
            }
        }

        // ReSharper disable once SuspiciousTypeConversion.Global
        if (entity is ILastUpdaterEntity<TUserKey> lue)
        {
            lue.LastUpdaterId = await userProvider.GetRequiredCurrentUserId();
        }

        var dateTime = DateTime.Now;
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (entity is ICreationEntity creationEntity)
        {
            creationEntity.Creation = dateTime;
        }

        // ReSharper disable once SuspiciousTypeConversion.Global
        if (entity is ILastUpdateEntity lastUpdateEntity)
        {
            lastUpdateEntity.LastUpdate = dateTime;
        }

        await UpdateUserAttribute<T, TKey>(entity);
    }

    private async Task ApplyUpdateModification<TKey, T>(T entity) where T : Entity<TKey>
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (entity is ILastUpdaterEntity<TUserKey> lue)
        {
            lue.LastUpdaterId = await userProvider.GetRequiredCurrentUserId();
        }

        // ReSharper disable once SuspiciousTypeConversion.Global
        if (entity is ILastUpdateEntity lastUpdateEntity)
        {
            lastUpdateEntity.LastUpdate = DateTime.Now;
        }

        await UpdateUserAttribute<T, TKey>(entity, true);
    }

    private async Task UpdateUserAttribute<T, TKey>(T entity, bool update = false) where T : Entity<TKey>
    {
        var props = entity.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(UserAttribute)));

        foreach (var p in props.ToList())
        {
            var attr = (UserAttribute?)Attribute.GetCustomAttribute(p, typeof(UserAttribute));

            if (ObjectHelper.IsNotNull(attr))
            {
                var userId = await userProvider.GetCurrentUserId();
                if ((attr.Force || ObjectHelper.IsNotNull(userId)) && (!update || !attr.OnlyInit))
                {
                    p.SetValue(entity, userId);
                }
            }
        }
    }
}