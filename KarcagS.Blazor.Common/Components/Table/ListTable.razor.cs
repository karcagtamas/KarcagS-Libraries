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

    private bool Loading { get; set; } = false;

    protected override async void OnInitialized()
    {
        Loading = true;
        StateHasChanged();
        await DataSource.Init();
        Loading = false;
        StateHasChanged();

        base.OnInitialized();
    }

    public async Task Refresh()
    {
        await DataSource.Refresh();
        await InvokeAsync(StateHasChanged);
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
}