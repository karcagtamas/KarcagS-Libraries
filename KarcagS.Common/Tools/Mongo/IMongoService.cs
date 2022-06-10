using MongoDB.Driver;

namespace KarcagS.Common.Tools.Mongo;

public interface IMongoService<Configuration> where Configuration : MongoCollectionConfiguration
{
    MongoClient GetClient();
    IMongoDatabase GetDatabase();
    MongoConfiguration<Configuration> GetConfiguration();
}
