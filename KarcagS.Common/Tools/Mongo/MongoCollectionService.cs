using AutoMapper;
using KarcagS.Common.Tools.Entities;
using KarcagS.Shared.Helpers;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace KarcagS.Common.Tools.Mongo;

public class MongoCollectionService<T, Configuration> : IMongoCollectionService<T> where T : IMongoEntity where Configuration : MongoCollectionConfiguration
{
    protected readonly IMongoService<Configuration> MongoService;
    protected readonly IMapper Mapper;
    protected readonly IMongoCollection<T> Collection;

    public MongoCollectionService(IMongoService<Configuration> mongoService, IMapper mapper, Func<Configuration, string> collectionNameGetter)
    {
        MongoService = mongoService;
        Mapper = mapper;
        Collection = mongoService.GetDatabase().GetCollection<T>(collectionNameGetter(mongoService.GetConfiguration().CollectionNames));
    }

    public List<T> Get() => Collection.Find(x => true).ToList();

    public T? Get(string id) => Collection.Find(x => x.Id == id).FirstOrDefault();

    public T? Get(Expression<Func<T, bool>> where) => Collection.Find(where).FirstOrDefault();

    public List<T> GetList(Expression<Func<T, bool>> where) => Collection.Find(where).ToList();

    public string Insert(T entity)
    {
        Collection.InsertOne(entity);

        return entity.Id;
    }

    public string InsertByModel<M>(M model) => Insert(Mapper.Map<T>(model));

    public void Update(T entity, Dictionary<Expression<Func<T, object>>, object> updateSets)
    {
        if (updateSets.Count > 0)
        {
            return;
        }

        var updateBuilder = Builders<T>.Update;
        UpdateDefinition<T>? def = null;

        foreach (var i in updateSets)
        {
            def = updateBuilder.Set(i.Key, i.Value);
        }

        Collection.UpdateOne(Builders<T>.Filter.Eq(x => x.Id, entity.Id), ObjectHelper.OrElseThrow(def, () => new Exception("Invalid set count")));
    }

    public void UpdateFromModel<M>(string id, M model, Dictionary<Expression<Func<T, object>>, object> updateSets)
    {
        var entity = Get(id);

        if (ObjectHelper.IsNull(entity))
        {
            InsertByModel(model);
        }
        else
        {
            Update(Mapper.Map(model, entity), updateSets);
        }
    }

    public void DeleteById(string id) => Collection.DeleteOne(Builders<T>.Filter.Eq(x => x.Id, id));
    public void DeleteByIds(params string[] ids) => Collection.DeleteMany(Builders<T>.Filter.In(x => x.Id, ids.ToList()));

}
