using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.Common.Tools.Table;

public interface ITableService<T, TKey> where T : class, IIdentified<TKey>
{
    abstract Table<T, TKey> BuildTable();
    TableResult GetData(QueryModel query);
    TableMetaData GetTableMetaData();
}
