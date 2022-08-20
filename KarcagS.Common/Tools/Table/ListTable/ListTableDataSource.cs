using KarcagS.Shared.Common;

namespace KarcagS.Common.Tools.Table.ListTable;

public class ListTableDataSource<T, TKey> : DataSource<T, TKey> where T : class, IIdentified<TKey>
{
    protected readonly Func<IQueryable<T>> Fetcher;

    private ListTableDataSource(Func<IQueryable<T>> fetcher)
    {
        Fetcher = fetcher;
    }

    public static ListTableDataSource<T, TKey> Build(Func<IQueryable<T>> fetcher)
    {
        return new ListTableDataSource<T, TKey>(fetcher);
    }

    public override IEnumerable<T> LoadData(TableOptions options)
    {
        return Fetcher().ToList();
    }
}
