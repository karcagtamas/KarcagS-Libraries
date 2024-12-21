using KarcagS.Shared.Helpers;
using Microsoft.Extensions.Localization;

namespace KarcagS.Http;

public class HttpCall<TKey> : IHttpCall<TKey>
{
    protected readonly IHttpService Http;
    protected readonly string Url;
    protected readonly IStringLocalizer? Localizer;
    private readonly string caption;

    protected HttpCall(IHttpService http, string url, string caption, IStringLocalizer? localizer)
    {
        Http = http;
        Url = url;
        this.caption = caption;
        Localizer = localizer;
    }

    public Task<List<T>> GetAll<T>() => GetAll<T>("Id");

    public Task<List<T>> GetAll<T>(string orderBy, string direction = "asc")
    {
        var queryParams = HttpQueryParameters.Build()
            .AddOptional("orderBy", orderBy, x => !string.IsNullOrEmpty(x))
            .AddOptional("direction", direction, x => !string.IsNullOrEmpty(x));

        var settings = new HttpSettings(Url).AddQueryParams(queryParams);

        return Http.Get<List<T>>(settings).ExecuteWithResultOrElse([]);
    }

    public Task<T?> Get<T>(TKey id)
    {
        var pathParams = HttpPathParameters.Build().Add(id);

        var settings = new HttpSettings(Url).AddPathParams(pathParams);

        return Http.Get<T>(settings).ExecuteWithResult();
    }

    public Task<bool> Create<T>(T model)
    {
        var settings = new HttpSettings(Url).AddToaster(GetMessage(MessageType.Add));

        return Http.Post(settings, model).Execute();
    }

    public Task<bool> Update<T>(TKey id, T model)
    {
        var pathParams = HttpPathParameters.Build().Add(id);

        var settings = new HttpSettings(Url).AddPathParams(pathParams).AddToaster(GetMessage(MessageType.Update));

        return Http.Put(settings, model).Execute();
    }

    public Task<bool> Delete(TKey id)
    {
        var pathParams = HttpPathParameters.Build().Add(id);

        var settings = new HttpSettings(Url).AddPathParams(pathParams).AddToaster(GetMessage(MessageType.Delete));

        return Http.Delete(settings).Execute();
    }

    private string GetMessage(MessageType type)
    {
        if (ObjectHelper.IsNotNull(Localizer))
        {
            return Localizer[$"Toaster.{type}", Localizer["Entity"]];
        }

        return type switch
        {
            MessageType.Delete => $"{caption} deleting",
            MessageType.Update => $"{caption} updating",
            MessageType.Add => $"{caption} adding",
            _ => "",
        };
    }

    private enum MessageType
    {
        Delete,
        Update,
        Add
    }
}