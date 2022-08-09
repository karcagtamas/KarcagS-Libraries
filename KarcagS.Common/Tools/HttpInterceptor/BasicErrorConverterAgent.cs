using KarcagS.Shared.Http;

namespace KarcagS.Common.Tools.HttpInterceptor;

public class BasicErrorConverterAgent : IErrorConverterAgent
{
    private const string FatalError = "Something bad happened. Please try again later";

    public HttpResultError? TryToConvert(Exception exception)
    {
        return new(exception)
        {
            Message = FatalError,
            SubMessages = Array.Empty<string>()
        };
    }
}
