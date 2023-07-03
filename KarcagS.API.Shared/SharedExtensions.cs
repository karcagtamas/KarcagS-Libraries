using KarcagS.API.Shared.Configurations;
using KarcagS.API.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KarcagS.API.Shared;

public static class SharedExtensions
{
    public static WebApplicationBuilder AddLoggerAndUtils<TUserKey, TUserProvider>(this WebApplicationBuilder builder, Func<ConfigurationManager, IConfigurationSection> configuration)
        where TUserProvider : class, IUserProvider<TUserKey>
    {
        builder.Services.AddScoped<IUserProvider<TUserKey>, TUserProvider>();
        builder.Services.AddScoped<IUtilsService, UtilsService>();
        builder.Services.AddScoped<ILoggerService, LoggerService<TUserKey>>();
        builder.Services.Configure<UtilsSettings>(configuration(builder.Configuration));

        return builder;
    }
}