using KarcagS.API.Mongo.Configurations;
using MongoDB.Driver;

namespace KarcagS.API.Mongo;

public interface IMongoService<Configuration> where Configuration : MongoCollectionConfiguration
{
    MongoClient GetClient();
    IMongoDatabase GetDatabase();
    MongoConfiguration<Configuration> GetConfiguration();
}
