using System;

namespace KarcagS.Blazor.Common.Enums;

public enum OrderDirection
{
    Ascend = 1,
    Descend = 2,
    None = 3
}

public static class OrderDirectionService
{
    public static string GetValue(OrderDirection direction)
    {
        switch (direction)
        {
            case OrderDirection.Ascend: return "asc";
            case OrderDirection.Descend: return "desc";
            case OrderDirection.None: return "none";
            default: throw new ArgumentException("Direction does not exist");
        }
    }

    public static OrderDirection ValueToKey(string value)
    {
        switch (value)
        {
            case "asc": return OrderDirection.Ascend;
            case "desc": return OrderDirection.Descend;
            case "none": return OrderDirection.None;
            default: throw new ArgumentException("Value does not exist");
        }
    }
}
