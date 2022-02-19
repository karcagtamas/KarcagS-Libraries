using KarcagS.Blazor.Common.Enums;
using KarcagS.Blazor.Common.Models.Interfaces;
using Microsoft.AspNetCore.Components;

namespace KarcagS.Blazor.Common.Components.Table;

public partial class ListTable<T> where T : IIdentified<string>
{

    [Parameter]
    public List<T> DataSource { get; set; } = new();

    [Parameter]
    public List<TableColumn<T>> Columns { get; set; } = new();
}

public class TableColumn<T>
{
    public string Title { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public Alignment Alignment { get; set; } = Alignment.Left;
    public Func<T, string> ValueGetter { get; set; } = (o) => "";
}