using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Blazor.Common.Http;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.Blazor.Common.Services;

public abstract class TableService<TKey> : ITableService<TKey>
{
    protected readonly IHttpService Http;

    public TableService(IHttpService http)
    {
        Http = http;
    }

    public abstract string GetBaseUrl();

    public Task<TableResult<TKey>?> GetData(TableOptions options)
    {
        var queryParams = HttpQueryParameters.Build()
            .AddTableParams(options);

        var settings = new HttpSettings(Http.BuildUrl(GetBaseUrl(), "Data"))
            .AddQueryParams(queryParams);

        return Http.Get<TableResult<TKey>>(settings).ExecuteWithResult();
    }

    public Task<TableMetaData?> GetMetaData()
    {
        var settings = new HttpSettings(Http.BuildUrl(GetBaseUrl(), "Meta"));

        return Http.Get<TableMetaData>(settings).ExecuteWithResult();
    }
}
