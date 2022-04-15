using System.Net;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace KarcagS.Common.Tools.HttpInterceptor;

public class HttpInterceptor
{
    private readonly RequestDelegate next;
    private const string FatalError = "Something bad happened. Please try again later";

    public HttpInterceptor(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILoggerService logger)
    {
        logger.LogRequest(context);

        if (IsSwagger(context))
        {
            await next.Invoke(context);
        }
        else
        {
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await next.Invoke(context);

                if (context.Request.Method == HttpMethod.Options.Method)
                {
                    return;
                }

                if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    var body = JsonConvert.DeserializeObject(await FormatResponse(context.Response));
                    await HandleSuccessRequestAsync(context, body, context.Response.StatusCode);
                }
                else if (context.Response.StatusCode == ValidationErrorActionResult.ValidationErrorCode)
                {
                    return;
                }
                else
                {
                    await HandleNotSuccessRequestAsync(context, context.Response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
            finally
            {
                if (context.Response.StatusCode != (int)HttpStatusCode.NoContent)
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }
    }

    private static async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return plainBodyText;
    }

    private static bool IsSwagger(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/swagger");
    }

    private static Task HandleSuccessRequestAsync(HttpContext context, object body, int code)
    {
        context.Response.ContentType = "application/json";

        var response = new HttpResult<object>
        {
            IsSuccess = true,
            StatusCode = code,
            Result = body
        };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    private static Task HandleNotSuccessRequestAsync(HttpContext context, int code)
    {
        context.Response.ContentType = "application/json";

        var response = new HttpResult<object>
        {
            IsSuccess = false,
            StatusCode = code
        };


        if (code == (int)HttpStatusCode.NotFound)
        {
            response.Error = new HttpResultError
            {
                Message = "Resource not found.",
                SubMessages = Array.Empty<string>()
            };
        }
        else
        {
            response.Error = new HttpResultError
            {
                Message = "Request cannot be processed.",
                SubMessages = Array.Empty<string>()
            };
        }
        context.Response.StatusCode = code;

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILoggerService logger)
    {
        context.Response.ContentType = "application/json";
        const int statusCode = (int)HttpStatusCode.InternalServerError;

        var response = new HttpResult<object>
        {
            IsSuccess = false,
            StatusCode = statusCode,
        };

        if (exception is ServerException)
        {
            response.Error = new HttpResultError
            {
                Message = exception.Message,
                SubMessages = Array.Empty<string>()
            };
        }
        else
        {
            response.Error = new HttpResultError
            {
                Message = FatalError,
                SubMessages = Array.Empty<string>()
            };
        }

        logger.LogError(exception);

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
