using System.Globalization;
using System.Linq.Expressions;
using KarcagS.API.Data.Entities;
using KarcagS.API.Shared.Services;

namespace KarcagS.API.Repository;

/// <summary>
/// Repository manager
/// </summary>
/// <typeparam name="T">Type of Entity</typeparam>
/// <typeparam name="TKey">Type of key</typeparam>
public abstract class Repository<T, TKey>(ILoggerService logger, IPersistence persistence, string entity) : IRepository<T, TKey> where T : Entity<TKey>
{
    protected readonly ILoggerService Logger = logger;
    protected readonly string Entity = entity;
    protected readonly IPersistence Persistence = persistence;

    /// <summary>
    /// Get entity
    /// </summary>
    /// <param name="id">Identity id of entity</param>
    /// <returns>Entity with the given key</returns>
    public virtual Task<T> GetAsync(TKey id) => Persistence.GetAsync<TKey, T>(id);

    /// <summary>
    /// Get entity as optional value
    /// </summary>
    /// <param name="id">Identity id of entity</param>
    /// <returns>Entity with the given key or default</returns>
    public virtual Task<T?> GetOptionalAsync(TKey id) => Persistence.GetOptionalAsync<TKey, T>(id);

    /// <summary>
    /// Get all entity
    /// </summary>
    /// <returns>All existing entity</returns>
    public virtual Task<IEnumerable<T>> GetAllAsync() => Persistence.GetAllAsync<TKey, T>();

    /// <summary>
    /// Get list of entities.
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Filtered list of entities with max count and first skip.</returns>
    public virtual Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) => Persistence.GetListAsync<TKey, T>(predicate, count, skip);

    /// <summary>
    /// Get ordered list
    /// </summary>
    /// <param name="orderBy">Ordering by</param>
    /// <param name="direction">Order direction</param>
    /// <returns>Ordered all list</returns>
    public virtual Task<IEnumerable<T>> GetAllAsOrderedAsync(string orderBy, string direction) => Persistence.GetAllAsOrderedAsync<TKey, T>(orderBy, direction);

    /// <summary>
    /// Get ordered list
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="orderBy">Ordering by</param>
    /// <param name="direction">Order direction</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Ordered list</returns>
    public virtual Task<IEnumerable<T>> GetOrderedListAsync(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null) =>
        Persistence.GetOrderedListAsync<TKey, T>(predicate, orderBy, direction);

    /// <summary>
    /// Get all entities as query
    /// </summary>
    /// <returns>Queryable object</returns>
    public virtual Task<IQueryable<T>> GetAllAsQueryAsync() => Persistence.GetAllAsQueryAsync<TKey, T>();

    /// <summary>
    /// Get list of entities as query
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Queryable object</returns>
    public virtual Task<IQueryable<T>> GetListAsQueryAsync(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) => Persistence.GetListAsQueryAsync<TKey, T>(predicate, count, skip);

    /// <summary>
    /// Get count of entries
    /// </summary>
    /// <returns>Count of entries</returns>
    public virtual Task<long> CountAsync() => Persistence.CountAsync<TKey, T>();

    /// <summary>
    /// Get count of entries
    /// </summary>
    /// <param name="predicate">Filter predicated</param>
    /// <returns>Count of entries</returns>
    public virtual Task<long> CountAsync(Expression<Func<T, bool>> predicate) => Persistence.CountAsync<TKey, T>(predicate);

    /// <summary>
    /// Add entity Async
    /// </summary>
    /// <param name="entity">Entity object</param>
    /// <param name="doPersist">Do object persist</param>
    /// <returns>Newly created key</returns>
    public virtual Task<TKey> CreateAsync(T entity, bool doPersist = true) => Persistence.CreateAsync<TKey, T>(entity, doPersist);

    /// <summary>
    /// Add multiple entity Async
    /// </summary>
    /// <param name="entities">Entity objects</param>
    /// <param name="doPersist">Do object persist</param>
    public virtual Task CreateRangeAsync(IEnumerable<T> entities, bool doPersist = true) => Persistence.CreateRangeAsync<TKey, T>(entities, doPersist);

    /// <summary>
    /// Update entity Async
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="doPersist">Do Persist</param>
    public virtual Task UpdateAsync(T entity, bool doPersist = true) => Persistence.UpdateAsync<TKey, T>(entity, doPersist);

    /// <summary>
    /// Update multiple entity Async
    /// </summary>
    /// <param name="entities">Entities</param>
    /// <param name="doPersist">Do Persist</param>
    public virtual Task UpdateRangeAsync(IEnumerable<T> entities, bool doPersist = true) => Persistence.UpdateRangeAsync<TKey, T>(entities, doPersist);

    /// <summary>
    /// Remove entity Async
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="doPersist">Do Persist</param>
    public virtual Task DeleteAsync(T entity, bool doPersist = true) => Persistence.DeleteAsync<TKey, T>(entity, doPersist);

    /// <summary>
    /// Remove by Id Async
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <param name="doPersist">Do Persist</param>
    public virtual async Task DeleteByIdAsync(TKey id, bool doPersist = true) => await DeleteAsync(await GetAsync(id), doPersist);

    /// <summary>
    /// Remove range Async
    /// </summary>
    /// <param name="entities">Entities</param>
    /// <param name="doPersist">Do Persist</param>
    public virtual Task DeleteRangeAsync(IEnumerable<T> entities, bool doPersist = true) => Persistence.DeleteRangeAsync<TKey, T>(entities, doPersist);

    /// <summary>
    /// Save changes Async
    /// </summary>
    public virtual Task PersistAsync() => Persistence.PersistAsync();

    /// <summary>
    /// Generate entity service
    /// </summary>
    /// <returns>Entity Service name</returns>
    protected string GetService() => $"{Entity} Service";

    /// <summary>
    /// Generate event from action
    /// </summary>
    /// <param name="action">Action</param>
    /// <returns>Event name</returns>
    protected string GetEvent(string action) => $"{action} {Entity}";

    /// <summary>
    /// Generate entity error message
    /// </summary>
    /// <returns>Error message</returns>
    protected string GetEntityErrorMessage() => $"{Entity} does not exist";

    /// <summary>
    /// Determine arguments from entity by name
    /// </summary>
    /// <param name="nameList">Name list</param>
    /// <param name="firstType">First entity's type</param>
    /// <param name="entity">Entity</param>
    /// <returns>Declared argument value list</returns>
    protected List<string> DetermineArguments(IEnumerable<string> nameList, Type firstType, T entity)
    {
        var args = new List<string>();

        foreach (var i in nameList)
        {
            var propList = i.Split(".");
            var lastType = firstType;
            object? lastEntity = entity;

            foreach (var propElement in propList)
            {
                // Get inner entity from entity
                var prop = lastType.GetProperty(propElement);
                if (prop == null) continue;
                lastEntity = prop.GetValue(lastEntity);
                if (lastEntity != null)
                {
                    lastType = lastEntity.GetType();
                }
            }

            // Last entity is primitive (writeable)
            if (lastEntity != null && lastType != null)
            {
                if (lastType == typeof(string))
                {
                    args.Add((string)lastEntity);
                }
                else if (lastType == typeof(DateTime))
                {
                    args.Add(((DateTime)lastEntity).ToLongDateString());
                }
                else if (lastType == typeof(int))
                {
                    args.Add(((int)lastEntity).ToString());
                }
                else if (lastType == typeof(decimal))
                {
                    args.Add(((decimal)lastEntity).ToString(CultureInfo.CurrentCulture));
                }
                else if (lastType == typeof(double))
                {
                    args.Add(((double)lastEntity).ToString(CultureInfo.CurrentCulture));
                }
                else
                {
                    args.Add("");
                }
            }
            else
            {
                args.Add("");
            }
        }

        return args;
    }
}