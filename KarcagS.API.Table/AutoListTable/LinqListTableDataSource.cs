using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Common;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table;

namespace KarcagS.API.Table.AutoListTable;

public class LinqListTableDataSource<T, TKey> : AutoListTableDataSource<T, TKey> where T : class, IIdentified<TKey>
{
    protected readonly Func<QueryModel, Task<IQueryable<T>>> Fetcher;

    private LinqListTableDataSource(Func<QueryModel, Task<IQueryable<T>>> fetcher)
    {
        Fetcher = fetcher;
    }

    public static LinqListTableDataSource<T, TKey> Build(Func<QueryModel, Task<IQueryable<T>>> fetcher) => new(fetcher);

    public override async Task<int> LoadAllDataCountAsync(QueryModel query) => (await Fetcher(query)).Count();

    public override async Task<int> LoadFilteredAllDataCountAsync(QueryModel query,
        Configuration<T, TKey> configuration) =>
        GetFilteredQuery(query, configuration, await Fetcher(query)).Count();

    public override async Task<IEnumerable<T>> LoadDataAsync(QueryModel query, Configuration<T, TKey> configuration)
    {
        var fetcherQuery = await Fetcher(query);
        var ordering = query.IsOrderingNeeded()
            ? ConstructOrderingSettingsFromModel(query, configuration.Columns)
            : DefaultOrdering;

        if (ObjectHelper.IsEmpty(ordering))
        {
            fetcherQuery = fetcherQuery.OrderBy(x => x.Id);
        }
        else
        {
            var orderedQuery = ApplyOrdering(fetcherQuery, ordering[0].Exp, ordering[0].Direction);
            for (var i = 1; i < ordering.Count; i++)
            {
                orderedQuery = ApplyAdditionalOrdering(orderedQuery, ordering[i].Exp, ordering[i].Direction);
            }

            if (IsIdOrderingFallbackRequired())
            {
                orderedQuery = orderedQuery.ThenBy(x => x.Id);
            }

            fetcherQuery = orderedQuery;
        }

        fetcherQuery = GetFilteredQuery(query, configuration, fetcherQuery);

        if (ObjectHelper.IsNotNull(query.Size) && ObjectHelper.IsNotNull(query.Page))
        {
            fetcherQuery = fetcherQuery.Skip((int)query.Size * (int)query.Page).Take((int)query.Size);
        }

        return fetcherQuery.ApplyPagination(query.Page, query.Size).ToList();
    }

    private IQueryable<T> GetFilteredQuery(QueryModel query, Configuration<T, TKey> configuration,
        IQueryable<T> fetcherQuery)
    {
        ObjectHelper.WhenNotNull(query.TextFilter, filter =>
        {
            if (ObjectHelper.IsNotEmpty(TextFilteredColumns))
            {
                configuration.Columns
                    .Where(col => TextFilteredColumns.Contains(col.Key))
                    .ToList()
                    .ForEach(col => fetcherQuery = ApplyTextFilter(col, fetcherQuery, filter));
            }
        });

        return fetcherQuery;
    }

    private static IQueryable<T> ApplyTextFilter(Column<T, TKey> column, IQueryable<T> query, string filter) =>
        query.Where(obj => ((string?)column.ValueGetter(obj).GetAwaiter().GetResult() ?? "").ToLower()
            .Contains(filter.ToLower()));
}