using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace KarcagS.Common.Tools.HttpInterceptor;

public static class HttpInterceptorExtensions
{
    public static IApplicationBuilder UseHttpInterceptor(this WebApplication app, Action<HttpInterceptorOptions> configureOptions)
    {
        var options = new HttpInterceptorOptions();
        configureOptions(options);

        return app.UseMiddleware<HttpInterceptor>(options);
    }

    public static IServiceCollection AddModelValidatedControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(conf =>
            {
                conf.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            })
            .ConfigureApiBehaviorOptions(
            opt =>
            {
                opt.InvalidModelStateResponseFactory = context =>
                {
                    return new ValidationErrorActionResult(new HttpResult<object>
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Error = new HttpErrorResult
                        {
                            Message = new ResourceMessage { Text = "Validation error", ResourceKey = "Server.Message.Validation" },
                            SubMessages = context.ModelState
                                .SelectMany(x =>
                                {
                                    if (x.Value is null)
                                    {
                                        return new List<ResourceMessage>();
                                    }

                                    return x.Value.Errors.Select(e =>
                                    {
                                        return new ResourceMessage { Text = $"{x.Key}: {e.ErrorMessage}" };
                                    }).ToList();
                                })
                                .ToArray()
                        }
                    });
                };
            });

        return services;
    }
}
