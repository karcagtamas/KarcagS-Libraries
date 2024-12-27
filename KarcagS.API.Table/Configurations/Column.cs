using KarcagS.Shared.Common;
using KarcagS.Shared.Table.Enums;

namespace KarcagS.API.Table.Configurations;

public class Column<T, TKey> where T : class, IIdentified<TKey>
{
    public string Key { get; }
    public string Title { get; set; } = string.Empty;
    public string? ResourceKey { get; set; }
    public Func<T, Task<object?>> ValueGetter { get; set; } = _ => Task.FromResult(default(object?));
    public ColumnFormatter Formatter { get; set; } = ColumnFormatter.Text;
    public string[] FormatterArgs { get; set; } = [];
    public bool IsAction { get; set; }
    public string OrderBy { get; set; } = string.Empty;
    public bool IsSortable { get; set; }

    private Column(string key)
    {
        Key = key;
        ResourceKey = $"Table.Column.{key}";
    }

    public static Column<T, TKey> Build(string key) => new(key);

    public Column<T, TKey> SetTitle(string value)
    {
        Title = value;

        return this;
    }

    public Column<T, TKey> AddValueGetter(Func<T, Task<object?>> getter)
    {
        ValueGetter = getter;

        return this;
    }

    public Column<T, TKey> AddValueGetter(Func<T, object?> getter) => AddValueGetter(obj => Task.FromResult(getter(obj)));

    public Column<T, TKey> SetFormatter(ColumnFormatter value, params string[] args)
    {
        Formatter = value;
        FormatterArgs = args;

        return this;
    }

    public Column<T, TKey> MarkAsAction(bool isAction = true)
    {
        IsAction = isAction;

        return this;
    }

    public Column<T, TKey> MarkAsSortable(string by, bool sortable = true)
    {
        OrderBy = by;
        IsSortable = sortable;

        return this;
    }
}