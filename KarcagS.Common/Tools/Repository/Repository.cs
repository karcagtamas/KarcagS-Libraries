using System.Globalization;
using System.Linq.Expressions;
using KarcagS.Common.Attributes;
using KarcagS.Common.Tools.Entities;
using KarcagS.Common.Tools.Services;
using Microsoft.EntityFrameworkCore;

namespace KarcagS.Common.Tools.Repository;

/// <summary>
/// Repository manager
/// </summary>
/// <typeparam name="T">Type of Entity</typeparam>
public abstract class Repository<T, TKey, TUserKey> : IRepository<T, TKey>
    where T : class, IEntity<TKey>
{
    protected readonly DbContext Context;
    protected readonly ILoggerService Logger;
    protected readonly IUtilsService<TUserKey> Utils;
    protected readonly string Entity;

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
    }

    /// <summary>
    /// Add entity
    /// </summary>
    /// <param name="entity">Entity object</param>
    public virtual TKey Create(T entity, bool doPersist = true)
    {
        ApplyCreateModification(entity);

        Context.Set<T>().Add(entity);

        if (doPersist)
        {
            Persist();
        }

        return entity.Id;
    }

    /// <summary>
    /// Add multiple entity.
    /// </summary>
    /// <param name="entities">Entities</param>
    public virtual void CreateRange(IEnumerable<T> entities, bool doPersist = true)
    {
        var list = entities.ToList();

        list.ForEach(x => ApplyCreateModification(x));

        // Add
        Context.Set<T>().AddRange(list);

        if (doPersist)
        {
            // Save
            Persist();
        }
    }

    /// <summary>
    /// Save changes
    /// </summary>
    public virtual void Persist()
    {
        Context.SaveChanges();
    }

    /// <summary>
    /// Get entity
    /// </summary>
    /// <param name="id">Identity id of entity</param>
    /// <returns>Entity with the given keys</returns>
    public virtual T Get(TKey id)
    {
        var el = Context.Set<T>().Find(id);

        if (el is null)
        {
            throw new ArgumentException($"Element not found with id: {id}");
        }

        return el;
    }

    /// <summary>
    /// Get all entity
    /// </summary>
    /// <returns>All existing entity</returns>
    public virtual IEnumerable<T> GetAll()
    {
        // Get
        var list = Context.Set<T>().ToList();

        return list;
    }

    /// <summary>
    /// Get list of entities.
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Filtered list of entities with max count and first skip.</returns>
    public virtual IEnumerable<T> GetList(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null)
    {
        // Get
        var query = Context.Set<T>().Where(predicate);

        // Count
        if (count is not null)
        {
            query = query.Take((int)count);
        }

        // Skip
        if (skip is not null)
        {
            query = query.Skip((int)skip);
        }

        return query.ToList();
    }

    /// <summary>
    /// Remove entity.
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual void Delete(T entity, bool doPersist = true)
    {
        Context.Set<T>().Remove(entity);

        if (doPersist)
        {
            Persist();
        }
    }

    /// <summary>
    /// Remove by Id
    /// </summary>
    /// <param name="id">Id of entity</param>
    public virtual void DeleteById(TKey id, bool doPersist = true)
    {
        // Get entity
        var entity = Get(id);

        if (entity == null)
        {
            throw new ArgumentException($"Element not found with id: {id}");
        }

        // Remove
        Delete(entity, doPersist);
    }

    /// <summary>
    /// Remove range
    /// </summary>
    /// <param name="entities">Entities</param>
    public virtual void DeleteRange(IEnumerable<T> entities, bool doPersist = true)
    {
        // Remove range
        Context.Set<T>().RemoveRange(entities.ToList());

        if (doPersist)
        {
            // Save
            Persist();
        }
    }

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual void Update(T entity, bool doPersist = true)
    {
        ApplyUpdateModification(entity);

        Context.Set<T>().Update(entity);

        if (doPersist)
        {
            // Save
            Persist();
        }
    }

    /// <summary>
    /// Update multiple entity
    /// </summary>
    /// <param name="entities">Entities</param>
    public virtual void UpdateRange(IEnumerable<T> entities, bool doPersist = true)
    {
        var list = entities.ToList();

        list.ForEach(x => ApplyUpdateModification(x));

        // Update 
        Context.Set<T>().UpdateRange(list);

        if (doPersist)
        {
            // Save
            Persist();
        }
    }

    /// <summary>
    /// Generate entity service
    /// </summary>
    /// <returns>Entity Service name</returns>
    protected string GetService()
    {
        return $"{Entity} Service";
    }

    /// <summary>
    /// Generate event from action
    /// </summary>
    /// <param name="action">Action</param>
    /// <returns>Event name</returns>
    protected string GetEvent(string action)
    {
        return $"{action} {Entity}";
    }

    /// <summary>
    /// Generate entity error message
    /// </summary>
    /// <returns>Error message</returns>
    protected string GetEntityErrorMessage()
    {
        return $"{Entity} does not exist";
    }

    /// <summary>
    /// Generate notification action from action
    /// </summary>
    /// <param name="action">Action</param>
    /// <returns>Notification action</returns>
    private string GetNotificationAction(string action)
    {
        return string.Join("",
            GetEvent(action).Split(" ").Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));
    }

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

    /// <summary>
    /// Get ordered list
    /// </summary>
    /// <param name="orderBy">Ordering by</param>
    /// <param name="direction">Order direction</param>
    /// <returns>Ordered all list</returns>
    public virtual IEnumerable<T> GetAllAsOrdered(string orderBy, string direction)
    {
        if (string.IsNullOrEmpty(orderBy)) throw new ArgumentException("Order by value is empty or null");
        var type = typeof(T);
        var property = type.GetProperty(orderBy);

        if (property == null)
        {
            throw new ArgumentException("Property does not exist");
        }

        return direction switch
        {
            "asc" => GetAll().OrderBy(x => property.GetValue(x)),
            "desc" => GetAll().OrderByDescending(x => property.GetValue(x)),
            "none" => GetAll(),
            _ => throw new ArgumentException("Ordering direction does not exist")
        };

    }

    public virtual IEnumerable<T> GetOrderedList(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null)
    {
        if (string.IsNullOrEmpty(orderBy)) throw new ArgumentException("Order by value is empty or null");
        var type = typeof(T);
        var property = type.GetProperty(orderBy);

        if (property == null)
        {
            throw new ArgumentException("Property does not exist");
        }

        return direction switch
        {
            "asc" => GetList(predicate, count, skip).OrderBy(x => property.GetValue(x)),
            "desc" => GetList(predicate, count, skip).OrderByDescending(x => property.GetValue(x)),
            "none" => GetList(predicate, count, skip),
            _ => throw new ArgumentException("Ordering direction does not exist")
        };
    }

    private void ApplyCreateModification(T entity)
    {
        if (entity is IEntity<string> e)
        {
            if (string.IsNullOrEmpty(e.Id))
            {
                e.Id = Guid.NewGuid().ToString();
            }
        }

        if (entity is ILastUpdaterEntity<TUserKey> lue)
        {
            lue.LastUpdaterId = Utils.GetRequiredCurrentUserId();
        }

        var dateTime = DateTime.Now;
        if (entity is ICreationEntity creationEntity)
        {
            creationEntity.Creation = dateTime;
        }

        if (entity is ILastUpdateEntity lastUpdateEntity)
        {
            lastUpdateEntity.LastUpdate = dateTime;
        }

        var props = entity.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(UserAttribute)));
        props.ToList().ForEach(p =>
        {
            p.SetValue(entity, Utils.GetCurrentUserId());
        });
    }

    private void ApplyUpdateModification(T entity)
    {
        if (entity is ILastUpdaterEntity<TUserKey> lue)
        {
            lue.LastUpdaterId = Utils.GetRequiredCurrentUserId();
        }

        if (entity is ILastUpdateEntity lastUpdateEntity)
        {
            lastUpdateEntity.LastUpdate = DateTime.Now;
        }

        var props = entity.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(UserAttribute)));
        props.ToList().ForEach(p =>
        {
            var attr = Attribute.GetCustomAttribute(p, typeof(UserAttribute));

            if (attr is not null)
            {
                if (!((UserAttribute)attr).OnlyInit)
                {
                    p.SetValue(entity, Utils.GetCurrentUserId());
                }
            }
        });
    }
}
