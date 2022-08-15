using KarcagS.Shared.Http;

namespace KarcagS.Common.Tools.HttpInterceptor;

public interface IErrorConverterAgent
{
    HttpErrorResult? TryToConvert(Exception exception);
}
