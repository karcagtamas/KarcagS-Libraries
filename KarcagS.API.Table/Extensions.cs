using KarcagS.Shared.Enums;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table;
using static System.Int32;

namespace KarcagS.API.Table;

public static class Extensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> queryable, int? page, int? size) =>
        ObjectHelper.IsNotNull(size) && ObjectHelper.IsNotNull(page) ? queryable.Skip((int)size * (int)page).Take((int)size) : queryable;

    public static Dictionary<string, OrderDirection> Ordering(this QueryModel model)
    {
        return model.Ordering.Select(ordering => ordering.Split(";"))
            .Where(items => items.Length > 1)
            .Where(items => TryParse(items[0], out var order) && order is >= 1 and <= 3)
            .Select(items => new KeyValuePair<string, OrderDirection>(items[0], (OrderDirection)Parse(items[1])))
            .ToDictionary();
    }
}