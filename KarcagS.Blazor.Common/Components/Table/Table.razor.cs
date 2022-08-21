using KarcagS.Blazor.Common.Components.ListTable;
using KarcagS.Blazor.Common.Services;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;
using KarcagS.Shared.Table.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static KarcagS.Shared.Table.TableResult;

namespace KarcagS.Blazor.Common.Components.Table;

public partial class Table : ComponentBase
{
    [Parameter, EditorRequired]
    public ITableService Service { get; set; } = default!;

    [Parameter]
    public StyleConfiguration Style { get; set; } = StyleConfiguration.Build();

    [Parameter]
    public EventCallback<ResultRowItem> OnRowClick { get; set; }

    [Parameter]
    public string Class { get; set; } = string.Empty;

    private MudTable<ResultRowItem>? TableComponent { get; set; }

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

    private async Task<TableData<ResultRowItem>> TableReload(TableState state)
    {
        var data = await Service.GetData(new TableOptions
        {
            Filter = GetCurrentFilter(),
            Pagination = (MetaData?.PaginationData.PaginationEnabled ?? false) ? new TablePagination { Page = state.Page, Size = state.PageSize } : null
        });

        if (ObjectHelper.IsNull(data))
        {
            return new TableData<ResultRowItem>
            {
                Items = new List<ResultRowItem>(),
                TotalItems = 0,
            };
        }

        return new TableData<ResultRowItem>
        {
            Items = data.Items.Select(x => new ResultRowItem(x)).ToList(),
            TotalItems = data.FilteredAll
        };
    }

    private async Task RowClickHandler(TableRowClickEventArgs<ResultRowItem> e)
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
