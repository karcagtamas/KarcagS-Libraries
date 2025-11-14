using System.Reactive.Subjects;
using KarcagS.Blazor.Common.Components.Table.Styles;
using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Client.Common.Services.Interfaces;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Http;
using KarcagS.Shared.Table;
using KarcagS.Shared.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table;

public partial class Table<TKey> : ComponentBase, IDisposable
{
    [Parameter]
    public RenderFragment<Table<TKey>>? FilterRow { get; set; }

    [Parameter, EditorRequired]
    public ITableService<TKey> Service { get; set; } = null!;

    [Parameter]
    public StyleConfiguration<TKey> TableStyle { get; set; } = StyleConfiguration<TKey>.Build();

    [Parameter]
    public EventCallback<TableRowClickArgs<TableRowItem<TKey>>> OnRowClick { get; set; }

    [Parameter]
    public string Class { get; set; } = string.Empty;

    [Parameter]
    public Dictionary<string, object> InitialParams { get; set; } = new();

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public IStringLocalizer? Localizer { get; set; }

    [Parameter]
    public EventCallback<KeyValuePair<string, TableRowItem<TKey>>> OnAction { get; set; }

    [Inject]
    private ILocalizationService LocalizationService { get; set; } = null!;

    private MudTable<TableRowItem<TKey>>? TableComponent { get; set; }

    private string AppendedClass => $"lib-table w-100 flex-box h-100 {Class}";

    private bool Loading { get; set; } = false;
    private string? TextFilter { get; set; }
    private Dictionary<string, object> Params { get; set; } = new();
    private List<Order> Orders { get; set; } = [];

    private TableMetaData? MetaData { get; set; }
    private Dictionary<string, TableHeaderStyle<TKey>> ColumnStyles { get; set; } = new();
    private HttpErrorResult? ErrorResult { get; set; }

    private readonly Subject<TableRowClickArgs<TableRowItem<TKey>>> rowClickSubject = new();
    private IDisposable? disposable;

    protected override async Task OnInitializedAsync()
    {
        foreach (var p in InitialParams)
        {
            Params.Add(p.Key, p.Value);
        }

        await LoadMetaData();
        await base.OnInitializedAsync();

        disposable = rowClickSubject.ThrottleMax(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(800))
            .Subscribe(async void (args) => { await OnRowClick.InvokeAsync(args); });
    }

    private async Task LoadMetaData()
    {
        var meta = await Service.GetMetaData();

        if (ObjectHelper.IsNotNull(meta.Error))
        {
            ErrorResult = meta.Error;
        }

        MetaData = meta.Result;
        ColumnStyles = new Dictionary<string, TableHeaderStyle<TKey>>();
        MetaData?.ColumnsData.Columns.ForEach(column =>
        {
            var columnStyle = TableStyle.ColumnStyleGetter(column.Key);
            ColumnStyles.Add(column.Key, new TableHeaderStyle<TKey>(columnStyle, columnStyle.GetClass(TableStyle, column), columnStyle.GetStyle(), columnStyle.GetInnerClass(), columnStyle.GetInnerStyle()));
        });

        await InvokeAsync(StateHasChanged);
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        ObjectHelper.WhenNotNull(MetaData, meta =>
        {
            if (meta.PaginationData.PaginationEnabled)
            {
                ObjectHelper.WhenNotNull(TableComponent, t =>
                {
                    if (t.RowsPerPage != meta.PaginationData.PageSize)
                    {
                        t.SetRowsPerPage(meta.PaginationData.PageSize);
                    }
                });
            }
        });

        return base.OnAfterRenderAsync(firstRender);
    }

    public Task Refresh() => ObjectHelper.WhenNotNull(TableComponent, async t => await t.ReloadServerData());

    public List<TableRowItem<TKey>> GetData() =>
        TableComponent?.Context.Rows.Select(x => x.Key).ToList() ?? [];

    public async Task SetAdditionalFilter(string key, object? value)
    {
        var needRefresh = false;
        if (Params.ContainsKey(key))
        {
            if (ObjectHelper.IsNotNull(value))
            {
                Params[key] = value;
            }
            else
            {
                Params.Remove(key);
            }

            needRefresh = true;
        }
        else if (ObjectHelper.IsNotNull(value))
        {
            Params.Add(key, value);
            needRefresh = true;
        }

        if (needRefresh)
        {
            await Refresh();
        }
    }

    private TableFilter GetCurrentFilter()
    {
        return new TableFilter
        {
            TextFilter = MetaData?.FilterData.TextFilterEnabled ?? false
                ? string.IsNullOrEmpty(TextFilter)
                    ? null
                    : TextFilter
                : null
        };
    }

    private async Task<TableData<TableRowItem<TKey>>> TableReload(TableState state, CancellationToken cancellationToken = default)
    {
        var emptyData = new TableData<TableRowItem<TKey>>()
        {
            Items = new List<TableRowItem<TKey>>(),
            TotalItems = 0,
        };
        if (ObjectHelper.IsNotNull(ErrorResult))
        {
            return emptyData;
        }

        var data = await Service.GetData(new TableOptions
        {
            Filter = GetCurrentFilter(),
            Pagination = (MetaData?.PaginationData.PaginationEnabled ?? false)
                ? new TablePagination { Page = state.Page, Size = state.PageSize }
                : null,
            Ordering = Orders
        }, Params);

        if (ObjectHelper.IsNotNull(data.Error))
        {
            ErrorResult = data.Error;
            return emptyData;
        }

        if (ObjectHelper.IsNull(data.Result) || ObjectHelper.IsNull(MetaData))
        {
            return emptyData;
        }

        return new TableData<TableRowItem<TKey>>
        {
            Items = data.Result.Items.Select(x => new TableRowItem<TKey>(x, TableStyle, MetaData)).ToList(),
            TotalItems = data.Result.FilteredAll
        };
    }

    private void RowClickHandler(TableRowClickEventArgs<TableRowItem<TKey>> e)
    {
        if (ReadOnly || ObjectHelper.IsNull(e.Item) || e.Item.Disabled || e.Item.ClickDisabled)
        {
            return;
        }

        rowClickSubject.OnNext(new TableRowClickArgs<TableRowItem<TKey>>
        {
            Item = e.Item,
            MouseEventArgs = e.MouseEventArgs,
        });
    }

    private async Task ActionHandler(ColumnData col, TableRowItem<TKey> item)
    {
        if (ReadOnly || item.Disabled || item.ClickDisabled || item.Values[col.Key].ActionDisabled)
        {
            return;
        }

        await OnAction.InvokeAsync(KeyValuePair.Create(col.Key, item));
    }

    private Task HandleOrdering(ColumnData col)
    {
        if (!(MetaData?.OrderingData.OrderingEnabled ?? false) || !col.IsSortable)
        {
            return Task.CompletedTask;
        }

        var status = GetOrderingStatus(col);
        var increased = OrderDirectionIncrease(status);

        Orders = increased == OrderDirection.None
            ? []
            : [new() { Key = col.Key, Direction = increased }];

        return Refresh();
    }

    private Task TextFilterHandler(string text)
    {
        TextFilter = text;
        return Refresh();
    }

    private OrderDirection GetOrderingStatus(ColumnData col) =>
        Orders.FirstOrDefault(o => o.Key == col.Key)?.Direction ?? OrderDirection.None;

    private bool IsOrderedColumn(ColumnData col) => GetOrderingStatus(col) != OrderDirection.None;

    private static string GetOrderingIcon(OrderDirection direction)
    {
        return direction switch
        {
            OrderDirection.Ascend => Icons.Material.Filled.ArrowDropDown,
            OrderDirection.Descend => Icons.Material.Filled.ArrowDropUp,
            _ => Icons.Material.Filled.Info
        };
    }

    private static OrderDirection OrderDirectionIncrease(OrderDirection direction)
    {
        return direction switch
        {
            OrderDirection.None => OrderDirection.Ascend,
            OrderDirection.Ascend => OrderDirection.Descend,
            OrderDirection.Descend => OrderDirection.None,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private static string GetErrorMessage(ResourceMessage message, IStringLocalizer? localizer) =>
        ObjectHelper.IsNotNull(message.ResourceKey) && ObjectHelper.IsNotNull(localizer)
            ? localizer[message.ResourceKey]
            : message.Text;

    private static string GetTitle(TableMetaData meta, IStringLocalizer? localizer) =>
        ObjectHelper.IsNotNull(meta.ResourceKey) && ObjectHelper.IsNotNull(localizer)
            ? localizer[meta.ResourceKey]
            : meta.Title;

    private static string GetColumnTitle(ColumnData col, IStringLocalizer? localizer) =>
        ObjectHelper.IsNotNull(col.ResourceKey) && ObjectHelper.IsNotNull(localizer)
            ? localizer[col.ResourceKey]
            : col.Title;

    private static string GetValue(string value, IStringLocalizer? localizer) =>
        ObjectHelper.IsNotNull(localizer) ? localizer[value] : value;

    public void Dispose() => disposable?.Dispose();
}