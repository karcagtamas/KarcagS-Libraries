using KarcagS.Blazor.Common.Models.Interfaces;
using Microsoft.AspNetCore.Components;

namespace KarcagS.Blazor.Common.Components.Table;

public partial class ListTable<T> : ComponentBase where T : class, IIdentified<string>
{

    [Parameter, EditorRequired]
    public TableDataSource<T, string> DataSource { get; set; } = default!;

    [Parameter, EditorRequired]
    public TableConfiguration<T, string> Config { get; set; } = new();

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