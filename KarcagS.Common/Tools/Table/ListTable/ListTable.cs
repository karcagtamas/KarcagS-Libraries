using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.Common.Tools.Table.ListTable;

public class ListTable<T, TKey> : Table<T, TKey> where T : class, IIdentified<TKey>
{
    public ListTable(ListTableDataSource<T, TKey> dataSource, Configuration<T, TKey> configuration) : base(dataSource, configuration)
    {
    }

    public override int GetAllDataCount() => DataSource.LoadAllDataCount();

    public override IEnumerable<T> GetData(QueryModel query) => DataSource.LoadData(query, Configuration);
}
