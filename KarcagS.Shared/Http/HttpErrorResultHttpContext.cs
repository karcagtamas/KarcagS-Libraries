namespace KarcagS.Shared.Http;

public class HttpErrorResultHttpContext
{
    public string Host { get; set; } = default!;
    public string? Path { get; set; }
    public string Method { get; set; } = default!;
    public Dictionary<string, object?> QueryParams { get; set; } = new();
}
