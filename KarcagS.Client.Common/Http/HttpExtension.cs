using KarcagS.Client.Common.Services.Interfaces;
using KarcagS.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KarcagS.Client.Common.Http;

public static class HttpExtension
{
    public static IServiceCollection AddClientHttpService(this IServiceCollection serviceCollection,
        Action<HttpConfiguration> configuration)
    {
        var conf = new HttpConfiguration();
        configuration(conf);

        var refreshService = new HttpRefreshService();
        refreshService.RefreshInProgressSubject.OnNext(HttpRefreshService.RefreshState.FinishState(true));

        serviceCollection.AddSingleton<HttpRefreshService>(_ => refreshService);
        serviceCollection.AddTransient<HttpConfiguration>(_ => conf);
        serviceCollection.TryAddScoped((Func<IServiceProvider, IHttpService>)(builder =>
                new ClientHttpService(
                    builder.GetRequiredService<HttpClient>(),
                    conf,
                    builder.GetRequiredService<ITokenHandler>(),
                    refreshService,
                    builder.GetRequiredService<IHelperService>())
            ));
        return serviceCollection;
    }
}