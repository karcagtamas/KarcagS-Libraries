using KarcagS.Shared.Enums;
using KarcagS.Shared.Table;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public class ColumnStyle<TKey>(Style.NumericStyleValue? width, Style.NumericStyleValue? maxWidth, Style.NumericStyleValue? minWidth, Alignment alignment, Color titleColor, bool forceWidth) : Style
{
    public NumericStyleValue? Width => width;
    public NumericStyleValue? MaxWidth => maxWidth;
    public NumericStyleValue? MinWidth => minWidth;
    public Alignment Alignment => alignment;
    public Color TitleColor => titleColor;
    public bool ForceWidth => forceWidth;

    public string GetStyle()
    {
        List<string> styles = [];

        if (!forceWidth || ObjectHelper.IsNull(width))
        {
            ObjectHelper.WhenNotNull(maxWidth, w => styles.Add(ToProperty("max-width", w)));
            ObjectHelper.WhenNotNull(minWidth, w => styles.Add(ToProperty("min-width", w)));
        }

        ObjectHelper.WhenNotNull(width, w =>
        {
            styles.Add(ToProperty("width", w));

            if (forceWidth)
            {
                styles.Add(ToProperty("max-width", w));
                styles.Add(ToProperty("min-width", w));
            }
        });

        return ConcatStyles(styles);
    }

    public string GetClass(StyleConfiguration<TKey> styleConfiguration, ColumnData columnData)
    {
        List<string> classes =
        [
            "lib-table-header-cell",
            $"lib-table-header-cell-{columnData.Key}"
        ];

        if (columnData.IsSortable)
        {
            classes.Add("lib-table-header-cell-sortable");
        }

        if (styleConfiguration.EllipsisTextOverflow)
        {
            classes.Add("ellipsis");
        }

        return ConcatClasses(classes);
    }

    public string GetInnerStyle()
    {
        var alignmentText = alignment switch
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