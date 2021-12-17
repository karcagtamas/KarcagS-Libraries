using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Karcags.Common.Tools
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        List<TEntity> GetAll();
        List<T> GetAll<T>();
        List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);
        List<T> GetList<T>(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate, int? count);
        List<T> GetList<T>(Expression<Func<TEntity, bool>> predicate, int? count);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate, int? count, int? skip);
        List<T> GetList<T>(Expression<Func<TEntity, bool>> predicate, int? count, int? skip);
        List<TEntity> GetOrderedAll(string orderBy, string direction);
        List<T> GetOrderedAll<T>(string orderBy, string direction);
        TEntity Get(params object[] keys);
        T Get<T>(params object[] keys);
        void Update(TEntity entity);
        void Update<T>(int id, T entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void UpdateRange<T>(Dictionary<int, T> entities);
        void Update<TU>(TEntity entity) where TU : class, IEntity;
        void Update<T, TU>(int id, T entity) where TU : class, IEntity;
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Add<TU>(TEntity entity) where TU : class, IEntity;
        void AddRange<TU>(IEnumerable<TEntity> entities) where TU : class, IEntity;
        void Add<T>(T entity);
        void AddRange<T>(IEnumerable<T> entities);
        void Add<T, TU>(T entity) where TU : class, IEntity;
        void AddRange<T, TU>(IEnumerable<T> entities) where TU : class, IEntity;
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Remove(int id);
        void RemoveRange(IEnumerable<int> ids);
        void Complete();
    }
}