using System;
using System.Net;
using System.Threading.Tasks;
using Karcags.Common.Tools.ErrorHandling;
using Karcags.Common.Tools.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;

namespace Karcags.Common.Middlewares;

public class ExceptionHandler
{
    private readonly RequestDelegate next;
    private ILoggerService logger;
    private const string FatalError = "Something bad happened. Try again later";

    public ExceptionHandler(RequestDelegate next)
    {
        this.next = next;
        logger = default!;
    }

    public async Task InvokeAsync(HttpContext context, ILoggerService logger)
    {
        this.logger = logger;
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

                if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    var body = await FormatResponse(context.Response);
                    await HandleSuccessRequestAsync(context, body, context.Response.StatusCode);
                }
                else
                {
                    await HandleNotSuccessRequestAsync(context, context.Response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        try
        {
            await next.Invoke(context);
        }
        catch (ServerException me)
        {
            await HandleExceptionAsync(context, me).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e).ConfigureAwait(false);
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

        var response = new HttpResult
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

        var response = new HttpResult
        {
            IsSuccess = false,
            StatusCode = code
        };


        if (code == (int)HttpStatusCode.NotFound)
        {
            response.Error = new ErrorResult
            {
                Message = "Resource not found.",
                SubMessages = new string[0]
            };
        }
        else
        {
            response.Error = new ErrorResult
            {
                Message = "Request cannot be processed.",
                SubMessages = new string[0]
            };
        }
        context.Response.StatusCode = code;

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        const int statusCode = (int)HttpStatusCode.InternalServerError;

        var response = new HttpResult
        {
            IsSuccess = false,
            StatusCode = statusCode,
        };

        if (exception is ServerException)
        {
            response.Error = new ErrorResult
            {
                Message = exception.Message,
                SubMessages = new string[0]
            };
        }
        else
        {
            response.Error = new ErrorResult
            {
                Message = FatalError,
                SubMessages = new string[0]
            };
        }

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
