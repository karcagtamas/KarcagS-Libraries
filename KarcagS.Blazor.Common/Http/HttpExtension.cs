using KarcagS.Client.Common.Http;
using KarcagS.Client.Common.Services.Interfaces;
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
        serviceCollection.AddTransient<HttpConfiguration>(_ => conf);

        serviceCollection.TryAddScoped<ITokenHandler, LocalStorageTokenHandler>();
        serviceCollection.TryAddScoped((Func<IServiceProvider, IHttpService>)(builder =>
                new BlazorHttpService(
                    builder.GetRequiredService<HttpClient>(),
                    builder.GetRequiredService<IHelperService>(),
                    conf,
                    builder.GetRequiredService<ITokenHandler>(),
                    builder.GetRequiredService<IJSRuntime>(),
                    builder.GetRequiredService<NavigationManager>()))
        );
        return serviceCollection;
    }
}