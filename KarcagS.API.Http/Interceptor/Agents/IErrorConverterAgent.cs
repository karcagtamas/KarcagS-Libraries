using KarcagS.Shared.Http;

namespace KarcagS.API.Http.Interceptor.Agents;

public interface IErrorConverterAgent
{
    HttpErrorResult? TryToConvert(Exception exception);
}
