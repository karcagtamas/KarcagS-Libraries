using KarcagS.Shared.Http;

namespace KarcagS.Common.Tools.HttpInterceptor;

public class ServerErrorConverterAgent : IErrorConverterAgent
{
    public HttpErrorResult? TryToConvert(Exception exception)
    {
        if (exception is ServerException)
        {
            return new(exception)
            {
                Message = exception.Message,
                SubMessages = Array.Empty<string>()
            };
        }

        return default;
    }
}
