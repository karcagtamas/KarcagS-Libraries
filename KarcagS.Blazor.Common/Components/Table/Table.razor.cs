using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Http;
using KarcagS.Shared.Table;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table;

public partial class Table<TKey> : ComponentBase
{
    [Parameter, EditorRequired]
    public ITableService<TKey> Service { get; set; } = default!;

    [Parameter]
    public StyleConfiguration Style { get; set; } = StyleConfiguration.Build();

    [Parameter]
    public EventCallback<ResultRowItem<TKey>> OnRowClick { get; set; }

    [Parameter]
    public string Class { get; set; } = string.Empty;

    [Parameter]
    public Dictionary<string, object> ExtraParams { get; set; } = new();

    [Parameter]
    public bool ReadOnly { get; set; } = false;

    [Parameter]
    public IStringLocalizer? Localizer { get; set; }

    [Inject]
    private ILocalizationService LocalizationService { get; set; } = default!;

    private MudTable<ResultRowItem<TKey>>? TableComponent { get; set; }

    private string AppendedClass { get => $"w-100 flex-box h-100 {Class}"; }

    private bool Loading { get; set; } = false;
    private string? TextFilter { get; set; }

    private TableMetaData? MetaData { get; set; }
    private HttpErrorResult? ErrorResult { get; set; }

    protected override async void OnInitialized()
    {
        await LoadMetaData();

        base.OnInitialized();
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
        StateHasChanged();
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

    public List<ResultRowItem<TKey>> GetData() => TableComponent?.Context.Rows.Select(x => x.Value).ToList() ?? new List<ResultRowItem<TKey>>();

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
            Pagination = (MetaData?.PaginationData.PaginationEnabled ?? false) ? new TablePagination { Page = state.Page, Size = state.PageSize } : null
        }, ExtraParams);

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

    private void TextFilterHandler(string text)
    {
        TextFilter = text;
        ObjectHelper.WhenNotNull(TableComponent, async t =>
        {
            await t.ReloadServerData();
        });
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
        return $"text-align: {alignmentText}";
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

    private static string GetValue(string value, IStringLocalizer? localizer)
    {
        if (ObjectHelper.IsNotNull(localizer))
        {
            return localizer[value];
        }

        return value;
    }
}
