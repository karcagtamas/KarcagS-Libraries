using KarcagS.API.Mail.Configurations;
using KarcagS.API.Mail.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KarcagS.API.Mail;

public static class MailExtensions
{
    public static WebApplicationBuilder AddMailService(this WebApplicationBuilder builder, Func<ConfigurationManager, IConfigurationSection> configuration)
    {
        builder.Services.AddScoped<IMailService, MailService>();
        builder.Services.Configure<EmailSettings>(configuration(builder.Configuration));

        return builder;
    }
}