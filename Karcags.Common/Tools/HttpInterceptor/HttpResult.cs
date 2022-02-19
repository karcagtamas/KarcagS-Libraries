namespace Karcags.Common.Tools.ErrorHandling;

public class HttpResult
{
    public int StatusCode { get; set; }
    public object? Result { get; set; }
    public bool IsSuccess { get; set; }
    public HttpResultError? Error { get; set; }
}
