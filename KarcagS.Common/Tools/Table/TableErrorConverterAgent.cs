using KarcagS.Common.Tools.HttpInterceptor.Agents;
using KarcagS.Shared.Http;

namespace KarcagS.Common.Tools.Table;

public class TableErrorConverterAgent : IErrorConverterAgent
{
    public HttpErrorResult? TryToConvert(Exception exception)
    {
        if (exception is TableException t)
        {
            return new(exception)
            {
                Message = new ResourceMessage { Text = t.Message, ResourceKey = t.ResourceKey },
                SubMessages = Array.Empty<ResourceMessage>()
            };
        }

        return default;
    }
}
