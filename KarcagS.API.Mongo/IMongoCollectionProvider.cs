using KarcagS.API.Mongo.Configurations;
using KarcagS.API.Data.Entities;

namespace KarcagS.API.Mongo;

public interface IMongoCollectionProvider<Configuration> where Configuration : MongoCollectionConfiguration
{
    string GetCollectionName<TKey, T>(Configuration configuration) where T : Entity<TKey>;
}