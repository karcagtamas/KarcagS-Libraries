using KarcagS.API.Auth.JWT;
using KarcagS.API.Auth.JWT.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KarcagS.API.Auth;

public static class AuthExtensions
{
    public static WebApplicationBuilder AddJWTService(this WebApplicationBuilder builder, Func<ConfigurationManager, IConfigurationSection> configuration)
    {
        builder.Services.AddScoped<IJWTAuthService, JWTAuthService>();
        builder.Services.Configure<JWTConfiguration>(configuration(builder.Configuration));

        return builder;
    }
}