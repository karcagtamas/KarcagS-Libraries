namespace KarcagS.Blazor.Common.Http;

public class HttpCall<TKey> : IHttpCall<TKey>
{
    protected readonly IHttpService Http;
    protected readonly string Url;
    private readonly string _caption;

    protected HttpCall(IHttpService http, string url, string caption)
    {
        Http = http;
        Url = url;
        _caption = caption;
    }

    public Task<List<T>> GetAll<T>() => GetAll<T>("Id");
    

    public async Task<List<T>> GetAll<T>(string orderBy, string direction = "asc")
    {
        var queryParams = HttpQueryParameters.Build()
            .AddOptional("orderBy", orderBy, (x) => !string.IsNullOrEmpty(x))
            .AddOptional("direction", direction, (x) => !string.IsNullOrEmpty(x));

        var settings = new HttpSettings(Url).AddQueryParams(queryParams);

        return await Http.Get<List<T>>(settings).ExecuteWithResultOrElse(new());
    }

    public async Task<T?> Get<T>(TKey id)
    {
        var pathParams = HttpPathParameters.Build().Add(id);

        var settings = new HttpSettings(Url).AddPathParams(pathParams);

        return await Http.Get<T>(settings).ExecuteWithResult();
    }

    public async Task<bool> Create<T>(T model)
    {
        var settings = new HttpSettings(Url).AddToaster($"{_caption} adding");

        var body = new HttpBody<T>(model);

        return await Http.Post(settings, body).Execute();
    }

    public async Task<bool> Update<T>(TKey id, T model)
    {
        var pathParams = HttpPathParameters.Build().Add(id);

        var settings = new HttpSettings(Url).AddPathParams(pathParams).AddToaster($"{_caption} updating");

        var body = new HttpBody<T>(model);

        return await Http.Put(settings, body).Execute();
    }

    public async Task<bool> Delete(TKey id)
    {
        var pathParams = HttpPathParameters.Build().Add(id);

        var settings = new HttpSettings(Url).AddPathParams(pathParams).AddToaster($"{_caption} deleting");

        return await Http.Delete(settings).Execute();
    }
}
