using System.Linq.Expressions;
using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table;
using Microsoft.EntityFrameworkCore;

namespace KarcagS.API.Table.AutoListTable;

public class EFListTableDataSource<T, TKey> : AutoListTableDataSource<T, TKey> where T : class, IIdentified<TKey>
{
    protected readonly Func<QueryModel, Task<IQueryable<T>>> Fetcher;
    protected List<string> EFTextFilteredEntries = [];

    private EFListTableDataSource(Func<QueryModel, Task<IQueryable<T>>> fetcher)
    {
        Fetcher = fetcher;
    }

    public static EFListTableDataSource<T, TKey> Build(Func<QueryModel, Task<IQueryable<T>>> fetcher) => new(fetcher);

    public EFListTableDataSource<T, TKey> SetEFFilteredEntries(params string[] names)
    {
        EFTextFilteredEntries = [.. names];
        return this;
    }

    public override async Task<long> LoadAllDataCountAsync(QueryModel query) => (await Fetcher(query)).LongCount();

    public override async Task<long> LoadFilteredAllDataCountAsync(QueryModel query,
        Configuration<T, TKey> configuration) =>
        GetFilteredQuery(query, configuration, await Fetcher(query)).LongCount();

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

        return [.. fetcherQuery.ApplyPagination(query.Page, query.Size)];
    }

    private IQueryable<T> GetFilteredQuery(QueryModel query, Configuration<T, TKey> configuration,
        IQueryable<T> fetcherQuery)
    {
        ObjectHelper.WhenNotNull(query.TextFilter, filter =>
        {
            if (ObjectHelper.IsNotEmpty(EFTextFilteredEntries))
            {
                fetcherQuery = ApplyEFTextFilter(EFTextFilteredEntries, fetcherQuery, filter);
            }
            else if (ObjectHelper.IsNotEmpty(TextFilteredColumns))
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

    private static IQueryable<T> ApplyEFTextFilter(List<string> entries, IQueryable<T> query, string filter)
    {
        var param = Expression.Parameter(typeof(T), "e");
        var body = entries.Select(entry =>
        {
            var segments = entry.Split('.');
            var p = (Expression)param;
            foreach (var propName in segments) p = Expression.PropertyOrField(p, propName);

            Expression filterBody = Expression.Call(p, "ToLower", Type.EmptyTypes);
            filterBody = Expression.Call(typeof(DbFunctionsExtensions), "Like", Type.EmptyTypes,
                Expression.Constant(EF.Functions), filterBody, Expression.Constant($"%{filter}%".ToLower()));

            return Expression.AndAlso(Expression.NotEqual(p, Expression.Constant(null)), filterBody);
        }).Aggregate(Expression.OrElse);

        var queryExpr = Expression.Call(typeof(Queryable), "Where", [typeof(T)], query.Expression,
            Expression.Lambda(body, param));
        return query.Provider.CreateQuery<T>(queryExpr);
    }
}