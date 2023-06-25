using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Http;
using KarcagS.Shared.Table;

namespace KarcagS.Blazor.Common.Services.Interfaces;

public interface ITableService<TKey>
{
    string GetBaseUrl();
    Task<ResultWrapper<TableResult<TKey>>> GetData(TableOptions options, Dictionary<string, object> extraParams);
    Task<ResultWrapper<TableMetaData>> GetMetaData();
}