using KarcagS.Shared.Http;

namespace KarcagS.Blazor.Common.Http;

public class ResultWrapper<T>
{
    public T? Result { get; set; }
    public HttpErrorResult? Error { get; set; }
}
