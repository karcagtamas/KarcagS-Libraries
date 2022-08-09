namespace KarcagS.Shared.Http;

/// <summary>
/// Error response model
/// </summary>
public class HttpResultError
{
    public HttpResultError()
    {

    }

    public HttpResultError(Exception exception)
    {
        StackTrace = exception.StackTrace;
    }

    public string Message { get; set; } = string.Empty;
    public string[] SubMessages { get; set; } = Array.Empty<string>();
    public string? StackTrace { get; set; }
}
