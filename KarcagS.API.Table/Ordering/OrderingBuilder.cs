using System.Linq.Expressions;
using KarcagS.API.Table.ListTable;
using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;

namespace KarcagS.API.Table.Ordering;

public class OrderingBuilder<T, TKey> where T : class, IIdentified<TKey>
{
    private readonly ListTableDataSource<T, TKey> dataSource;
    private readonly List<OrderingSetting<T, TKey>> ordering = new();

    public OrderingBuilder(ListTableDataSource<T, TKey> dataSource, Expression<Func<T, object?>> expression, OrderDirection direction)
    {
        this.dataSource = dataSource;
        ordering.Add(new OrderingSetting<T, TKey> { Exp = expression, Direction = direction });
    }

    public OrderingBuilder<T, TKey> ThenBy(Expression<Func<T, object?>> expression, OrderDirection direction = OrderDirection.Ascend)
    {
        ordering.Add(new OrderingSetting<T, TKey> { Exp = expression, Direction = direction });

        return this;
    }

    public ListTableDataSource<T, TKey> ApplyOrdering()
    {
        dataSource.DefaultOrdering = ordering;
        return dataSource;
    }
}