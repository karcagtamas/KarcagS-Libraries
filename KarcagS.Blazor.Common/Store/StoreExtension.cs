using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KarcagS.Blazor.Common.Store;

public static class StoreExtension
{
    public static IServiceCollection AddStoreService(this IServiceCollection serviceCollection, Action<StoreService, ILocalStorageService> action)
    {
        serviceCollection.TryAddScoped((Func<IServiceProvider, IStoreService>)(builder =>
       {
           var service = new StoreService(builder.GetRequiredService<ILocalStorageService>());
           service.Init(action);

           return service;
       }));
        return serviceCollection;
    }
}
