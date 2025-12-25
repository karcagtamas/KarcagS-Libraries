using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.ListTable;

public class TableConfiguration<T, TKey> where T : class, IIdentified<TKey>
{
    public string Title { get; set; } = "Table";

    public List<TableColumn<T, TKey>> Columns { get; set; } = [];

    public TableStyleConfiguration Style { get; set; } = TableStyleConfiguration.Build();

    public TableFilterConfiguration Filter { get; set; } = TableFilterConfiguration.Build();

    public TablePaginationConfiguration Pagination { get; set; } = TablePaginationConfiguration.Build();

    public Func<T, bool> ClickDisableOn { get; set; } = _ => false;

    private TableConfiguration() { }

    public static TableConfiguration<T, TKey> Build() => new();

    public TableConfiguration<T, TKey> AddTitle(string title)
    {
        Title = title;

        return this;
    }

    public TableConfiguration<T, TKey> AddColumn(TableColumn<T, TKey> column)
    {
        if (Columns.Any(col => col.Key == column.Key))
        {
            return this;
        }

        Columns.Add(column);

        return this;
    }

    public TableConfiguration<T, TKey> AddStyle(TableStyleConfiguration style)
    {
        Style = style;

        return this;
    }

    public TableConfiguration<T, TKey> AddFilter(TableFilterConfiguration filter)
    {
        Filter = filter;

        return this;
    }

    public TableConfiguration<T, TKey> AddPagination(TablePaginationConfiguration pagination)
    {
        Pagination = pagination;

        return this;
    }

    public TableConfiguration<T, TKey> DisableClickOn(Func<T, bool> func)
    {
        ClickDisableOn = func;

        return this;
    }
}

public class TableColumn<T, TKey>
{
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public Color TitleColor { get; set; } = Color.Default;
    public Alignment Alignment { get; set; } = Alignment.Left;
    public Func<T, object> ValueGetter { get; set; } = _ => null!;
    public Func<T, TKey, Color> ColorGetter { get; set; } = (_, _) => Color.Default;
    public Func<object, string> Formatter { get; set; } = data => data.ToString() ?? string.Empty;
    public int? Width { get; set; }

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
    public Func<object, string> Formatter { get; init; } = data => data.ToString() ?? string.Empty;
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
