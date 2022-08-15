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

    public async Task Refresh()
    {
        await DataSource.Refresh();
        await InvokeAsync(StateHasChanged);
    }

    public TableFilter GetCurrentFilter()
    {
        return new TableFilter
        {
            TextFilter = Config.Filter.TextFilterEnabled ? TextFilter : null
        };
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

    private async void TextFilterHandler(string text)
    {
        TextFilter = text;
        await Refresh();
    }
}