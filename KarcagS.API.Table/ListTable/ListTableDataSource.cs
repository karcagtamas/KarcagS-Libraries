using System.Linq.Expressions;
using KarcagS.API.Table.Attributes;
using KarcagS.API.Table.Configurations;
using KarcagS.API.Table.Ordering;
using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table;
using Microsoft.EntityFrameworkCore;

namespace KarcagS.API.Table.ListTable;

public class ListTableDataSource<T, TKey> : DataSource<T, TKey> where T : class, IIdentified<TKey>
{
    protected readonly Func<QueryModel, Task<IQueryable<T>>> Fetcher;

    protected List<string> TextFilteredColumns = new();
    protected List<string> EFTextFilteredEntries = new();
    public List<OrderingSetting<T, TKey>> DefaultOrdering = new();

    private ListTableDataSource(Func<QueryModel, Task<IQueryable<T>>> fetcher)
    {
        Fetcher = fetcher;
    }

    public static ListTableDataSource<T, TKey> Build(Func<QueryModel, Task<IQueryable<T>>> fetcher) => new(fetcher);

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

    public OrderingBuilder<T, TKey> OrderBy(Expression<Func<T, object?>> expression,
        OrderDirection direction = OrderDirection.Ascend) => new(this, expression, direction);

    public override async Task<int> LoadAllDataCountAsync(QueryModel query) => (await Fetcher(query)).Count();

    public override async Task<int> LoadFilteredAllDataCountAsync(QueryModel query, Configuration<T, TKey> configuration) =>
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
                orderedQuery =
                    ApplyAdditionalOrdering(orderedQuery, ordering[i].Exp, ordering[i].Direction);
            }

            var orderByAttr = Attribute.GetCustomAttribute(typeof(T), typeof(OrderByIdAttribute));
            if (ObjectHelper.IsNull(orderByAttr) || ((OrderByIdAttribute)orderByAttr).Enabled)
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

        return fetcherQuery.ToList();
    }

    private static IQueryable<T> ApplyTextFilter(Column<T, TKey> column, IQueryable<T> query, string filter) => query.Where(obj => ((string?)column.ValueGetter(obj).Result ?? "").ToLower().Contains(filter.ToLower()));

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
            .Aggregate(Expression.OrElse);

        var lambda = Expression.Lambda(body, param);

        var queryExpr = Expression.Call(typeof(Queryable), "Where", new[] { typeof(T) }, query.Expression, lambda);

        return query.Provider.CreateQuery<T>(queryExpr);
    }

    private static IOrderedQueryable<T> ApplyOrdering(IQueryable<T> query, Expression<Func<T, object?>> expression, OrderDirection direction)
    {
        return direction switch
        {
            OrderDirection.Ascend => query.OrderBy(expression),
            OrderDirection.Descend => query.OrderByDescending(expression),
            _ => throw new TableException("Ordering cannot be applied.")
        };
    }

    private static IOrderedQueryable<T> ApplyAdditionalOrdering(IOrderedQueryable<T> query, Expression<Func<T, object?>> expression, OrderDirection direction)
    {
        return direction switch
        {
            OrderDirection.Ascend => query.ThenBy(expression),
            OrderDirection.Descend => query.ThenByDescending(expression),
            _ => throw new TableException("Ordering cannot be applied.")
        };
    }

    private IQueryable<T> GetFilteredQuery(QueryModel query, Configuration<T, TKey> configuration, IQueryable<T> fetcherQuery)
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

    private List<OrderingSetting<T, TKey>> ConstructOrderingSettingsFromModel(QueryModel model, List<Column<T, TKey>> columns)
    {
        return model.Ordering.Select(x =>
        {
            var t = x.Split(";");

            if (t is [string key, string dir])
            {
                if (int.TryParse(dir, out var d))
                {
                    return KeyValuePair.Create(key, (OrderDirection)d);
                }
            }

            throw new TableException("Ordering cannot be applied because of invalid values.");
        }).Select(e =>
        {
            var entityType = typeof(T);
            var col = columns.FirstOrDefault(c => c.Key == e.Key);

            if (ObjectHelper.IsNull(col))
            {
                throw new ArgumentException("Invalid column key");
            }

            var param = Expression.Parameter(entityType);

            var segments = col.OrderBy.Split('.');
            var body = (Expression)param;
            foreach (var propName in segments)
            {
                body = Expression.PropertyOrField(body, propName);
            }

            var lambda = Expression.Lambda<Func<T, object?>>(Expression.Convert(body, typeof(object)), param);

            return new OrderingSetting<T, TKey> { Exp = lambda, Direction = e.Value };
        }).ToList();
    }
}