using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KarcagS.Common.Tools.Table.ListTable;

public class ListTableDataSource<T, TKey> : DataSource<T, TKey> where T : class, IIdentified<TKey>
{
    protected readonly Func<QueryModel, IQueryable<T>> Fetcher;

    protected List<string> TextFilteredColumns = new();
    protected List<string> EFTextFilteredEntries = new();
    protected Expression<Func<T, object?>> DefaultOrderBy = (x) => x.Id;

    private ListTableDataSource(Func<QueryModel, IQueryable<T>> fetcher)
    {
        Fetcher = fetcher;
    }

    public static ListTableDataSource<T, TKey> Build(Func<QueryModel, IQueryable<T>> fetcher) => new(fetcher);

    public ListTableDataSource<T, TKey> SetTextFilteredColumns(params string[] keys)
    {
        TextFilteredColumns = keys.ToList();

        return this;
    }

    public ListTableDataSource<T, TKey> SetEFFilteredEntries(params string[] names)
    {
        EFTextFilteredEntries = names.ToList();

        return this;
    }

    public ListTableDataSource<T, TKey> ApplyDefaultOrdering(Expression<Func<T, object?>> defaultOrderBy)
    {
        DefaultOrderBy = defaultOrderBy;

        return this;
    }

    public override int LoadAllDataCount(QueryModel query) => Fetcher(query).Count();

    public override int LoadFilteredAllDataCount(QueryModel query, Configuration<T, TKey> configuration) => GetFilteredQuery(query, configuration, Fetcher(query)).Count();

    public override IEnumerable<T> LoadData(QueryModel query, Configuration<T, TKey> configuration)
    {
        var fetcherQuery = Fetcher(query);

        fetcherQuery = GetFilteredQuery(query, configuration, fetcherQuery);

        if (ObjectHelper.IsNotNull(query.Size) && ObjectHelper.IsNotNull(query.Page))
        {
            fetcherQuery = fetcherQuery.Skip((int)query.Size * (int)query.Page).Take((int)query.Size);
        }

        fetcherQuery = fetcherQuery.OrderBy(DefaultOrderBy);

        return fetcherQuery.ToList();
    }

    private static IQueryable<T> ApplyTextFilter(Column<T, TKey> column, IQueryable<T> query, string filter) => query.Where(obj => ((string)column.ValueGetter(obj)).ToLower().Contains(filter.ToLower()));

    private static IQueryable<T> ApplyEFTextFilter(string entry, IQueryable<T> query, string filter) => query.Where(obj => EF.Property<string>(obj, entry).ToLower().Contains(filter.ToLower()));

    private IQueryable<T> GetFilteredQuery(QueryModel query, Configuration<T, TKey> configuration, IQueryable<T> fetcherQuery)
    {
        ObjectHelper.WhenNotNull(query.TextFilter, filter =>
        {
            if (ObjectHelper.IsNotEmpty(EFTextFilteredEntries))
            {
                EFTextFilteredEntries.ForEach(entry => fetcherQuery = ListTableDataSource<T, TKey>.ApplyEFTextFilter(entry, fetcherQuery, filter));
            }
            else if (ObjectHelper.IsNotEmpty(TextFilteredColumns))
            {
                configuration.Columns
                    .Where(col => TextFilteredColumns.Contains(col.Key))
                    .ToList()
                    .ForEach(col => fetcherQuery = ListTableDataSource<T, TKey>.ApplyTextFilter(col, fetcherQuery, filter));
            }
        });

        return fetcherQuery;
    }
}