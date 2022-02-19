using Blazored.LocalStorage;
using KarcagS.Blazor.Common.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.JSInterop;

namespace KarcagS.Blazor.Common.Http;

public static class HttpExtension
{
    public static IServiceCollection AddHttpService<TError, TValidationError>(this IServiceCollection serviceCollection,
        Action<HttpConfiguration> configuration)
    {
        var conf = new HttpConfiguration();
        configuration(conf);
        serviceCollection.TryAddScoped((Func<IServiceProvider, IHttpService>)(builder =>
           new HttpService<TError, TValidationError>(builder.GetRequiredService<HttpClient>(), builder.GetRequiredService<IHelperService>(),
               builder.GetRequiredService<IJSRuntime>(), conf, builder.GetRequiredService<ILocalStorageService>(),
               builder.GetRequiredService<NavigationManager>())));
        return serviceCollection;
    }
}
