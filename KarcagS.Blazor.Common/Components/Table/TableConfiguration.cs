using KarcagS.Blazor.Common.Enums;
using KarcagS.Shared.Common;
using KarcagS.Shared.Helpers;
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

    private TableConfiguration() { }

    public static TableConfiguration<T, TKey> Build() => new();

    public TableConfiguration<T, TKey> AddColumn(TableColumn<T, TKey> column)
    {
        if (Columns.Any(col => col.Key == column.Key))
        {
            return this;
        }

        Columns.Add(column);

        return this;
    }

    public TableConfiguration<T, TKey> IsDense(bool value)
    {
        Dense = value;

        return this;
    }

    public TableConfiguration<T, TKey> IsFixedHeader(bool value)
    {
        FixedHeader = value;

        return this;
    }

    public TableConfiguration<T, TKey> IsHover(bool value)
    {
        Hover = value;

        return this;
    }

    public TableConfiguration<T, TKey> IsStriped(bool value)
    {
        Striped = value;

        return this;
    }

    public TableConfiguration<T, TKey> SetElevation(int value)
    {
        Elevation = value;

        return this;
    }
}

public class TableColumn<T, TKey>
{
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public Color TitleColor { get; set; } = Color.Default;
    public Alignment Alignment { get; set; } = Alignment.Left;
    public Func<T, object> ValueGetter { get; set; } = (data) => default!;
    public Func<T, TKey, Color> ColorGetter { get; set; } = (data, key) => Color.Default;
    public Func<object, string> Formatter { get; set; } = (data) => data?.ToString() ?? string.Empty;

    public TableColumn()
    {

    }

    public TableColumn(TableColumnPreset preset)
    {
        Alignment = preset.Alignment;
        Formatter = preset.Formatter;
    }
}

public class TableColumnPreset
{
    public Alignment Alignment { get; init; } = Alignment.Left;
    public Func<object, string> Formatter { get; init; } = (data) => data?.ToString() ?? string.Empty;
}

public static class Presets
{
    public static readonly TableColumnPreset Number = new()
    {
        Alignment = Alignment.Right,
        Formatter = (data) => Formatters.Format((long)data)
    };

    public static readonly TableColumnPreset Date = new()
    {
        Alignment = Alignment.Left,
        Formatter = (data) => Formatters.Format((DateTime)data)
    };
}

public static class Formatters
{
    public static string Format(long value)
    {
        return value.ToString();
    }

    public static string Format(DateTime value)
    {
        return DateHelper.DateToString(value);
    }
}