using KarcagS.Blazor.Common.Components.Table;
using KarcagS.Shared.Table;

namespace KarcagS.Blazor.Common.Services;

public interface ITableService
{
    string GetBaseUrl();
    Task<TableResult?> GetData(TableOptions options);
    Task<TableMetaData?> GetMetaData();
}
