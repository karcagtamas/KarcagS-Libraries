using KarcagS.Blazor.Common.Components.Table.Styles;

namespace KarcagS.Blazor.Common.Components.Table;

public record TableDataStyle<TKey>(CellStyle<TKey> CellStyle, string Class, string Style, string InnerClass, string InnerStyle);