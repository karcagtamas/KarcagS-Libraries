using KarcagS.Common.Tools.Entities;
using System.Linq.Expressions;

namespace KarcagS.Common.Tools.Mongo;

public interface IMongoCollectionService<T> where T : IMongoEntity
{
    List<T> Get();
    T? Get(string id);
    T? Get(Expression<Func<T, bool>> where);
    List<T> GetList(Expression<Func<T, bool>> where);
    string Insert(T entity);
    string InsertByModel<M>(M model);
}
