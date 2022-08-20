using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;

namespace KarcagS.Common.Tools.Table.ListTable;

public class ListTable<T, TKey> : Table<T, TKey> where T : class, IIdentified<TKey>
{
    public ListTable(ListTableDataSource<T, TKey> dataSource, Configuration<T, TKey> configuration) : base(dataSource, configuration)
    {
    }

    public override IEnumerable<T> GetData()
    {
        throw new NotImplementedException();
    }
}
