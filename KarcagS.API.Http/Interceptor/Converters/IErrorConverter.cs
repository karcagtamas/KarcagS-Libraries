using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Http;

namespace KarcagS.API.Http.Interceptor.Converters;

public interface IErrorConverter
{
    HttpErrorResult ConvertException(Exception exception, HttpContext httpContext);
}
