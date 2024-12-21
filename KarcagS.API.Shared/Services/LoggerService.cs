using KarcagS.Shared.Helpers;
using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KarcagS.API.Shared.Services;

/// <summary>
/// Logger Service
/// </summary>
public class LoggerService<TUserKey> : ILoggerService
{
    private readonly ILogger<LoggerService<TUserKey>> logger;

    /// <summary>
    /// Injector Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    public LoggerService(ILogger<LoggerService<TUserKey>> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Log error to the console
    /// </summary>
    /// <param name="e">Exception for logging</param>
    /// <returns>Error Response from Exception</returns>
    public void LogError(Exception? e)
    {
        if (ObjectHelper.IsNotNull(e))
        {
            logger.LogError(e, "ERROR");
        }
    }

    public void LogError(HttpErrorResult? error, int code)
    {
        if (ObjectHelper.IsNotNull(error))
        {
            logger.LogError("Error occurred during the request process[Message={@ErrorMessage}, Code={Code}]", error.Message, code);
        }
    }

    public void LogRequest(HttpContext? context)
    {
        if (ObjectHelper.IsNotNull(context))
        {
            var request = context.Request;
            logger.LogInformation("[{RequestMethod}]: {RequestPath}", request.Method, request.Path);
        }
    }

    public void LogValidationError() => logger.LogError("Validation error occurred during the request process");
}