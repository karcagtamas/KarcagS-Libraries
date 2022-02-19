namespace Karcags.Common.Tools.ErrorHandling;

public class HttpResult<T>
{
    public int StatusCode { get; set; }
    public T? Result { get; set; }
    public bool IsSuccess { get; set; }
    public HttpResultError? Error { get; set; }
}
