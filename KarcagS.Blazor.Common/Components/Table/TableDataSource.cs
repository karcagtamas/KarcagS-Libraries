using KarcagS.Shared.Common;

namespace KarcagS.Blazor.Common.Components.Table;

public class TableDataSource<T, TKey> where T : class, IIdentified<TKey>
{
    private readonly Func<Task<List<T>>> fetcher;
    private List<T> rawData = new();
    private bool initialized = false;

    private Predicate<T> isDisabled = (data) => false;
    private Predicate<T> isHidden = (data) => false;

    private List<TKey> preSelection = new();

    public List<RowItem<T, TKey>> data = new();

    public TableDataSource(Func<Task<List<T>>> fetcher)
    {
        this.fetcher = fetcher;
    }

    public async Task Init()
    {
        await Init(new());
    }

    public async Task Init(List<TKey> preSelection)
    {
        if (initialized)
        {
            return;
        }

        this.preSelection = preSelection;

        await Refresh();

        initialized = true;
    }

    public async Task Refresh()
    {
        await Fetch();

        data.ForEach(x =>
        {
            if (preSelection.Contains(x.Id))
            {
                x.Selected = true;
            }
        });
    }

    public TableDataSource<T, TKey> SetHiddenPredicate(Predicate<T> predicate)
    {
        isHidden = predicate;

        data.ForEach(x =>
        {
            x.Hidden = isHidden(x.Data);
        });

        return this;
    }

    public TableDataSource<T, TKey> SetDisabledPredicate(Predicate<T> predicate)
    {
        isDisabled = predicate;

        data.ForEach(x =>
        {
            x.Disabled = isDisabled(x.Data);
        });

        return this;
    }

    private async Task Fetch()
    {
        rawData = await fetcher();

        data = rawData.Select(x => new RowItem<T, TKey> { Id = x.Id, Data = x })
            .ToList();

        data.ForEach(x =>
        {
            x.Hidden = isHidden(x.Data);
            x.Disabled = isDisabled(x.Data);
        });
    }

    
}

public class RowItem<T, TKey> where T : class, IIdentified<TKey>
{
    public TKey Id { get; set; } = default!;
    public T Data { get; set; } = default!;
    public bool Selected { get; set; }
    public bool Disabled { get; set; }
    public bool Hidden { get; set; }
}
