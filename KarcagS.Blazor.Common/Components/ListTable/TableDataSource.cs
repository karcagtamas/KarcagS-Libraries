using KarcagS.Shared.Common;
using KarcagS.Shared.Table;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.ListTable;

public class TableDataSource<T, TKey> where T : class, IIdentified<TKey>
{
    private readonly Func<TableOptions, Task<TableResult<TKey>>> fetcher;
    private readonly List<T> rawData = [];
    private int allDataCount;
    private bool initialized;

    private Predicate<T> isDisabled = _ => false;
    private Predicate<T> isHidden = _ => false;

    private ListTable<T, TKey> tableInstance = null!;

    private List<TKey> preSelection = [];

    public List<RowItem<T, TKey>> Data = [];

    public List<T> RawData => Data.Select(x => x.Data).ToList();
    public int AllDataCount => allDataCount;

    public TableDataSource(Func<TableOptions, Task<TableResult<TKey>>> fetcher)
    {
        this.fetcher = fetcher;
    }

    public async Task Init(ListTable<T, TKey> instance)
    {
        await Init(instance, []);
    }

    public Task Init(ListTable<T, TKey> instance, List<TKey> preSelectedKeys)
    {
        if (initialized)
        {
            return Task.CompletedTask;
        }

        preSelection = preSelectedKeys;

        tableInstance = instance;

        initialized = true;

        return Task.CompletedTask;
    }

    public async Task Refresh(TableState state)
    {
        await Fetch(state);

        Data.ForEach(x =>
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

        Data.ForEach(x => { x.Hidden = isHidden(x.Data); });

        return this;
    }

    public TableDataSource<T, TKey> SetDisabledPredicate(Predicate<T> predicate)
    {
        isDisabled = predicate;

        Data.ForEach(x => { x.Disabled = isDisabled(x.Data); });

        return this;
    }

    private async Task Fetch(TableState state)
    {
        var result = await fetcher(new TableOptions
        {
            Filter = tableInstance.GetCurrentFilter(),
            Pagination = tableInstance.Config.Pagination.PaginationEnabled
                ? new TablePagination
                {
                    Page = state.Page,
                    Size = state.PageSize
                }
                : null
        });

        //rawData = result.Items;
        allDataCount = result.All;

        Data = rawData.Select(x => new RowItem<T, TKey> { Id = x.Id, Data = x })
            .ToList();

        Data.ForEach(x =>
        {
            x.Hidden = isHidden(x.Data);
            x.Disabled = isDisabled(x.Data);
        });
    }
}

public class RowItem<T, TKey> where T : class, IIdentified<TKey>
{
    public TKey Id { get; set; } = default!;
    public T Data { get; set; } = null!;
    public bool Selected { get; set; }
    public bool Disabled { get; set; }
    public bool Hidden { get; set; }
}