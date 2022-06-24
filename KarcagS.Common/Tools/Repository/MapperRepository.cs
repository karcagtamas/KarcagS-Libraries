using System.Linq.Expressions;
using AutoMapper;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.Entities;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace KarcagS.Common.Tools.Repository;

public abstract class MapperRepository<TEntity, TKey, TUserKey> : Repository<TEntity, TKey, TUserKey>, IMapperRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    protected readonly IMapper Mapper;

    public MapperRepository(DbContext context, ILoggerService loggerService, IUtilsService<TUserKey> utilsService, IMapper mapper, string entity) : base(context, loggerService, utilsService, entity)
    {
        Mapper = mapper;
    }

    public virtual TKey CreateFromModel<TModel>(TModel model, bool doPersist = true) => Create(Mapper.Map<TEntity>(model), doPersist);

    public virtual IEnumerable<T> GetAllMapped<T>() => Mapper.Map<IEnumerable<T>>(GetAll());

    public virtual T GetMapped<T>(TKey id) => Mapper.Map<T>(Get(id));

    public virtual T? GetOptionalMapped<T>(TKey id) => ObjectHelper.MapOrDefault(GetOptional(id), (obj) => Mapper.Map<T>(obj));

    public virtual IEnumerable<T> GetMappedList<T>(Expression<Func<TEntity, bool>> expression, int? count = null, int? skip = null) => Mapper.Map<IEnumerable<T>>(GetList(expression, count, skip));

    public virtual IEnumerable<T> GetAllMappedAsOrdered<T>(string orderBy, string direction) => Mapper.Map<IEnumerable<T>>(GetAllAsOrdered(orderBy, direction));

    public virtual void UpdateByModel<TModel>(TKey id, TModel model, bool doPersist = true)
    {
        ExceptionHelper.ThrowIfIsNull<TModel, ArgumentException>(model, "Model cannot be null");

        Update(Mapper.Map(model, Get(id)), doPersist);
    }

    public virtual IEnumerable<T> GetMappedOrderedList<T>(Expression<Func<TEntity, bool>> expression, string orderBy, string direction, int? count = null, int? skip = null) => Mapper.Map<IEnumerable<T>>(GetOrderedList(expression, orderBy, direction, count, skip));

}