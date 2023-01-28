using KarcagS.Shared.Helpers;

namespace KarcagS.Shared.Table;

public class QueryModel
{
    public string? TextFilter { get; set; }
    public int? Page { get; set; }
    public int? Size { get; set; }
    public List<string> Ordering { get; set; } = new();

    public Dictionary<string, object> ExtraParams { get; set; } = new();

    public bool IsTextFilterNeeded() => ObjectHelper.IsNotNull(TextFilter);

    public bool IsPaginationNeeded() => ObjectHelper.IsNotNull(Page) && ObjectHelper.IsNotNull(Size);

    public bool IsOrderingNeeded() => ObjectHelper.IsNotNull(Ordering) && Ordering.Count > 0;
}