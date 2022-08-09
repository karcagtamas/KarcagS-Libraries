using KarcagS.Shared.Http;

namespace KarcagS.Common.Tools.HttpInterceptor;

public interface IErrorConverterAgent
{
    HttpResultError? TryToConvert(Exception exception);
}
