namespace KarcagS.Blazor.Common.Http;

public class HttpResponse<T>
{
    public T Content { get; init; }
    public bool IsSuccess { get; init; }
    public bool HasErrorContent { get; set; }
    public ErrorContent ErrorContent { get; set; }
}
