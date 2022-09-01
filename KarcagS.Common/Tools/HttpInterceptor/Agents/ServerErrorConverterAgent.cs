using KarcagS.Shared.Http;

namespace KarcagS.Common.Tools.HttpInterceptor.Agents;

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
