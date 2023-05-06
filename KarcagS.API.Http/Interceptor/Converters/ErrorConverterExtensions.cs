using KarcagS.API.Http.Interceptor.Agents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KarcagS.API.Http.Interceptor.Converters;

public static class ErrorConverterExtensions
{
    public static IServiceCollection AddErrorConverter(this IServiceCollection serviceCollection, Action<ErrorConverterConfiguration> configurator)
    {
        var config = new ErrorConverterConfiguration();

        config.AddAgent(new ServerErrorConverterAgent());

        configurator(config);

        config.AddAgent(new BasicErrorConverterAgent());

        serviceCollection.TryAddScoped((Func<IServiceProvider, IErrorConverter>)(builder => new ErrorConverter(config.Agents)));
        return serviceCollection;
    }
}