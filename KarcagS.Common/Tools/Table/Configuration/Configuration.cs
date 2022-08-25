using KarcagS.Shared.Common;

namespace KarcagS.Common.Tools.Table.Configuration;

public class Configuration<T, TKey> where T : class, IIdentified<TKey>
{
    public string Key { get; set; }
    public string Title { get; set; } = "Table";
    public bool ReadOnly { get; set; } = false;

    public List<Column<T, TKey>> Columns { get; set; } = new();

    public FilterConfiguration Filter { get; set; } = FilterConfiguration.Build();

    public PaginationConfiguration Pagination { get; set; } = PaginationConfiguration.Build();

    public Func<T, bool> ClickDisableOn { get; set; } = (data) => false;
    public List<Func<T, Column<T, TKey>, string>> TagProviders { get; set; } = new();

    private Configuration()
    {
        Key = "table";
    }

    private Configuration(string key)
    {
        Key = key;
    }

    public static Configuration<T, TKey> Build(string key) => new(key);

    public Configuration<T, TKey> SetTitle(string title)
    {
        Title = title;

        return this;
    }

    public Configuration<T, TKey> IsReadOnly(bool value)
    {
        ReadOnly = value;

        return this;
    }

    public Configuration<T, TKey> AddColumn(Column<T, TKey> column)
    {
        if (Columns.Any(col => col.Key == column.Key))
        {
            return this;
        }

        Columns.Add(column);

        return this;
    }

    public Configuration<T, TKey> AddFilter(FilterConfiguration filter)
    {
        Filter = filter;

        return this;
    }

    public Configuration<T, TKey> AddPagination(PaginationConfiguration pagination)
    {
        Pagination = pagination;

        return this;
    }

    public Configuration<T, TKey> DisableClickOn(Func<T, bool> func)
    {
        ClickDisableOn = func;

        return this;
    }

    public Configuration<T, TKey> AddTagProvider(Func<T, Column<T, TKey>, string> func)
    {
        TagProviders.Add(func);

        return this;
    }
}
