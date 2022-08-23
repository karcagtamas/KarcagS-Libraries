using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.Common.Tools.Table.ListTable;

public class ListTable<T, TKey> : Table<T, TKey> where T : class, IIdentified<TKey>
{
    public ListTable(ListTableDataSource<T, TKey> dataSource, Configuration<T, TKey> configuration) : base(dataSource, configuration)
    {
    }

    public override int GetAllDataCount(QueryModel query) => DataSource.LoadAllDataCount(query);

    public override int GetAllFilteredCount(QueryModel query) => DataSource.LoadFilteredAllDataCount(query, Configuration);

    public override IEnumerable<T> GetData(QueryModel query)
    {
        try
        {
            return DataSource.LoadData(query, Configuration);
        }
        catch (Exception ex)
        {
            throw new TableException("Data cannot be loaded.", ex);
        }
    }
}
