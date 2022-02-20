using Microsoft.AspNetCore.Builder;

namespace KarcagS.Common.Middlewares
{
    public static class HttpInterceptorExtension
    {
        public static IApplicationBuilder UseHttpInterceptor(this WebApplication app)
        {
            return app.UseMiddleware<HttpInterceptor>();
        }
    }
}