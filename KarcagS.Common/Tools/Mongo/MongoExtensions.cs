using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace KarcagS.Common.Tools.Mongo;

public static class MongoExtensions
{
    public static WebApplicationBuilder UseMongo<Configuration>(this WebApplicationBuilder builder) where Configuration : MongoCollectionConfiguration
    {
        builder.Services.AddSingleton<IMongoService<Configuration>, MongoService<Configuration>>();

        return builder;
    }
}
