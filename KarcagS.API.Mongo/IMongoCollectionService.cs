using System.Linq.Expressions;
using MongoDB.Driver;

namespace KarcagS.API.Mongo;

public interface IMongoCollectionService<T> where T : MongoEntity
{
    List<T> Get();
    T? Get(string id);
    T? Get(Expression<Func<T, bool>> where);
    List<T> GetList(Expression<Func<T, bool>> where);
    string Insert(T entity);
    string InsertByModel<M>(M model);
    void Update(T entity, UpdateDefinition<T> definition);
    void UpdateFromModel<M>(string id, M model, UpdateDefinition<T> definition);
    void DeleteById(string id);
    void DeleteByIds(params string[] ids);
}