using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Shared.Http;

namespace KarcagS.Common.Tools.Table;

public class TableErrorConverterAgent : IErrorConverterAgent
{
    public HttpErrorResult? TryToConvert(Exception exception)
    {
        if (exception is TableException)
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
