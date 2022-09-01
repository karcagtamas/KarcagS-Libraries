using KarcagS.Shared.Http;

namespace KarcagS.Common.Tools.HttpInterceptor.Agents;

public interface IErrorConverterAgent
{
    HttpErrorResult? TryToConvert(Exception exception);
}
