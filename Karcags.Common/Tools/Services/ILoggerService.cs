using Karcags.Common.Tools.ErrorHandling;
using Microsoft.AspNetCore.Http;

namespace Karcags.Common.Tools.Services;

public interface ILoggerService
{
    void LogError(Exception e);
    void LogRequest(HttpContext context);
}
