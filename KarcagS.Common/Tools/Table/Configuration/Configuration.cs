using KarcagS.Shared.Common;

namespace KarcagS.Common.Tools.Table.Configuration;

public class Configuration<T, TKey> where T : class, IIdentified<TKey>
{
    public string Key { get; set; }
    public string Title { get; set; } = "Table";
    public string? ResourceKey { get; set; }

    public List<Column<T, TKey>> Columns { get; set; } = new();

    public FilterConfiguration Filter { get; set; } = FilterConfiguration.Build();
    public OrderingConfiguration Ordering { get; set; } = OrderingConfiguration.Build();
    public PaginationConfiguration Pagination { get; set; } = PaginationConfiguration.Build();

    public Func<T, bool> ClickDisableOn { get; set; } = _ => false;
    public Func<T, Column<T, TKey>, bool> IsActionsDisabled = (_, _) => false;
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

    public Configuration<T, TKey> SetTitle(string title, string? resourceKey = null)
    {
        Title = title;
        ResourceKey = resourceKey;

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

    public Configuration<T, TKey> AddOrdering(OrderingConfiguration ordering)
    {
        Ordering = ordering;

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

    public Configuration<T, TKey> ActionsDisabledOn(Func<T, Column<T, TKey>, bool> func)
    {
        IsActionsDisabled = func;

        return this;
    }

    public Configuration<T, TKey> AddTagProvider(Func<T, Column<T, TKey>, string> func)
    {
        TagProviders.Add(func);

        return this;
    }
}