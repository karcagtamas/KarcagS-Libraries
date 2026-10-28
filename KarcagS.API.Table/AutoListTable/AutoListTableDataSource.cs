using System.Linq.Expressions;
using KarcagS.API.Table.Attributes;
using KarcagS.API.Table.Configurations;
using KarcagS.API.Table.Ordering;
using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table;

namespace KarcagS.API.Table.AutoListTable;

public abstract class AutoListTableDataSource<T, TKey> : DataSource<T, TKey> where T : class, IIdentified<TKey>
{
    protected List<string> TextFilteredColumns = [];
    public List<OrderingSetting<T, TKey>> DefaultOrdering = [];

    public AutoListTableDataSource<T, TKey> SetTextFilteredColumns(params string[] keys)
    {
        TextFilteredColumns = [.. keys];
        return this;
    }

    public OrderingBuilder<T, TKey> OrderBy(Expression<Func<T, object?>> expression,
        OrderDirection direction = OrderDirection.Ascend) => new(this, expression, direction);

    protected List<OrderingSetting<T, TKey>> ConstructOrderingSettingsFromModel(QueryModel model,
        List<Column<T, TKey>> columns)
    {
        return
        [
            .. model.Ordering.Select(x =>
            {
                var t = x.Split(";");
                if (t is [string key, string dir] && int.TryParse(dir, out var d))
                {
                    return KeyValuePair.Create(key, (OrderDirection)d);
                }

                throw new TableException("Ordering cannot be applied because of invalid values.");
            }).Select(e =>
            {
                var col = columns.FirstOrDefault(c => c.Key == e.Key);
                if (ObjectHelper.IsNull(col)) throw new ArgumentException("Invalid column key");

                var param = Expression.Parameter(typeof(T), "e");
                var segments = col.OrderBy.Split('.');
                Expression body = param;
                foreach (var propName in segments)
                {
                    body = Expression.PropertyOrField(body, propName);
                }

                var lambda = Expression.Lambda<Func<T, object?>>(Expression.Convert(body, typeof(object)), param);
                return new OrderingSetting<T, TKey> { Exp = lambda, Direction = e.Value };
            })
        ];
    }

    protected static bool IsIdOrderingFallbackRequired()
    {
        var orderByAttr = Attribute.GetCustomAttribute(typeof(T), typeof(OrderByIdAttribute));
        return ObjectHelper.IsNull(orderByAttr) || ((OrderByIdAttribute)orderByAttr).Enabled;
    }
    
    protected static IOrderedQueryable<T> ApplyOrdering(IQueryable<T> query, Expression<Func<T, object?>> expr, OrderDirection dir) =>
        dir == OrderDirection.Ascend ? query.OrderBy(expr) : query.OrderByDescending(expr);

    protected static IOrderedQueryable<T> ApplyAdditionalOrdering(IOrderedQueryable<T> query, Expression<Func<T, object?>> expr, OrderDirection dir) =>
        dir == OrderDirection.Ascend ? query.ThenBy(expr) : query.ThenByDescending(expr);
}