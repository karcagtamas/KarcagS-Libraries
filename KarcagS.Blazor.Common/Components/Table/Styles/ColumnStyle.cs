using KarcagS.Shared.Enums;
using KarcagS.Shared.Table;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public record ColumnStyle<TKey>(int? Width, Alignment Alignment, Color TitleColor)
{
    public string GetStyle()
    {
        List<string> styles = [];

        if (ObjectHelper.IsNotNull(Width))
        {
            styles.Add($"max-width: {Width}px");
        }

        return string.Join("; ", styles);
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

        return string.Join(" ", classes);
    }

    public string GetInnerStyle()
    {
        var alignmentText = Alignment switch
        {
            Alignment.Left => "left",
            Alignment.Center => "center",
            Alignment.Right => "right",
            _ => "left"
        };

        List<string> styles =
        [
            "display: flex",
            "flex-direction: row",
            "align-items: center",
            $"justify-content: {alignmentText}"
        ];

        return string.Join("; ", styles);
    }

    public string GetInnerClass() => string.Empty;
}