using System.Linq.Expressions;
using KarcagS.API.Data.Entities;
using KarcagS.API.Repository.Attributes;
using KarcagS.API.Shared.Helpers;
using KarcagS.API.Shared.Services;
using KarcagS.Shared.Helpers;

namespace KarcagS.API.Repository;

public abstract class AbstractPersistence<TUserKey> : IPersistence
{
    protected readonly IUserProvider<TUserKey> UserProvider;

    protected AbstractPersistence(IUserProvider<TUserKey> userProvider)
    {
        UserProvider = userProvider;
    }

    public abstract Task<T> GetAsync<TKey, T>(TKey id) where T : Entity<TKey>;

    public abstract Task<T?> GetOptionalAsync<TKey, T>(TKey id) where T : Entity<TKey>;

    public abstract Task<IEnumerable<T>> GetAllAsync<TKey, T>() where T : Entity<TKey>;

    public abstract Task<IEnumerable<T>> GetListAsync<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : Entity<TKey>;

    public abstract Task<IEnumerable<T>> GetAllAsOrderedAsync<TKey, T>(string orderBy, string direction) where T : Entity<TKey>;

    public abstract Task<IEnumerable<T>> GetOrderedListAsync<TKey, T>(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null) where T : Entity<TKey>;

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

    public abstract Task<IQueryable<T>> GetAllAsQueryAsync<TKey, T>() where T : Entity<TKey>;

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

    public abstract Task<long> CountAsync<TKey, T>() where T : Entity<TKey>;

    public abstract Task<long> CountAsync<TKey, T>(Expression<Func<T, bool>> predicate) where T : Entity<TKey>;

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

        var id = await CreateActionAsync<TKey, T>(entity);

        if (doPersist)
        {
            await PersistAsync();
        }

        return id;
    }

    protected abstract Task<TKey> CreateActionAsync<TKey, T>(T entity) where T : Entity<TKey>;

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
        await CreateRangeActionAsync<TKey, T>(list);

        if (doPersist)
        {
            // Save
            await PersistAsync();
        }
    }

    protected abstract Task CreateRangeActionAsync<TKey, T>(IEnumerable<T> entities) where T : Entity<TKey>;

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

        await UpdateActionAsync<TKey, T>(entity);

        if (doPersist)
        {
            // Save
            await PersistAsync();
        }
    }

    protected abstract Task UpdateActionAsync<TKey, T>(T entity) where T : Entity<TKey>;

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
        await UpdateRangeActionAsync<TKey, T>(list);

        if (doPersist)
        {
            // Save
            await PersistAsync();
        }
    }

    protected abstract Task UpdateRangeActionAsync<TKey, T>(IEnumerable<T> entities) where T : Entity<TKey>;

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

        await DeleteActionAsync<TKey, T>(entity);

        if (doPersist)
        {
            await PersistAsync();
        }
    }

    protected abstract Task DeleteActionAsync<TKey, T>(T entity) where T : Entity<TKey>;

    /// <summary>
    /// Remove by Id
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="id">Id of entity</param>
    /// <param name="doPersist">Do object persist</param>
    public async Task DeleteByIdAsync<TKey, T>(TKey id, bool doPersist = true) where T : Entity<TKey>
    {
        await DeleteByIdActionAsync<TKey, T>(id);

        if (doPersist)
        {
            await PersistAsync();
        }
    }

    protected abstract Task DeleteByIdActionAsync<TKey, T>(TKey id) where T : Entity<TKey>;

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
        await DeleteRangeActionAsync<TKey, T>(entities.Where(ObjectHelper.IsNotNull).ToList());

        if (doPersist)
        {
            // Save
            await PersistAsync();
        }
    }

    protected abstract Task DeleteRangeActionAsync<TKey, T>(IEnumerable<T> entities) where T : Entity<TKey>;

    public abstract Task PersistAsync();

    protected async Task ApplyCreateModification<TKey, T>(T entity) where T : Entity<TKey>
    {
        HandleMissingId<TKey, T>(entity);

        // ReSharper disable once SuspiciousTypeConversion.Global
        if (entity is ILastUpdaterEntity<TUserKey> lue)
        {
            lue.LastUpdaterId = await UserProvider.GetRequiredCurrentUserId();
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

    protected async Task ApplyUpdateModification<TKey, T>(T entity) where T : Entity<TKey>
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (entity is ILastUpdaterEntity<TUserKey> lue)
        {
            lue.LastUpdaterId = await UserProvider.GetRequiredCurrentUserId();
        }

        // ReSharper disable once SuspiciousTypeConversion.Global
        if (entity is ILastUpdateEntity lastUpdateEntity)
        {
            lastUpdateEntity.LastUpdate = DateTime.Now;
        }

        await UpdateUserAttribute<T, TKey>(entity, true);
    }

    protected async Task UpdateUserAttribute<T, TKey>(T entity, bool update = false) where T : Entity<TKey>
    {
        var props = entity.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(UserAttribute)));

        foreach (var p in props.ToList())
        {
            var attr = (UserAttribute?)Attribute.GetCustomAttribute(p, typeof(UserAttribute));

            if (ObjectHelper.IsNotNull(attr))
            {
                var userId = await UserProvider.GetCurrentUserId();
                if ((attr.Force || ObjectHelper.IsNotNull(userId)) && (!update || !attr.OnlyInit))
                {
                    p.SetValue(entity, userId);
                }
            }
        }
    }

    protected virtual void HandleMissingId<TKey, T>(T entity)
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
    }
}