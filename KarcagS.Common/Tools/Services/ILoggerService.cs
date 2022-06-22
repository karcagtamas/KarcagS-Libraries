using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Http;

namespace KarcagS.Common.Tools.Services;

public interface ILoggerService
{
    void LogError(Exception e);
    void LogRequest(HttpContext context);
    void LogValidationError();
    void LogError(HttpResultError error, int code);
}
