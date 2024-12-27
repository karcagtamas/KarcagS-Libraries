using KarcagS.Shared.Enums;
using KarcagS.Shared.Table;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public record CellStyle<TKey>(Color Color, string Icon, Color IconColor)
{
    public string GetStyle(ColumnStyle<TKey> style)
    {
        var alignmentText = style.Alignment switch
        {
            Alignment.Left => "left",
            Alignment.Center => "center",
            Alignment.Right => "right",
            _ => "left"
        };

        List<string> styles =
        [
            $"justify-content: {alignmentText}"
        ];

        if (ObjectHelper.IsNotNull(style.Width))
        {
            styles.Add($"max-width: {style.Width}px");
        }

        return string.Join("; ", styles);
    }

    public string GetClass(StyleConfiguration<TKey> styleConfiguration, ColumnData columnData, TableRowItem<TKey> item)
    {
        List<string> classes =
        [
            "lib-table-cell",
            $"lib-table-cell-{columnData.Key}"
        ];

        if (item.Disabled || item.ClickDisabled)
        {
            classes.Add("lib-table-cell-disabled");
        }

        if (styleConfiguration.EllipsisTextOverflow)
        {
            classes.Add("ellipsis");
        }

        return string.Join(" ", classes);
    }
}