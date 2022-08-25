using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Blazor.Common.Http;
using KarcagS.Shared.Table;

namespace KarcagS.Blazor.Common.Services;

public interface ITableService<TKey>
{
    string GetBaseUrl();
    Task<ResultWrapper<TableResult<TKey>>> GetData(TableOptions options, Dictionary<string, object> extraParams);
    Task<ResultWrapper<TableMetaData>> GetMetaData();
}
