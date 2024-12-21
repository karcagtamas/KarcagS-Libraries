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

    public ResourceMessage Message { get; set; } = new();
    public ResourceMessage[] SubMessages { get; set; } = [];
    public string? StackTrace { get; set; }

    public Dictionary<string, object> Context { get; set; } = new();
}

public class ResourceMessage
{
    public string Text { get; set; } = string.Empty;
    public string? ResourceKey { get; set; }
}
