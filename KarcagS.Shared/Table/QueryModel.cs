using KarcagS.Shared.Helpers;

namespace KarcagS.Shared.Table;

public class QueryModel
{
    public string? TextFilter { get; set; }
    public int? Page { get; set; }
    public int? Size { get; set; }

    public Dictionary<string, object> ExtraParams { get; set; } = new();

    public bool IsTextFilterNeeded() => ObjectHelper.IsNotNull(TextFilter);

    public bool IsPaginationNeeded() => ObjectHelper.IsNotNull(Page) && ObjectHelper.IsNotNull(Size);
}
