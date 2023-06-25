using KarcagS.Shared.Http;

namespace KarcagS.Http;

public class ResultWrapper<T>
{
    public T? Result { get; set; }
    public HttpErrorResult? Error { get; set; }
}
