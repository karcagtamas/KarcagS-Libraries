using KarcagS.API.Shared.Exceptions;
using KarcagS.Shared.Http;

namespace KarcagS.API.Http.Interceptor.Agents;

public class ServerErrorConverterAgent : IErrorConverterAgent
{
    public HttpErrorResult? TryToConvert(Exception exception)
    {
        if (exception is ServerException s)
        {
            return new(s)
            {
                Message = new ResourceMessage { Text = s.Message, ResourceKey = s.ResourceKey },
                SubMessages = Array.Empty<ResourceMessage>()
            };
        }

        return default;
    }
}