using Microsoft.AspNetCore.Builder;

namespace Karcags.Common.Middlewares
{
    public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseMyExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandler>();
        }
    }
}