using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Http;
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

    public Task<ResultWrapper<TableResult<TKey>>> GetData(TableOptions options, Dictionary<string, object> extraParams)
    {
        var queryParams = HttpQueryParameters.Build()
            .AddTableParams(options);

        foreach (var item in extraParams)
        {
            queryParams.Add(item.Key, item.Value);
        }

        var settings = new HttpSettings(Http.BuildUrl(GetBaseUrl(), "Data"))
            .AddQueryParams(queryParams);

        return Http.Get<TableResult<TKey>>(settings)
            .ExecuteWithWrapper();
    }

    public Task<ResultWrapper<TableMetaData>> GetMetaData()
    {
        var settings = new HttpSettings(Http.BuildUrl(GetBaseUrl(), "Meta"));

        return Http.Get<TableMetaData>(settings)
            .ExecuteWithWrapper();
    }
}
