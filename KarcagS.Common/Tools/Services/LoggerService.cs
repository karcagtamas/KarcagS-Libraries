using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KarcagS.Common.Tools.Services;

/// <summary>
/// Logger Service
/// </summary>
public class LoggerService<TUserKey> : ILoggerService
{
    private readonly ILogger<LoggerService<TUserKey>> _logger;
    private readonly IUtilsService<TUserKey> _utilsService;

    /// <summary>
    /// Injector Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="utilsService">Utils Service</param>
    public LoggerService(ILogger<LoggerService<TUserKey>> logger, IUtilsService<TUserKey> utilsService)
    {
        _logger = logger;
        _utilsService = utilsService;
    }

    /// <summary>
    /// Log error to the console
    /// </summary>
    /// <param name="e">Exception for logging</param>
    /// <returns>Error Response from Exception</returns>
    public void LogError(Exception e)
    {
        if (e is not null)
        {
            _logger.LogError(e, "ERROR");
        }
    }

    public void LogError(HttpResultError error, int code)
    {
        if (error is not null)
        {
            _logger.LogError($"Error occurred during the request process[Message={error.Message}, Code={code}]");
        }
    }

    public void LogRequest(HttpContext context)
    {
        if (context is not null)
        {
            var request = context.Request;
            string message = $"[{request.Method}]: {request.Path}";
            _logger.LogInformation(message, Array.Empty<string>());
        }
    }

    public void LogValidationError()
    {
        _logger.LogError($"Validation error occurred during the request process");
    }
}
