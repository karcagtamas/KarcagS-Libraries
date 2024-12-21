using KarcagS.API.Http.Interceptor.Agents;
using KarcagS.Shared.Http;

namespace KarcagS.API.Table;

public class TableErrorConverterAgent : IErrorConverterAgent
{
    public HttpErrorResult? TryToConvert(Exception exception)
    {
        if (exception is TableException t)
        {
            return new(exception)
            {
                Message = new ResourceMessage { Text = t.Message, ResourceKey = t.ResourceKey },
                SubMessages = []
            };
        }

        return null;
    }
}
