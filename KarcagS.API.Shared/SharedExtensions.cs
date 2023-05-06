using KarcagS.API.Shared.Configurations;
using KarcagS.API.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KarcagS.API.Shared;

public static class SharedExtensions
{
    public static WebApplicationBuilder AddLoggerAndUtils<TContext, TUserKey>(this WebApplicationBuilder builder, Func<ConfigurationManager, IConfigurationSection> configuration) where TContext : DbContext
    {
        builder.Services.AddScoped<IUtilsService<TUserKey>, UtilsService<TContext, TUserKey>>();
        builder.Services.AddScoped<ILoggerService, LoggerService<TUserKey>>();
        builder.Services.Configure<UtilsSettings>(configuration(builder.Configuration));

        return builder;
    }
}