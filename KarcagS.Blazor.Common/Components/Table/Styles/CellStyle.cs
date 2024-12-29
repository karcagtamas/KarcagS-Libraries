using KarcagS.Shared.Enums;
using KarcagS.Shared.Table;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public class CellStyle<TKey>(Color color, string icon, Color iconColor) : Style
{
    public Color Color => color;
    public string Icon => icon;
    public Color IconColor => iconColor;

    public string GetStyle(ColumnStyle<TKey> style)
    {
        List<string> styles = [];

        if (!style.ForceWidth || ObjectHelper.IsNull(style.Width))
        {
            ObjectHelper.WhenNotNull(style.MaxWidth, w => styles.Add(ToProperty("max-width", w)));
            ObjectHelper.WhenNotNull(style.MinWidth, w => styles.Add(ToProperty("min-width", w)));
        }

        ObjectHelper.WhenNotNull(style.Width, w =>
        {
            styles.Add(ToProperty("width", w));

            if (style.ForceWidth)
            {
                styles.Add(ToProperty("max-width", w));
                styles.Add(ToProperty("min-width", w));
            }
        });

        return ConcatStyles(styles);
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

    public string GetInnerStyle(ColumnStyle<TKey> style)
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
            ToProperty("display", "flex"),
            ToProperty("flex-direction", "row"),
            ToProperty("align-items", "center"),
            ToProperty("justify-content", alignmentText),
        ];

        return ConcatStyles(styles);
    }

    public string GetInnerClass() => string.Empty;
}