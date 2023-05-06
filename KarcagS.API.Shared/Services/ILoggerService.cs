using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Http;

namespace KarcagS.API.Shared.Services;

public interface ILoggerService
{
    void LogError(Exception? e);
    void LogRequest(HttpContext? context);
    void LogValidationError();
    void LogError(HttpErrorResult? error, int code);
}