using KarcagS.Blazor.Common.Enums;
using KarcagS.Shared.Common;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table;

public class TableConfiguration<T, TKey> where T : class, IIdentified<TKey>
{
    public List<TableColumn<T, TKey>> Columns { get; set; } = new();
    public bool Dense { get; set; } = true;
    public bool FixedHeader { get; set; } = true;
    public bool Hover { get; set; } = true;
    public bool Striped { get; set; } = true;
    public int Elevation { get; set; } = 2;
}

public class TableColumn<T, TKey>
{
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public Color TitleColor { get; set; } = Color.Default;
    public Alignment Alignment { get; set; } = Alignment.Left;
    public Func<T, string> ValueGetter { get; set; } = (data) => string.Empty;
    public Func<T, TKey, Color> ColorGetter { get; set; } = (data, key) => Color.Default;
}
