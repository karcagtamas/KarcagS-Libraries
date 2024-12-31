using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.API.Table.ListTable;

public class ListTableDataSource<T, TKey> : DataSource<T, TKey> where T : class, IIdentified<TKey>
{
    protected readonly Func<QueryModel, Task<IQueryable<T>>> Fetcher;

    private ListTableDataSource(Func<QueryModel, Task<IQueryable<T>>> fetcher)
    {
        Fetcher = fetcher;
    }

    public static ListTableDataSource<T, TKey> Build(Func<QueryModel, Task<IQueryable<T>>> fetcher) => new(fetcher);

    public override async Task<IEnumerable<T>> LoadDataAsync(QueryModel query, Configuration<T, TKey> configuration) => (await Fetcher(query)).ToList();

    public override async Task<int> LoadAllDataCountAsync(QueryModel query) => (await Fetcher(query)).Count();

    public override async Task<int> LoadFilteredAllDataCountAsync(QueryModel query, Configuration<T, TKey> configuration) => (await Fetcher(query)).Count();
}