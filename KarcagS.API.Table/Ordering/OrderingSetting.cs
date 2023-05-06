using System.Linq.Expressions;
using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;

namespace KarcagS.API.Table.Ordering;

public class OrderingSetting<T, TKey> where T : class, IIdentified<TKey>
{
    public Expression<Func<T, object?>> Exp { get; set; } = x => x.Id;
    public OrderDirection Direction { get; set; } = OrderDirection.Ascend;
}