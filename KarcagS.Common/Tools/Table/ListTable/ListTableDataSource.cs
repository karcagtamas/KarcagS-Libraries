using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.Ordering;
using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Table;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KarcagS.Common.Tools.Table.ListTable;

public partial class ListTableDataSource<T, TKey> : DataSource<T, TKey> where T : class, IIdentified<TKey>
{
    protected readonly Func<QueryModel, IQueryable<T>> Fetcher;

    protected List<string> TextFilteredColumns = new();
    protected List<string> EFTextFilteredEntries = new();
    public List<OrderingSetting<T, TKey>> Ordering = new();

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

    public OrderingBuilder<T, TKey> OrderBy(Expression<Func<T, object?>> expression, OrderDirection direction = OrderDirection.Ascend) => new OrderingBuilder<T, TKey>(this, expression, direction);

    public override int LoadAllDataCount(QueryModel query) => Fetcher(query).Count();

    public override int LoadFilteredAllDataCount(QueryModel query, Configuration<T, TKey> configuration) => GetFilteredQuery(query, configuration, Fetcher(query)).Count();

    public override IEnumerable<T> LoadData(QueryModel query, Configuration<T, TKey> configuration)
    {
        var fetcherQuery = Fetcher(query);

        if (ObjectHelper.IsEmpty(Ordering))
        {
            fetcherQuery = fetcherQuery.OrderBy(x => x.Id);
        }
        else
        {
            var orderedQuery = ApplyOrdering(fetcherQuery, Ordering[0].Exp, Ordering[0].Direction);
            for (int i = 1; i < Ordering.Count; i++)
            {
                orderedQuery = ApplyAdditionalOrdering(orderedQuery, Ordering[i].Exp, Ordering[i].Direction);
            }
            fetcherQuery = orderedQuery;
        }

        fetcherQuery = GetFilteredQuery(query, configuration, fetcherQuery);

        if (ObjectHelper.IsNotNull(query.Size) && ObjectHelper.IsNotNull(query.Page))
        {
            fetcherQuery = fetcherQuery.Skip((int)query.Size * (int)query.Page).Take((int)query.Size);
        }

        return fetcherQuery.ToList();
    }

    private static IQueryable<T> ApplyTextFilter(Column<T, TKey> column, IQueryable<T> query, string filter) => query.Where(obj => ((string)column.ValueGetter(obj)).ToLower().Contains(filter.ToLower()));

    private static IQueryable<T> ApplyEFTextFilter(List<string> entries, IQueryable<T> query, string filter)
    {
        var param = Expression.Parameter(typeof(T), "e");
        var body = entries
            .Select(entry =>
            {
                var segments = entry.Split('.');
                var p = (Expression)param;
                foreach (var propName in segments)
                {
                    p = Expression.PropertyOrField(p, propName);
                }

                Expression filterBody = Expression.Call(p, "ToLower", Type.EmptyTypes);
                filterBody = Expression.Call(typeof(DbFunctionsExtensions), "Like", Type.EmptyTypes,
                    Expression.Constant(EF.Functions), filterBody, Expression.Constant($"%{filter}%".ToLower()));

                var checkerBody = Expression.NotEqual(p, Expression.Constant(null));

                return Expression.AndAlso(checkerBody, filterBody);
            })
            .Aggregate((a, b) => Expression.OrElse(a, b));

        var lambda = Expression.Lambda(body, param);

        var queryExpr = Expression.Call(typeof(Queryable), "Where", new[] { typeof(T) }, query.Expression, lambda);

        return query.Provider.CreateQuery<T>(queryExpr);
    }

    private static IOrderedQueryable<T> ApplyOrdering(IQueryable<T> query, Expression<Func<T, object?>> expression, OrderDirection direction)
    {
        if (direction == OrderDirection.Ascend)
        {
            return query.OrderBy(expression);
        }
        else if (direction == OrderDirection.Descend)
        {
            return query.OrderByDescending(expression);
        }

        throw new TableException("Ordering cannot be applied.");
    }

    private static IOrderedQueryable<T> ApplyAdditionalOrdering(IOrderedQueryable<T> query, Expression<Func<T, object?>> expression, OrderDirection direction)
    {
        if (direction == OrderDirection.Ascend)
        {
            return query.ThenBy(expression);
        }
        else if (direction == OrderDirection.Descend)
        {
            return query.ThenByDescending(expression);
        }

        throw new TableException("Ordering cannot be applied.");
    }

    private IQueryable<T> GetFilteredQuery(QueryModel query, Configuration<T, TKey> configuration, IQueryable<T> fetcherQuery)
    {
        ObjectHelper.WhenNotNull(query.TextFilter, filter =>
        {
            if (ObjectHelper.IsNotEmpty(EFTextFilteredEntries))
            {
                fetcherQuery = ListTableDataSource<T, TKey>.ApplyEFTextFilter(EFTextFilteredEntries, fetcherQuery, filter);
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