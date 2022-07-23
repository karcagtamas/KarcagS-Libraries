using KarcagS.Shared.Common;
using Microsoft.AspNetCore.Components;

namespace KarcagS.Blazor.Common.Components.Table;

public partial class ListTable<T, TKey> : ComponentBase where T : class, IIdentified<TKey>
{

    [Parameter, EditorRequired]
    public TableDataSource<T, TKey> DataSource { get; set; } = default!;

    [Parameter, EditorRequired]
    public TableConfiguration<T, TKey> Config { get; set; } = TableConfiguration<T, TKey>.Build();

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
}