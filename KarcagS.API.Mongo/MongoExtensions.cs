using KarcagS.API.Mongo.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KarcagS.API.Mongo;

public static class MongoExtensions
{
    public static WebApplicationBuilder AddMongo<Configuration>(this WebApplicationBuilder builder, Func<ConfigurationManager, IConfigurationSection> configuration) where Configuration : MongoCollectionConfiguration
    {
        builder.Services.Configure<MongoConfiguration<Configuration>>(configuration(builder.Configuration));
        builder.Services.AddSingleton<IMongoService<Configuration>, MongoService<Configuration>>();

        return builder;
    }
}