using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Http;

namespace KarcagS.Common.Tools.HttpInterceptor;

public interface IErrorConverter
{
    HttpErrorResult ConvertException(Exception exception, HttpContext httpContext);
}
