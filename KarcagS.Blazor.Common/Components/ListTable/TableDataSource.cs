using KarcagS.Shared.Common;
using KarcagS.Shared.Table;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.ListTable;

public class TableDataSource<T, TKey> where T : class, IIdentified<TKey>
{
    private readonly Func<TableOptions, Task<TableResult<TKey>>> fetcher;
    private readonly List<T> rawData = [];
    private int allDataCount = 0;
    private bool initialized = false;

    private Predicate<T> isDisabled = (data) => false;
    private Predicate<T> isHidden = (data) => false;

    private ListTable<T, TKey> tableInstance = null!;

    private List<TKey> preSelection = new();

    public List<RowItem<T, TKey>> Data = new();

    public List<T> RawData => Data.Select(x => x.Data).ToList();
    public int AllDataCount => allDataCount;

    public TableDataSource(Func<TableOptions, Task<TableResult<TKey>>> fetcher)
    {
        this.fetcher = fetcher;
    }

    public async Task Init(ListTable<T, TKey> tableInstance)
    {
        await Init(tableInstance, new());
    }

    public Task Init(ListTable<T, TKey> tableInstance, List<TKey> preSelection)
    {
        if (initialized)
        {
            return Task.CompletedTask;
        }

        this.preSelection = preSelection;

        this.tableInstance = tableInstance;

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

        Data.ForEach(x =>
        {
            x.Hidden = isHidden(x.Data);
        });

        return this;
    }

    public TableDataSource<T, TKey> SetDisabledPredicate(Predicate<T> predicate)
    {
        isDisabled = predicate;

        Data.ForEach(x =>
        {
            x.Disabled = isDisabled(x.Data);
        });

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
