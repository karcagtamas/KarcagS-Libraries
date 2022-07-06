using System.Globalization;
using System.Linq.Expressions;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.Entities;
using KarcagS.Common.Tools.Services;
using Microsoft.EntityFrameworkCore;

namespace KarcagS.Common.Tools.Repository;

/// <summary>
/// Repository manager
/// </summary>
/// <typeparam name="T">Type of Entity</typeparam>
/// <typeparam name="TKey">Type of key</typeparam>
/// <typeparam name="TUserKey">Type of user entity key</typeparam>
public abstract class Repository<T, TKey, TUserKey> : IRepository<T, TKey>
    where T : class, IEntity<TKey>
{
    protected readonly DbContext Context;
    protected readonly ILoggerService Logger;
    protected readonly IUtilsService<TUserKey> Utils;
    protected readonly string Entity;
    protected readonly IPersistence Persistence;

    /// <summary>
    /// Init
    /// </summary>
    /// <param name="context">Database Context</param>
    /// <param name="logger">Logger Service</param>
    /// <param name="utils">Utils Service</param>
    /// <param name="mapper">Mapper</param>
    /// <param name="entity">Entity name</param>
    protected Repository(DbContext context, ILoggerService logger, IUtilsService<TUserKey> utils, string entity)
    {
        Context = context;
        Logger = logger;
        Utils = utils;
        Entity = entity;
        Persistence = new Persistence<TUserKey>(context, utils);
    }

    /// <summary>
    /// Get entity
    /// </summary>
    /// <param name="id">Identity id of entity</param>
    /// <returns>Entity with the given key</returns>
    public virtual T Get(TKey id) => Persistence.Get<TKey, T>(id);

    /// <summary>
    /// Get entity as optional value
    /// </summary>
    /// <param name="id">Identity id of entity</param>
    /// <returns>Entity with the given key or default</returns>
    public virtual T? GetOptional(TKey id) => Persistence.GetOptional<TKey, T>(id);

    /// <summary>
    /// Get all entity
    /// </summary>
    /// <returns>All existing entity</returns>
    public virtual IEnumerable<T> GetAll() => Persistence.GetAll<TKey, T>();

    /// <summary>
    /// Get list of entities.
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Filtered list of entities with max count and first skip.</returns>
    public virtual IEnumerable<T> GetList(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) => Persistence.GetList<TKey, T>(predicate, count, skip);

    /// <summary>
    /// Get ordered list
    /// </summary>
    /// <param name="orderBy">Ordering by</param>
    /// <param name="direction">Order direction</param>
    /// <returns>Ordered all list</returns>
    public virtual IEnumerable<T> GetAllAsOrdered(string orderBy, string direction) => Persistence.GetAllAsOrdered<TKey, T>(orderBy, direction);

    /// <summary>
    /// Get ordered list
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="orderBy">Ordering by</param>
    /// <param name="direction">Order direction</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Ordered list</returns>
    public virtual IEnumerable<T> GetOrderedList(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null) => Persistence.GetOrderedList<TKey, T>(predicate, orderBy, direction);

    /// <summary>
    /// Add entity
    /// </summary>
    /// <param name="entity">Entity object</param>
    /// <param name="doPersist">Do object persist</param>
    /// <returns>Newly created key</returns>
    public virtual TKey Create(T entity, bool doPersist = true) => Persistence.Create<TKey, T>(entity, doPersist);

    /// <summary>
    /// Add multiple entity.
    /// </summary>
    /// <param name="entities">Entity objects</param>
    /// <param name="doPersist">Do object persist</param>
    public virtual void CreateRange(IEnumerable<T> entities, bool doPersist = true) => Persistence.CreateRange<TKey, T>(entities, doPersist);

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual void Update(T entity, bool doPersist = true) => Persistence.Update<TKey, T>(entity, doPersist);

    /// <summary>
    /// Update multiple entity
    /// </summary>
    /// <param name="entities">Entities</param>
    public virtual void UpdateRange(IEnumerable<T> entities, bool doPersist = true) => Persistence.UpdateRange<TKey, T>(entities, doPersist);

    /// <summary>
    /// Remove entity.
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual void Delete(T entity, bool doPersist = true) => Persistence.Delete<TKey, T>(entity, doPersist);

    /// <summary>
    /// Remove by Id
    /// </summary>
    /// <param name="id">Id of entity</param>
    public virtual void DeleteById(TKey id, bool doPersist = true)
    {
        // Get entity
        var entity = Get(id);

        ExceptionHelper.ThrowIfIsNull<T, ArgumentException>(entity, $"Element not found with id: {id}");

        // Remove
        Delete(entity, doPersist);
    }

    /// <summary>
    /// Remove range
    /// </summary>
    /// <param name="entities">Entities</param>
    public virtual void DeleteRange(IEnumerable<T> entities, bool doPersist = true) => Persistence.DeleteRange<TKey, T>(entities, doPersist);

    /// <summary>
    /// Save changes
    /// </summary>
    public virtual void Persist() => Persistence.Persist();

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
    /// Generate notification action from action
    /// </summary>
    /// <param name="action">Action</param>
    /// <returns>Notification action</returns>
    private string GetNotificationAction(string action) => string.Join("", GetEvent(action).Split(" ").Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));

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
