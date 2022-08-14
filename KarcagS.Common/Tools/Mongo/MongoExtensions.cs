using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KarcagS.Common.Tools.Mongo;

public static class MongoExtensions
{
    public static WebApplicationBuilder AddMongo<Configuration>(this WebApplicationBuilder builder, Func<ConfigurationManager, IConfigurationSection> configuration) where Configuration : MongoCollectionConfiguration
    {
        builder.Services.Configure<MongoConfiguration<Configuration>>(configuration(builder.Configuration));
        builder.Services.AddSingleton<IMongoService<Configuration>, MongoService<Configuration>>();

        return builder;
    }
}
