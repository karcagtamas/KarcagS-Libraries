using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;
using System.Linq.Expressions;

namespace KarcagS.Common.Tools.Table.Ordering;

public class OrderingBuilder<T, TKey> where T : class, IIdentified<TKey>
{
    private readonly ListTableDataSource<T, TKey> dataSource;

    protected List<OrderingSetting<T, TKey>> Ordering = new();

    public OrderingBuilder(ListTableDataSource<T, TKey> dataSource, Expression<Func<T, object?>> expression, OrderDirection direction)
    {
        this.dataSource = dataSource;
        Ordering.Add(new OrderingSetting<T, TKey> { Exp = expression, Direction = direction });
    }

    public OrderingBuilder<T, TKey> ThenBy(Expression<Func<T, object?>> expression, OrderDirection direction = OrderDirection.Ascend)
    {
        Ordering.Add(new OrderingSetting<T, TKey> { Exp = expression, Direction = direction });

        return this;
    }

    public ListTableDataSource<T, TKey> ApplyOrdering()
    {
        dataSource.Ordering = Ordering;
        return dataSource;
    }
}
