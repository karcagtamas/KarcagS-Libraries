using KarcagS.Blazor.Common.Enums;
using KarcagS.Shared.Common;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table;

public partial class ListTable<T, TKey> : ComponentBase where T : class, IIdentified<TKey>
{

    [Parameter, EditorRequired]
    public TableDataSource<T, TKey> DataSource { get; set; } = default!;

    [Parameter, EditorRequired]
    public TableConfiguration<T, TKey> Config { get; set; } = TableConfiguration<T, TKey>.Build();

    [Parameter]
    public EventCallback<RowItem<T, TKey>> OnRowClick { get; set; }

    [Parameter]
    public string Class { get; set; } = string.Empty;

    private MudTable<RowItem<T, TKey>>? Table { get; set; }

    private string AppendedClass { get => $"w-100 flex-box h-100 {Class}"; }

    private bool Loading { get; set; } = false;
    private string TextFilter { get; set; } = string.Empty;

    protected override async void OnInitialized()
    {
        Loading = true;
        StateHasChanged();
        await DataSource.Init(this);
        Loading = false;
        StateHasChanged();

        base.OnInitialized();
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && Config.Pagination.PaginationEnabled)
        {
            ObjectHelper.WhenNotNull(Table, t => t.SetRowsPerPage(Config.Pagination.PageSize));
        }

        return base.OnAfterRenderAsync(firstRender);
    }

    public async Task Refresh(TableState state) => await DataSource.Refresh(state);

    public void ForceRefresh() => ObjectHelper.WhenNotNull(Table, async t => await t.ReloadServerData());

    public TableFilter GetCurrentFilter()
    {
        return new TableFilter
        {
            TextFilter = Config.Filter.TextFilterEnabled ? TextFilter : null
        };
    }

    private async Task<TableData<RowItem<T, TKey>>> TableReload(TableState state)
    {
        await Refresh(state);
        return new TableData<RowItem<T, TKey>> { Items = DataSource.data, TotalItems = DataSource.AllDataCount };
    }

    private string GetTDStyle(Alignment alignment)
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

    private async Task RowClickHandler(TableRowClickEventArgs<RowItem<T, TKey>> e)
    {
        if (e.Item.Disabled || e.Item.Hidden || Config.ClickDisableOn(e.Item.Data))
        {
            return;
        }

        await OnRowClick.InvokeAsync(e.Item);
    }

    private void TextFilterHandler(string text)
    {
        TextFilter = text;

        ObjectHelper.WhenNotNull(Table, async t => await t.ReloadServerData());
    }
}