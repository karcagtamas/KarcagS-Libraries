using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KarcagS.Http;

public static class HttpExtension
{
    public static IServiceCollection AddHttpService(this IServiceCollection serviceCollection,
        Action<HttpConfiguration> configuration)
    {
        var conf = new HttpConfiguration();
        configuration(conf);
        serviceCollection.AddTransient<HttpConfiguration>(_ => conf);
        serviceCollection.TryAddScoped((Func<IServiceProvider, IHttpService>)(builder =>
                new HttpService(
                    builder.GetRequiredService<HttpClient>(),
                    conf,
                    builder.GetRequiredService<ITokenHandler>())
            ));
        return serviceCollection;
    }
}