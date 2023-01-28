using KarcagS.Shared.Enums;

namespace KarcagS.Blazor.Common.Components.Table;

public class Order
{
    public string Key { get; set; } = default!;
    public OrderDirection Direction { get; set; }

    public override string ToString() => $"{Key};{(int)Direction}";
}