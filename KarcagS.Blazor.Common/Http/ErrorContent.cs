namespace KarcagS.Blazor.Common.Http;

public class ErrorContent
{
    public ErrorContentType Type { get; set; }
    public object Content { get; set; } = null!;
}

public enum ErrorContentType
{
    Error,
    ValidationError
}
