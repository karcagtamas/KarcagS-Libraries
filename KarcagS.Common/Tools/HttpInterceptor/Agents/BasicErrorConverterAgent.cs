using KarcagS.Shared.Http;

namespace KarcagS.Common.Tools.HttpInterceptor.Agents;

public class BasicErrorConverterAgent : IErrorConverterAgent
{
    private const string FatalError = "Something bad happened. Please try again later";

    public HttpErrorResult? TryToConvert(Exception exception)
    {
        return new(exception)
        {
            Message = new ResourceMessage { Text = FatalError, ResourceKey = "Server.Message.Fatal" },
            SubMessages = Array.Empty<ResourceMessage>()
        };
    }
}
