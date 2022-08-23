using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Shared.Table;

namespace KarcagS.Blazor.Common.Services;

public interface ITableService<TKey>
{
    string GetBaseUrl();
    Task<TableResult<TKey>?> GetData(TableOptions options, Dictionary<string, object> extraParams);
    Task<TableMetaData?> GetMetaData();
}
