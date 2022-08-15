namespace KarcagS.Shared.Http;

public class HttpResult<T>
{
    public int StatusCode { get; set; }
    public T? Result { get; set; }
    public bool IsSuccess { get; set; }
    public HttpErrorResult? Error { get; set; }
}
