using KarcagS.Shared.Common;

namespace KarcagS.Common.Tools.Table;

public interface ITableService<T, TKey> where T : class, IIdentified<TKey>
{
    abstract Table<T, TKey> BuildTable();
    List<T> GetData();
    TableMetaData<T, TKey> GetTableMetaData();
}
