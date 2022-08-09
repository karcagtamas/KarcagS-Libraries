using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

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
            .ConfigureApiBehaviorOptions(
            opt =>
            {
                opt.InvalidModelStateResponseFactory = context =>
                {
                    return new ValidationErrorActionResult(new HttpResult<object>
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Error = new HttpResultError
                        {
                            Message = "Validation error",
                            SubMessages = context.ModelState
                                .SelectMany(x =>
                                {
                                    if (x.Value is null)
                                    {
                                        return new List<string>();
                                    }

                                    return x.Value.Errors.Select(e =>
                                    {
                                        return $"{x.Key}: {e.ErrorMessage}";
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
