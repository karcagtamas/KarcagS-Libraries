using KarcagS.Shared.Enums;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public class ColumnStyleBuilder<TKey>
{
    private Style.NumericStyleValue? maxWidth;
    private Style.NumericStyleValue? minWidth;
    private Style.NumericStyleValue? width;
    private Alignment textAlignment = Alignment.Left;
    private Color titleColor = Color.Primary;
    private bool forceWidth;

    private ColumnStyleBuilder()
    {
    }

    public static ColumnStyleBuilder<TKey> Init() => new();

    public static ColumnStyle<TKey> Default() => Init().Build();

    public ColumnStyle<TKey> Build() => new(width, maxWidth, minWidth, textAlignment, titleColor, forceWidth);

    public ColumnStyleBuilder<TKey> Width(double value, Style.StyleValueUnit unit, bool force = false)
    {
        width = new Style.NumericStyleValue(value, unit);
        forceWidth = force;
        return this;
    }

    public ColumnStyleBuilder<TKey> Width(double value, bool force = false) => Width(value, Style.StyleValueUnit.Pixel, force);

    public ColumnStyleBuilder<TKey> MaxWidth(double value, Style.StyleValueUnit unit = Style.StyleValueUnit.Pixel)
    {
        maxWidth = new Style.NumericStyleValue(value, unit);
        return this;
    }

    public ColumnStyleBuilder<TKey> MinWidth(double value, Style.StyleValueUnit unit = Style.StyleValueUnit.Pixel)
    {
        minWidth = new Style.NumericStyleValue(value, unit);
        return this;
    }

    public ColumnStyleBuilder<TKey> TextAlignment(Alignment value)
    {
        textAlignment = value;
        return this;
    }

    public ColumnStyleBuilder<TKey> TitleColor(Color value)
    {
        titleColor = value;
        return this;
    }

    public ColumnStyleBuilder<TKey> ForceWidth(bool value)
    {
        forceWidth = value;
        return this;
    }
}