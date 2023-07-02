using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.API.Table.ListTable;

public class ListTable<T, TKey> : Table<T, TKey> where T : class, IIdentified<TKey>
{
    public ListTable(ListTableDataSource<T, TKey> dataSource, Configuration<T, TKey> configuration) : base(dataSource, configuration)
    {
    }

    public override Task<int> GetAllDataCountAsync(QueryModel query) => DataSource.LoadAllDataCountAsync(query);

    public override Task<int> GetAllFilteredCountAsync(QueryModel query) => DataSource.LoadFilteredAllDataCountAsync(query, Configuration);

    public override Task<IEnumerable<T>> GetDataAsync(QueryModel query)
    {
        try
        {
            return DataSource.LoadDataAsync(query, Configuration);
        }
        catch (Exception ex)
        {
            throw new TableException("Data cannot be loaded.", ex);
        }
    }
}
