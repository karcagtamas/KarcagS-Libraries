using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace KarcagS.Common.Tools.Mongo;

public class MongoService<Configuration> : IMongoService<Configuration> where Configuration : MongoCollectionConfiguration
{
    private readonly MongoConfiguration<Configuration> configuration;
    private readonly MongoClient client;
    private readonly IMongoDatabase database;

    public MongoService(IOptions<MongoConfiguration<Configuration>> configurationOptions)
    {
        configuration = configurationOptions.Value;
        client = new MongoClient(configuration.ConnectionString);
        database = client.GetDatabase(configuration.DatabaseName);
    }

    public MongoClient GetClient()
    {
        return client;
    }

    public MongoConfiguration<Configuration> GetConfiguration()
    {
        return configuration;
    }

    public IMongoDatabase GetDatabase()
    {
        return database;
    }
}
