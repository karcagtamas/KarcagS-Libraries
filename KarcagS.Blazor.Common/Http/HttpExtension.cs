using KarcagS.Client.Common.Services.Interfaces;
using KarcagS.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.JSInterop;

namespace KarcagS.Blazor.Common.Http;

public static class HttpExtension
{
    public static IServiceCollection AddBlazorHttpService(this IServiceCollection serviceCollection,
        Action<HttpConfiguration> configuration)
    {
        var conf = new HttpConfiguration();
        configuration(conf);

        var refreshService = new HttpRefreshService();
        serviceCollection.AddSingleton<HttpRefreshService>(_ => refreshService);
        serviceCollection.AddTransient<HttpConfiguration>(_ => conf);

        serviceCollection.TryAddScoped<ITokenHandler, LocalStorageTokenHandler>();
        serviceCollection.TryAddScoped((Func<IServiceProvider, IHttpService>)(builder =>
                new BlazorHttpService(
                    builder.GetRequiredService<HttpClient>(),
                    conf,
                    builder.GetRequiredService<ITokenHandler>(),
                    refreshService,
                    builder.GetRequiredService<IHelperService>(),
                    builder.GetRequiredService<IJSRuntime>(),
                    builder.GetRequiredService<NavigationManager>()))
        );
        return serviceCollection;
    }
}