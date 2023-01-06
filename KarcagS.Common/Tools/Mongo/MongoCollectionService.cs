using AutoMapper;
using KarcagS.Common.Tools.Entities;
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

    public void Update(T entity, UpdateDefinition<T> definition) => Collection.UpdateOne(Builders<T>.Filter.Eq(x => x.Id, entity.Id), definition);

    public void UpdateFromModel<M>(string id, M model, UpdateDefinition<T> definition)
    {
        var entity = Get(id);

        if (ObjectHelper.IsNull(entity))
        {
            InsertByModel(model);
        }
        else
        {
            Update(Mapper.Map(model, entity), definition);
        }
    }

    public void DeleteById(string id) => Collection.DeleteOne(Builders<T>.Filter.Eq(x => x.Id, id));
    public void DeleteByIds(params string[] ids) => Collection.DeleteMany(Builders<T>.Filter.In(x => x.Id, ids.ToList()));

}
