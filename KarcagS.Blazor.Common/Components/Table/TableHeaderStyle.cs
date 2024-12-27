using KarcagS.Blazor.Common.Components.Table.Styles;

namespace KarcagS.Blazor.Common.Components.Table;

public record TableHeaderStyle<TKey>(ColumnStyle<TKey> ColumnStyle, string Class, string Style, string InnerClass, string InnerStyle);