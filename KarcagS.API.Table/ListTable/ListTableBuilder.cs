using KarcagS.Shared.Common;
using KarcagS.Shared.Helpers;

namespace KarcagS.API.Table.ListTable;

public class ListTableBuilder<T, TKey> : TableBuilder<T, TKey> where T : class, IIdentified<TKey>
{
    public static ListTableBuilder<T, TKey> Construct() => new();

    public override Table<T, TKey> Build()
    {
        return ObjectHelper.IsNotNull(DataSource) && ObjectHelper.IsNotNull(Configuration) && DataSource is ListTableDataSource<T, TKey> ltDataSource
            ? new ListTable<T, TKey>(ltDataSource, Configuration)
            : throw new TableException("Build cannot be produced. Invalid DataSource or Configuration detected.");
    }
}