using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Client.Common.Services.Interfaces;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Http;
using KarcagS.Shared.Table;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table;

public partial class Table<TKey> : ComponentBase
{
    [Parameter] public RenderFragment<Action<string, object?>>? FilterRow { get; set; }

    [Parameter, EditorRequired] public ITableService<TKey> Service { get; set; } = default!;

    [Parameter] public StyleConfiguration Style { get; set; } = StyleConfiguration.Build();

    [Parameter] public EventCallback<ResultRowItem<TKey>> OnRowClick { get; set; }

    [Parameter] public string Class { get; set; } = string.Empty;

    [Parameter] public Dictionary<string, object> InitialParams { get; set; } = new();

    [Parameter] public bool ReadOnly { get; set; }

    [Parameter] public IStringLocalizer? Localizer { get; set; }

    [Parameter] public EventCallback<KeyValuePair<string, ResultRowItem<TKey>>> OnAction { get; set; }

    [Inject] private ILocalizationService LocalizationService { get; set; } = default!;

    private MudTable<ResultRowItem<TKey>>? TableComponent { get; set; }

    private string AppendedClass => $"w-100 flex-box h-100 {Class}";

    private bool Loading { get; set; } = false;
    private string? TextFilter { get; set; }
    private Dictionary<string, object> Params { get; set; } = new();
    private List<Order> Orders { get; set; } = new();

    private TableMetaData? MetaData { get; set; }
    private HttpErrorResult? ErrorResult { get; set; }

    protected override async void OnInitialized()
    {
        foreach (var p in InitialParams)
        {
            Params.Add(p.Key, p.Value);
        }

        await LoadMetaData();

        await base.OnInitializedAsync();
    }

    private async Task LoadMetaData()
    {
        Loading = true;
        StateHasChanged();

        var meta = await Service.GetMetaData();

        if (ObjectHelper.IsNotNull(meta.Error))
        {
            ErrorResult = meta.Error;
        }

        MetaData = meta.Result;

        Loading = false;
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

    public void Refresh() => ObjectHelper.WhenNotNull(TableComponent, async t => await t.ReloadServerData());

    public List<ResultRowItem<TKey>> GetData() => TableComponent?.Context.Rows.Select(x => x.Key).ToList() ?? new List<ResultRowItem<TKey>>();

    public void SetAdditionalFilter(string key, object? value)
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
            Refresh();
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

    private async Task<TableData<ResultRowItem<TKey>>> TableReload(TableState state)
    {
        if (ObjectHelper.IsNotNull(ErrorResult))
        {
            return new TableData<ResultRowItem<TKey>>
            {
                Items = new List<ResultRowItem<TKey>>(),
                TotalItems = 0,
            };
        }

        var data = await Service.GetData(new TableOptions
        {
            Filter = GetCurrentFilter(),
            Pagination = (MetaData?.PaginationData.PaginationEnabled ?? false) ? new TablePagination { Page = state.Page, Size = state.PageSize } : null,
            Ordering = Orders
        }, Params);

        if (ObjectHelper.IsNotNull(data.Error))
        {
            ErrorResult = data.Error;
            return new TableData<ResultRowItem<TKey>>
            {
                Items = new List<ResultRowItem<TKey>>(),
                TotalItems = 0,
            };
        }

        if (ObjectHelper.IsNull(data.Result))
        {
            return new TableData<ResultRowItem<TKey>>
            {
                Items = new List<ResultRowItem<TKey>>(),
                TotalItems = 0,
            };
        }

        return new TableData<ResultRowItem<TKey>>
        {
            Items = data.Result.Items.Select(x => new ResultRowItem<TKey>(x)).ToList(),
            TotalItems = data.Result.FilteredAll
        };
    }

    private async Task RowClickHandler(TableRowClickEventArgs<ResultRowItem<TKey>> e)
    {
        if (ReadOnly || e.Item.Disabled || e.Item.ClickDisabled)
        {
            return;
        }

        await OnRowClick.InvokeAsync(e.Item);
    }

    private async Task ActionHandler(ColumnData col, ResultRowItem<TKey> item)
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
            ? new List<Order>()
            : new List<Order>
            {
                new() { Key = col.Key, Direction = increased }
            };

        ObjectHelper.WhenNotNull(TableComponent, async t => { await t.ReloadServerData(); });

        return Task.CompletedTask;
    }

    private void TextFilterHandler(string text)
    {
        TextFilter = text;
        ObjectHelper.WhenNotNull(TableComponent, async t => { await t.ReloadServerData(); });
    }

    private OrderDirection GetOrderingStatus(ColumnData col) => Orders.FirstOrDefault(o => o.Key == col.Key)?.Direction ?? OrderDirection.None;

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

    private static string GetTDStyle(Alignment alignment)
    {
        var alignmentText = alignment switch
        {
            Alignment.Left => "left",
            Alignment.Center => "center",
            Alignment.Right => "right",
            _ => ""
        };
        return $"text-align: {alignmentText}";
    }

    private static string GetTHStyle(Alignment alignment)
    {
        var alignmentText = alignment switch
        {
            Alignment.Left => "left",
            Alignment.Center => "center",
            Alignment.Right => "right",
            _ => ""
        };
        return $"display: flex; flex-direction: row; align-items: center; justify-content: {alignmentText};";
    }

    private static string GetErrorMessage(ResourceMessage message, IStringLocalizer? localizer)
    {
        if (ObjectHelper.IsNotNull(message.ResourceKey) && ObjectHelper.IsNotNull(localizer))
        {
            return localizer[message.ResourceKey];
        }

        return message.Text;
    }

    private static string GetTitle(TableMetaData meta, IStringLocalizer? localizer)
    {
        if (ObjectHelper.IsNotNull(meta.ResourceKey) && ObjectHelper.IsNotNull(localizer))
        {
            return localizer[meta.ResourceKey];
        }

        return meta.Title;
    }

    private static string GetColumnTitle(ColumnData col, IStringLocalizer? localizer)
    {
        if (ObjectHelper.IsNotNull(col.ResourceKey) && ObjectHelper.IsNotNull(localizer))
        {
            return localizer[col.ResourceKey];
        }

        return col.Title;
    }

    private static string GetValue(string value, IStringLocalizer? localizer) => ObjectHelper.IsNotNull(localizer) ? localizer[value] : value;
}