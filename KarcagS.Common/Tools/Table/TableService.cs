using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.Common.Tools.Table;

public abstract class TableService<T, TKey> : ITableService<T, TKey> where T : class, IIdentified<TKey>
{
    protected Table<T, TKey> Table { get; set; }

    public TableService()
    {
        Table = BuildTable();
    }

    public abstract Table<T, TKey> BuildTable();
    public abstract DataSource<T, TKey> BuildDataSource();
    public abstract Configuration<T, TKey> BuildConfiguration();

    public TableResult GetData(QueryModel query) => Table.ConstructResult(query);

    public TableMetaData<T, TKey> GetTableMetaData() => Table.GetMetaData();
}
