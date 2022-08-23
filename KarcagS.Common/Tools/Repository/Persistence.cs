using KarcagS.Common.Attributes;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.Entities;
using KarcagS.Common.Tools.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KarcagS.Common.Tools.Repository;

public class Persistence<TUserKey> : IPersistence
{
    private readonly DbContext context;
    private readonly IUtilsService<TUserKey> utils;

    public Persistence(DbContext context, IUtilsService<TUserKey> utils)
    {
        this.context = context;
        this.utils = utils;
    }

    /// <summary>
    /// Get entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="id">Identity id of entity</param>
    /// <returns>Entity with the given key</returns>
    public T Get<TKey, T>(TKey id) where T : class, IEntity<TKey> => ObjectHelper.OrElseThrow(context.Set<T>().Find(id), () => new ArgumentException($"Element not found with id: {id}"));

    /// <summary>
    /// Get entity as optional value
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="id">Identity id of entity</param>
    /// <returns>Entity with the given key or default</returns>
    public T? GetOptional<TKey, T>(TKey id) where T : class, IEntity<TKey> => context.Set<T>().Find(id);

    /// <summary>
    /// Get all entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <returns>All existing entity</returns>
    public IEnumerable<T> GetAll<TKey, T>() where T : class, IEntity<TKey> => context.Set<T>().ToList();

    /// <summary>
    /// Get list of entities.
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Filtered list of entities with max count and first skip.</returns>
    public IEnumerable<T> GetList<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : class, IEntity<TKey> => GetListAsQuery<TKey, T>(predicate, count, skip).ToList();

    /// <summary>
    /// Get ordered list
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="orderBy">Ordering by</param>
    /// <param name="direction">Order direction</param>
    /// <returns>Ordered all list</returns>
    public IEnumerable<T> GetAllAsOrdered<TKey, T>(string orderBy, string direction) where T : class, IEntity<TKey>
    {
        ExceptionHelper.Throw(string.IsNullOrEmpty(orderBy), () => new ArgumentException("Order by value is empty or null"));
        var type = typeof(T);
        var property = ObjectHelper.OrElseThrow(type.GetProperty(orderBy), () => new ArgumentException("Property does not exist"));

        return direction switch
        {
            "asc" => GetAllAsQuery<TKey, T>().OrderBy(x => property.GetValue(x)).ToList(),
            "desc" => GetAllAsQuery<TKey, T>().OrderByDescending(x => property.GetValue(x)).ToList(),
            "none" => GetAll<TKey, T>(),
            _ => throw new ArgumentException("Ordering direction does not exist")
        };
    }

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
    public IEnumerable<T> GetOrderedList<TKey, T>(Expression<Func<T, bool>> predicate, string orderBy, string direction, int? count = null, int? skip = null) where T : class, IEntity<TKey>
    {
        ExceptionHelper.Throw(string.IsNullOrEmpty(orderBy), () => new ArgumentException("Order by value is empty or null"));
        var type = typeof(T);
        var property = ObjectHelper.OrElseThrow(type.GetProperty(orderBy), () => new ArgumentException("Property does not exist"));

        return direction switch
        {
            "asc" => GetListAsQuery<TKey, T>(predicate, count, skip).OrderBy(x => property.GetValue(x)).ToList(),
            "desc" => GetListAsQuery<TKey, T>(predicate, count, skip).OrderByDescending(x => property.GetValue(x)).ToList(),
            "none" => GetList<TKey, T>(predicate, count, skip),
            _ => throw new ArgumentException("Ordering direction does not exist")
        };
    }

    /// <summary>
    /// Get all entities as query
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <returns>Queryable object</returns>
    public IQueryable<T> GetAllAsQuery<TKey, T>() where T : class, IEntity<TKey> => context.Set<T>().AsQueryable();

    /// <summary>
    /// Get list of entities as query
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="count">Max result count.</param>
    /// <param name="skip">Skipped element number.</param>
    /// <returns>Queryable object</returns>
    public IQueryable<T> GetListAsQuery<TKey, T>(Expression<Func<T, bool>> predicate, int? count = null, int? skip = null) where T : class, IEntity<TKey>
    {
        // Get
        var query = GetAllAsQuery<TKey, T>().Where(predicate);

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
    public int Count<TKey, T>() where T : class, IEntity<TKey> => context.Set<T>().Count();

    /// <summary>
    /// Get count of entries
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>Count of entries</returns>
    public int Count<TKey, T>(Expression<Func<T, bool>> predicate) where T : class, IEntity<TKey> => context.Set<T>().Count(predicate);

    /// <summary>
    /// Add entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="entity">Entity object</param>
    /// <param name="doPersist">Do object persist</param>
    /// <returns>Newly created key</returns>
    public TKey Create<TKey, T>(T entity, bool doPersist = true) where T : class, IEntity<TKey>
    {
        ExceptionHelper.ThrowIfIsNull<T, ArgumentException>(entity, "Entity cannot be null");

        ApplyCreateModification<TKey, T>(entity);

        context.Set<T>().Add(entity);

        if (doPersist)
        {
            Persist();
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
    public void CreateRange<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : class, IEntity<TKey>
    {
        var list = entities.Where(x => ObjectHelper.IsNotNull(x)).ToList();

        list.ForEach(x => ApplyCreateModification<TKey, T>(x));

        // Add
        context.Set<T>().AddRange(list);

        if (doPersist)
        {
            // Save
            Persist();
        }
    }

    /// <summary>
    /// Update entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="entity">Entity object</param>
    /// <param name="doPersist">Do object persist</param>
    public void Update<TKey, T>(T entity, bool doPersist = true) where T : class, IEntity<TKey>
    {
        ExceptionHelper.ThrowIfIsNull<T, ArgumentException>(entity, "Entity cannot be null");

        ApplyUpdateModification<TKey, T>(entity);

        context.Set<T>().Update(entity);

        if (doPersist)
        {
            // Save
            Persist();
        }
    }

    /// <summary>
    /// Update multiple entity
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="entities">Entity objects</param>
    /// <param name="doPersist">Do object persist</param>
    public void UpdateRange<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : class, IEntity<TKey>
    {
        var list = entities.Where(x => ObjectHelper.IsNotNull(x)).ToList();

        list.ForEach(x => ApplyUpdateModification<TKey, T>(x));

        // Update 
        context.Set<T>().UpdateRange(list);

        if (doPersist)
        {
            // Save
            Persist();
        }
    }

    /// <summary>
    /// Remove entity.
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="entity">Entity object</param>
    /// <param name="doPersist">Do object persist</param>
    public void Delete<TKey, T>(T entity, bool doPersist = true) where T : class, IEntity<TKey>
    {
        ExceptionHelper.ThrowIfIsNull<T, ArgumentException>(entity, "Entity cannot be null");

        context.Set<T>().Remove(entity);

        if (doPersist)
        {
            Persist();
        }
    }

    /// <summary>
    /// Remove by Id
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="id">Id of entity</param>
    /// <param name="doPersist">Do object persist</param>
    public void DeleteById<TKey, T>(TKey id, bool doPersist = true) where T : class, IEntity<TKey>
    {
        // Get entity
        var entity = Get<TKey, T>(id);

        ExceptionHelper.ThrowIfIsNull<T, ArgumentException>(entity, $"Element not found with id: {id}");

        // Remove
        Delete<TKey, T>(entity, doPersist);
    }

    /// <summary>
    /// Remove range
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="entities">Entity objects</param>
    /// <param name="doPersist">Do object persist</param>
    public void DeleteRange<TKey, T>(IEnumerable<T> entities, bool doPersist = true) where T : class, IEntity<TKey>
    {
        // Remove range
        context.Set<T>().RemoveRange(entities.Where(x => ObjectHelper.IsNotNull(x)).ToList());

        if (doPersist)
        {
            // Save
            Persist();
        }
    }

    /// <summary>
    /// Save changes
    /// </summary>
    public void Persist() => context.SaveChanges();

    private void ApplyCreateModification<TKey, T>(T entity) where T : class, IEntity<TKey>
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
            lue.LastUpdaterId = utils.GetRequiredCurrentUserId();
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
            p.SetValue(entity, utils.GetCurrentUserId());
        });
    }

    private void ApplyUpdateModification<TKey, T>(T entity) where T : class, IEntity<TKey>
    {
        if (entity is ILastUpdaterEntity<TUserKey> lue)
        {
            lue.LastUpdaterId = utils.GetRequiredCurrentUserId();
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
                    p.SetValue(entity, utils.GetCurrentUserId());
                }
            }
        });
    }
}
