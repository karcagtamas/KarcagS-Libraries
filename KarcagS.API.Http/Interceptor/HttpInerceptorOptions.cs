namespace KarcagS.API.Http.Interceptor;

public class HttpInterceptorOptions
{
    public bool OnlyApi { get; set; }
    public string ApiPath { get; set; } = "/api";
    public List<string> IgnoredPaths { get; set; } = new();
}
