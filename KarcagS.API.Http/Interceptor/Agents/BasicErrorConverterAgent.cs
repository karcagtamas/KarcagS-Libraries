using KarcagS.Shared.Http;

namespace KarcagS.API.Http.Interceptor.Agents;

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