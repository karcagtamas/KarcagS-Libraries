using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Table.Enums;

namespace KarcagS.Common.Tools.Table.Configuration;

public class Column<T, TKey> where T : class, IIdentified<TKey>
{
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? ResourceKey { get; set; }
    public Alignment Alignment { get; set; } = Alignment.Left;
    public Func<T, object> ValueGetter { get; set; } = (data) => default!;
    public ColumnFormatter Formatter { get; set; } = ColumnFormatter.Text;
    public string[] FormatterArgs { get; set; } = new string[0];
    public int? Width { get; set; } = null;

    private Column(string key)
    {
        Key = key;
    }

    public static Column<T, TKey> Build(string key) => new(key);

    public Column<T, TKey> SetTitle(string value, string? resourecKey = null)
    {
        Title = value;
        ResourceKey = resourecKey;

        return this;
    }

    public Column<T, TKey> SetAlignment(Alignment value)
    {
        Alignment = value;

        return this;
    }

    public Column<T, TKey> AddValueGetter(Func<T, object> getter)
    {
        ValueGetter = getter;

        return this;
    }

    public Column<T, TKey> SetFormatter(ColumnFormatter value, params string[] args)
    {
        Formatter = value;
        FormatterArgs = args;

        return this;
    }

    public Column<T, TKey> SetWidth(int value)
    {
        Width = value;

        return this;
    }
}
