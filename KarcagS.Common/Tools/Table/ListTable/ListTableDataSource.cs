using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.Common.Tools.Table.ListTable;

public class ListTableDataSource<T, TKey> : DataSource<T, TKey> where T : class, IIdentified<TKey>
{
    protected readonly Func<IQueryable<T>> Fetcher;

    protected List<string> TextFilteredColumns = new();

    private ListTableDataSource(Func<IQueryable<T>> fetcher)
    {
        Fetcher = fetcher;
    }

    public static ListTableDataSource<T, TKey> Build(Func<IQueryable<T>> fetcher) => new(fetcher);

    public ListTableDataSource<T, TKey> SetListFilteredColumns(params string[] keys)
    {
        TextFilteredColumns = keys.ToList();

        return this;
    }

    public override int LoadAllDataCount() => Fetcher().Count();

    public override IEnumerable<T> LoadData(QueryModel query, Configuration<T, TKey> configuration)
    {
        var fetcherQuery = Fetcher();

        ObjectHelper.WhenNotNull(query.TextFilter, filter =>
        {
            configuration.Columns
                .Where(x => TextFilteredColumns.Contains(x.Key))
                .ToList()
                .ForEach(x =>
                {
                    fetcherQuery = fetcherQuery.Where(obj => ((string)x.ValueGetter(obj)).ToLower().Contains(filter.ToLower()));
                });
        });

        if (ObjectHelper.IsNotNull(query.Size) && ObjectHelper.IsNotNull(query.Page))
        {
            fetcherQuery = fetcherQuery.Skip((int)query.Size * (int)query.Page).Take((int)query.Size);
        }

        return fetcherQuery.ToList();
    }
}
