using KarcagS.Shared.Enums;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public record ColumnStyle(int? Width, Alignment Alignment, Color TitleColor);