using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.API.Table;

public interface ITableService<T, TKey> where T : class, IIdentified<TKey>
{
    Task InitializeAsync();
    abstract Task<Table<T, TKey>> BuildTableAsync();
    Task<TableResult<TKey>> GetDataAsync(QueryModel query);
    Task<TableMetaData> GetTableMetaDataAsync();
    Task<bool> AuthorizeAsync(QueryModel query);
}
