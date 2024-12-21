using KarcagS.API.Http.Interceptor.Agents;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Http;

namespace KarcagS.API.Http.Interceptor.Converters;

public class ErrorConverter : IErrorConverter
{
    private readonly List<IErrorConverterAgent> agents;

    public ErrorConverter()
    {
        agents = [];
    }

    public ErrorConverter(List<IErrorConverterAgent> agents)
    {
        this.agents = agents;
    }

    public HttpErrorResult ConvertException(Exception exception, HttpContext httpContext)
    {
        foreach (var agent in agents)
        {
            var result = agent.TryToConvert(exception);

            if (ObjectHelper.IsNotNull(result))
            {
                return AppendHttpContext(result, httpContext);
            }
        }

        return AppendHttpContext(new BasicErrorConverterAgent().TryToConvert(exception) ?? new HttpErrorResult(), httpContext);
    }

    private static HttpErrorResult AppendHttpContext(HttpErrorResult result, HttpContext httpContext)
    {
        result.Context.Add("HTTP", new HttpErrorResultHttpContext
        {
            Host = httpContext.Request.Host.Value ?? "Unknown Host",
            Path = httpContext.Request.Path.Value,
            Method = httpContext.Request.Method,
            QueryParams = httpContext.Request.Query
            .ToDictionary(x => x.Key, x =>
            {
                return x.Value.Count switch
                {
                    0 => null,
                    1 => x.Value[0],
                    _ => (object?)x.Value.ToList()
                };
            })
        });

        return result;
    }
}
