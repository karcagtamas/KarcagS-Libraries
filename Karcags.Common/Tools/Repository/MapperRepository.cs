using System.Linq.Expressions;
using AutoMapper;
using KarcagS.Common.Tools.Entities;
using KarcagS.Common.Tools.Services;
using Microsoft.EntityFrameworkCore;

namespace KarcagS.Common.Tools.Repository;

public abstract class MapperRepository<TEntity, TKey> : Repository<TEntity, TKey>, IMapperRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    protected readonly IMapper Mapper;

    public MapperRepository(DbContext context, ILoggerService loggerService, IUtilsService utilsService, IMapper mapper, string entity) : base(context, loggerService, utilsService, entity)
    {
        Mapper = mapper;
    }

    public virtual void CreateFromModel<TModel>(TModel model)
    {
        Create(Mapper.Map<TEntity>(model));
    }

    public virtual IEnumerable<T> GetAllMapped<T>()
    {
        return Mapper.Map<IEnumerable<T>>(GetAll());
    }

    public virtual T GetMapped<T>(TKey id)
    {
        return Mapper.Map<T>(Get(id));
    }

    public virtual IEnumerable<T> GetMappedList<T>(Expression<Func<TEntity, bool>> expression)
    {
        return Mapper.Map<IEnumerable<T>>(GetList(expression));
    }

    public virtual IEnumerable<T> GetMappedList<T>(Expression<Func<TEntity, bool>> expression, int? count)
    {
        return Mapper.Map<IEnumerable<T>>(GetList(expression, count));
    }

    public virtual IEnumerable<T> GetMappedList<T>(Expression<Func<TEntity, bool>> expression, int? count, int? skip)
    {
        return Mapper.Map<IEnumerable<T>>(GetList(expression, count, skip));
    }

    public virtual IEnumerable<T> GetAllMappedAsOrdered<T>(string orderBy, string direction)
    {
        return Mapper.Map<IEnumerable<T>>(GetAllAsOrdered(orderBy, direction));
    }

    public virtual void UpdateByModel<TModel>(TKey id, TModel model)
    {
        Update(Mapper.Map(model, Get(id)));
    }

    public IEnumerable<T> GetMappedOrderedList<T>(Expression<Func<TEntity, bool>> expression, string orderBy, string direction)
    {
        return Mapper.Map<IEnumerable<T>>(GetOrderedList(expression, orderBy, direction));
    }

    public IEnumerable<T> GetMappedOrderedList<T>(Expression<Func<TEntity, bool>> expression, int? count, string orderBy, string direction)
    {
        return Mapper.Map<IEnumerable<T>>(GetOrderedList(expression, count, orderBy, direction));
    }

    public IEnumerable<T> GetMappedOrderedList<T>(Expression<Func<TEntity, bool>> expression, int? count, int? skip, string orderBy, string direction)
    {
        return Mapper.Map<IEnumerable<T>>(GetOrderedList(expression, count, skip, orderBy, direction));
    }
}