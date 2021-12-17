using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Karcags.Common.Tools.Services;
using Microsoft.EntityFrameworkCore;

namespace Karcags.Common.Tools
{
    /// <summary>
    /// Repository manager
    /// </summary>
    /// <typeparam name="TEntity">Type of Entity</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DbContext _context;
        protected readonly ILoggerService Logger;
        protected readonly IUtilsService Utils;
        protected readonly IMapper Mapper;
        protected readonly string Entity;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="logger">Logger Service</param>
        /// <param name="utils">Utils Service</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="entity">Entity name</param>
        protected Repository(DbContext context, ILoggerService logger, IUtilsService utils,
            IMapper mapper, string entity)
        {
            _context = context;
            Logger = logger;
            Utils = utils;
            Mapper = mapper;
            Entity = entity;
        }
        
        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity object</param>
        public void Add(TEntity entity)
        {
            // Add
            _context.Set<TEntity>().Add(entity);

            // Save
            Complete();
        }

        public void AddRange<TU>(IEnumerable<TEntity> entities) where TU : class, IEntity
        {
            // Add default data automatically
            entities = entities.Select(x =>
            {
                var type = x.GetType();
                var creator = type.GetProperty("CreatorId");
                if (creator != null)
                {
                    var user = Utils.GetCurrentUser<TU>();
                    creator.SetValue(x, user.Id, null);
                }

                var lastUpdater = type.GetProperty("LastUpdaterId");
                if (lastUpdater != null)
                {
                    var user = Utils.GetCurrentUser<TU>();
                    lastUpdater.SetValue(x, user.Id, null);
                }

                var userField = type.GetProperty("UserId");
                if (userField != null)
                {
                    var user = Utils.GetCurrentUser<TU>();
                    userField.SetValue(x, user.Id, null);
                }

                var owner = type.GetProperty("OwnerId");
                if (owner != null)
                {
                    var user = Utils.GetCurrentUser<TU>();
                    owner.SetValue(x, user.Id, null);
                }

                return x;
            });

            // Add
            var entityList = entities.ToList();
            _context.Set<TEntity>().AddRange(entityList);

            // Save
            Complete();
        }

        /// <summary>
        /// Add entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <typeparam name="T">Type of mappable entity</typeparam>
        public void Add<T>(T entity)
        {
            Add(Mapper.Map<TEntity>(entity));
        }

        /// <summary>
        /// Add multiple entity.
        /// </summary>
        /// <param name="entities">Entities</param>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            // Add
            var entityList = entities.ToList();
            _context.Set<TEntity>().AddRange(entityList);

            // Save
            Complete();
        }

        public void Add<TU>(TEntity entity) where TU : class, IEntity
        {
            // Add default data automatically
            var type = entity.GetType();
            var creator = type.GetProperty("CreatorId");
            if (creator != null)
            {
                var user = Utils.GetCurrentUser<TU>();
                creator.SetValue(entity, user.Id, null);
            }

            var lastUpdater = type.GetProperty("LastUpdaterId");
            if (lastUpdater != null)
            {
                var user = Utils.GetCurrentUser<TU>();
                lastUpdater.SetValue(entity, user.Id, null);
            }

            var userField = type.GetProperty("UserId");
            if (userField != null)
            {
                var user = Utils.GetCurrentUser<TU>();
                userField.SetValue(entity, user.Id, null);
            }

            var owner = type.GetProperty("OwnerId");
            if (owner != null)
            {
                var user = Utils.GetCurrentUser<TU>();
                owner.SetValue(entity, user.Id, null);
            }

            // Add
            _context.Set<TEntity>().Add(entity);

            // Save
            Complete();
        }

        /// <summary>
        /// Add multiple entity
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <typeparam name="T">Type of mappable entities</typeparam>
        public void AddRange<T>(IEnumerable<T> entities)
        {
            AddRange(Mapper.Map<IEnumerable<TEntity>>(entities));
        }

        public void Add<T, TU>(T entity) where TU : class, IEntity
        {
            Add<TU>(Mapper.Map<TEntity>(entity));
        }

        public void AddRange<T, TU>(IEnumerable<T> entities) where TU : class, IEntity
        {
            AddRange<TU>(Mapper.Map<IEnumerable<TEntity>>(entities));
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public void Complete()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Get entity
        /// </summary>
        /// <param name="keys">Identity keys of entity</param>
        /// <returns>Entity with the given keys</returns>
        public TEntity Get(params object[] keys)
        {
            // Get
            var entity = _context.Set<TEntity>().Find(keys);

            return entity;
        }

        /// <summary>
        /// Get entity
        /// </summary>
        /// <param name="keys">Identity keys of entity</param>
        /// <typeparam name="T">Type of mappable result type</typeparam>
        /// <returns>Mapped entity to the destination type</returns>
        public T Get<T>(params object[] keys)
        {
            return Mapper.Map<T>(Get(keys));
        }

        /// <summary>
        /// Get all entity
        /// </summary>
        /// <returns>All existing entity</returns>
        public List<TEntity> GetAll()
        {
            // Get
            var list = _context.Set<TEntity>().ToList();

            return list;
        }

        /// <summary>
        /// Get all entity.
        /// </summary>
        /// <typeparam name="T">Type of mappable result type</typeparam>
        /// <returns>Mapped entity list to the destination type</returns>
        public List<T> GetAll<T>()
        {
            return Mapper.Map<List<T>>(GetAll());
        }

        /// <summary>
        /// Get list of entities.
        /// </summary>
        /// <param name="predicate">Filter predicate.</param>
        /// <returns>Filtered list of entities</returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetList(predicate, null, null);
        }

        /// <summary>
        /// Get list of entities.
        /// </summary>
        /// <param name="predicate">Filter predicate.</param>
        /// <param name="count">Max result count.</param>
        /// <returns>Filtered list of entities with max count.</returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate, int? count)
        {
            return GetList(predicate, count, null);
        }

        /// <summary>
        /// Get list of entities.
        /// </summary>
        /// <param name="predicate">Filter predicate.</param>
        /// <param name="count">Max result count.</param>
        /// <param name="skip">Skipped element number.</param>
        /// <returns>Filtered list of entities with max count and first skip.</returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate, int? count, int? skip)
        {
            // Get
            var query = _context.Set<TEntity>().Where(predicate);

            // Count
            if (count != null)
            {
                query = query.Take((int) count);
            }

            // Skip
            if (skip != null)
            {
                query = query.Skip((int) skip);
            }

            // To list
            var list = query.ToList();

            return list;
        }

        /// <summary>
        /// Get list of entities.
        /// </summary>
        /// <param name="predicate">Filter predicate.</param>
        /// <typeparam name="T">Type of mappable result type</typeparam>
        /// <returns>Mapped filtered list of entities to destination type</returns>
        public List<T> GetList<T>(Expression<Func<TEntity, bool>> predicate)
        {
            return Mapper.Map<List<T>>(GetList(predicate));
        }

        /// <summary>
        /// Get list of entities.
        /// </summary>
        /// <param name="predicate">Filter predicate.</param>
        /// <param name="count">Max result count.</param>
        /// <typeparam name="T">Type of mappable result type</typeparam>
        /// <returns>Mapped filtered list of entities with max count to destination type</returns>
        public List<T> GetList<T>(Expression<Func<TEntity, bool>> predicate, int? count)
        {
            return Mapper.Map<List<T>>(GetList(predicate, count));
        }

        /// <summary>
        /// Get list of entities.
        /// </summary>
        /// <param name="predicate">Filter predicate.</param>
        /// <param name="count">Max result count.</param>
        /// <param name="skip">Skipped element number.</param>
        /// <typeparam name="T">Type of mappable result type</typeparam>
        /// <returns>Mapped filtered list of entities with max count and first skip to destination type</returns>
        public List<T> GetList<T>(Expression<Func<TEntity, bool>> predicate, int? count, int? skip)
        {
            return Mapper.Map<List<T>>(GetList(predicate, count, skip));
        }

        /// <summary>
        /// Remove entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Remove(TEntity entity)
        {
            // Remove
            _context.Set<TEntity>().Remove(entity);

            // Save
            Complete();
        }

        /// <summary>
        /// Remove by Id
        /// </summary>
        /// <param name="id">Id of entity</param>
        public void Remove(int id)
        {
            // Get entity
            var entity = Get(id);

            if (entity == null)
            {
                throw Logger.LogAnonymousInvalidThings(GetService(), "id", GetEntityErrorMessage());
            }

            // Remove
            Remove(Get(id));
        }

        /// <summary>
        /// Remove range
        /// </summary>
        /// <param name="entities">Entities</param>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            // Determine arguments
            var entityList = entities.ToList();

            // Remove range
            _context.Set<TEntity>().RemoveRange(entityList);

            // Save
            Complete();
        }

        /// <summary>
        /// Remove range by Id
        /// </summary>
        /// <param name="ids">List of Ids</param>
        public void RemoveRange(IEnumerable<int> ids)
        {
            // Remove
            var idList = ids.ToList();
            if (idList.Any())
            {
                RemoveRange(GetList(x => idList.Contains(x.Id)));
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Update(TEntity entity)
        {
            // Set default update data
            var type = entity.GetType();
            var lastUpdate = type.GetProperty("LastUpdate");
            if (lastUpdate != null)
            {
                lastUpdate.SetValue(entity, DateTime.Now, null);
            }

            // Update
            _context.Set<TEntity>().Update(entity);

            // Save
            Complete();
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="id">Id of entity</param>
        /// <param name="entity">Entity model object</param>
        /// <typeparam name="T">Mappable type</typeparam>
        public void Update<T>(int id, T entity)
        {
            // Get original
            var originalEntity = Get(id);

            if (originalEntity == null)
            {
                throw Logger.LogAnonymousInvalidThings(GetService(), "id", GetEntityErrorMessage());
            }

            // Update model to original entity
            Mapper.Map(entity, originalEntity);

            // Update
            Update(originalEntity);
        }

        /// <summary>
        /// Update multiple entity
        /// </summary>
        /// <param name="entities">Entities</param>
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            // Update 
            var entityList = entities.ToList();
            _context.Set<TEntity>().UpdateRange(entityList);

            // Save
            Complete();
        }

        /// <summary>
        /// Update multiple entity
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <typeparam name="T">Mappable type</typeparam>
        public void UpdateRange<T>(Dictionary<int, T> entities)
        {
            // Update
            foreach (var i in entities.Keys)
            {
                Update(i, entities[i]);
            }
        }

        public void Update<TU>(TEntity entity) where TU : class, IEntity
        {
            // Set default update data
            var type = entity.GetType();
            var lastUpdate = type.GetProperty("LastUpdate");
            if (lastUpdate != null)
            {
                lastUpdate.SetValue(entity, DateTime.Now, null);
            }

            var lastUpdater = type.GetProperty("LastUpdaterId");
            if (lastUpdater != null)
            {
                var user = Utils.GetCurrentUser<TU>();
                lastUpdater.SetValue(entity, user.Id, null);
            }

            // Update
            _context.Set<TEntity>().Update(entity);

            // Save
            Complete();
        }

        public void Update<T, TU>(int id, T entity) where TU : class, IEntity
        {
            // Get original
            var originalEntity = Get(id);

            if (originalEntity == null)
            {
                var user = Utils.GetCurrentUser<TU>();
                throw Logger.LogInvalidThings(user.ToString(), GetService(), "id", GetEntityErrorMessage());
            }

            // Update model to original entity
            Mapper.Map(entity, originalEntity);

            // Update
            Update(originalEntity);
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
        private List<string> DetermineArguments(IEnumerable<string> nameList, Type firstType, TEntity entity)
        {
            var args = new List<string>();

            foreach (var i in nameList)
            {
                var propList = i.Split(".");
                var lastType = firstType;
                object lastEntity = entity;

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
                        args.Add((string) lastEntity);
                    }
                    else if (lastType == typeof(DateTime))
                    {
                        args.Add(((DateTime) lastEntity).ToLongDateString());
                    }
                    else if (lastType == typeof(int))
                    {
                        args.Add(((int) lastEntity).ToString());
                    }
                    else if (lastType == typeof(decimal))
                    {
                        args.Add(((decimal) lastEntity).ToString(CultureInfo.CurrentCulture));
                    }
                    else if (lastType == typeof(double))
                    {
                        args.Add(((double) lastEntity).ToString(CultureInfo.CurrentCulture));
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
        public List<TEntity> GetOrderedAll(string orderBy, string direction)
        {
            if (string.IsNullOrEmpty(orderBy)) throw new ArgumentException("Order by value is empty or null");
            var type = typeof(TEntity);
            var property = type.GetProperty(orderBy);

            if (property == null)
            {
                throw new ArgumentException("Property does not exist");
            }

            return direction switch
            {
                "asc" => GetAll().OrderBy(x => property.GetValue(x)).ToList(),
                "desc" => GetAll().OrderByDescending(x => property.GetValue(x)).ToList(),
                "none" => GetAll(),
                _ => throw new ArgumentException("Ordering direction does not exist")
            };

        }

        /// <summary>
        /// Get ordered list
        /// </summary>
        /// <param name="orderBy">Order by</param>
        /// <param name="direction">Order direction</param>
        /// <typeparam name="T">Type of mappable result type</typeparam>
        /// <returns>Mapped and ordered list</returns>
        public List<T> GetOrderedAll<T>(string orderBy, string direction)
        {
            return Mapper.Map<List<T>>(GetOrderedAll(orderBy, direction));
        }
    }
}