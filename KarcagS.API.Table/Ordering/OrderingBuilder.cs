using System.Linq.Expressions;
using KarcagS.API.Table.AutoListTable;
using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;

namespace KarcagS.API.Table.Ordering;

public class OrderingBuilder<T, TKey> where T : class, IIdentified<TKey>
{
    private readonly AutoListTableDataSource<T, TKey> dataSource;
    private readonly List<OrderingSetting<T, TKey>> ordering = [];

    public OrderingBuilder(AutoListTableDataSource<T, TKey> dataSource, Expression<Func<T, object?>> expression, OrderDirection direction)
    {
        this.dataSource = dataSource;
        ordering.Add(new OrderingSetting<T, TKey> { Exp = expression, Direction = direction });
    }

    public OrderingBuilder<T, TKey> ThenBy(Expression<Func<T, object?>> expression, OrderDirection direction = OrderDirection.Ascend)
    {
        ordering.Add(new OrderingSetting<T, TKey> { Exp = expression, Direction = direction });

        return this;
    }

    public AutoListTableDataSource<T, TKey> ApplyOrdering()
    {
        dataSource.DefaultOrdering = ordering;
        return dataSource;
    }
}