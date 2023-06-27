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

    public static IServiceCollection UseMongoPersistence<Configuration, CollectionProvider>(this IServiceCollection services) where Configuration : MongoCollectionConfiguration where CollectionProvider : class, IMongoCollectionProvider<Configuration>
    {
        services.AddScoped<IMongoCollectionProvider<Configuration>, CollectionProvider>();
        services.AddTransient<MongoPersistence<Configuration>>();

        return services;
    }
}