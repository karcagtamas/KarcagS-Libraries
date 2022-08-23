using KarcagS.Blazor.Common.Services;
using KarcagS.Shared.Table;
using KarcagS.Shared.Table.Enums;
using Microsoft.AspNetCore.Components;
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

    private MudTable<ResultRowItem<TKey>>? TableComponent { get; set; }

    private string AppendedClass { get => $"w-100 flex-box h-100 {Class}"; }

    private bool Loading { get; set; } = false;
    private string? TextFilter { get; set; }

    private TableMetaData? MetaData { get; set; }

    protected override async void OnInitialized()
    {
        await LoadMetaData();

        base.OnInitialized();
    }

    private async Task LoadMetaData()
    {
        Loading = true;
        StateHasChanged();

        MetaData = await Service.GetMetaData();

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
        var data = await Service.GetData(new TableOptions
        {
            Filter = GetCurrentFilter(),
            Pagination = (MetaData?.PaginationData.PaginationEnabled ?? false) ? new TablePagination { Page = state.Page, Size = state.PageSize } : null
        });

        if (ObjectHelper.IsNull(data))
        {
            return new TableData<ResultRowItem<TKey>>
            {
                Items = new List<ResultRowItem<TKey>>(),
                TotalItems = 0,
            };
        }

        return new TableData<ResultRowItem<TKey>>
        {
            Items = data.Items.Select(x => new ResultRowItem<TKey>(x)).ToList(),
            TotalItems = data.FilteredAll
        };
    }

    private async Task RowClickHandler(TableRowClickEventArgs<ResultRowItem<TKey>> e)
    {
        if (e.Item.Disabled || e.Item.Hidden || e.Item.ClickDisabled)
        {
            return;
        }

        await OnRowClick.InvokeAsync(e.Item);
    }

    private void TextFilterHandler(string text)
    {
        TextFilter = text;
        // TODO: RESET pagination
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
}
