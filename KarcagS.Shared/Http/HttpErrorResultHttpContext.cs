namespace KarcagS.Shared.Http;

public class HttpErrorResultHttpContext
{
    public string Host { get; set; } = string.Empty;
    public string? Path { get; set; }
    public string Method { get; set; } = string.Empty;
    public Dictionary<string, object?> QueryParams { get; set; } = new();
}