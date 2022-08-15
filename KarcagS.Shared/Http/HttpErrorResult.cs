namespace KarcagS.Shared.Http;

/// <summary>
/// Error response model
/// </summary>
public class HttpErrorResult
{
    public HttpErrorResult()
    {

    }

    public HttpErrorResult(Exception exception)
    {
        StackTrace = exception.StackTrace;
    }

    public string Message { get; set; } = string.Empty;
    public string[] SubMessages { get; set; } = Array.Empty<string>();
    public string? StackTrace { get; set; }

    public Dictionary<string, object> Context { get; set; } = new();
}
