using KarcagS.Shared.Enums;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public class ColumnStyleBuilder<TKey>
{
    private int? width;
    private Alignment textAlignment = Alignment.Left;
    private Color titleColor = Color.Primary;

    private ColumnStyleBuilder()
    {
    }

    public static ColumnStyleBuilder<TKey> Init() => new();

    public static ColumnStyle<TKey> Default() => Init().Build();

    public ColumnStyle<TKey> Build() => new(width, textAlignment, titleColor);

    public ColumnStyleBuilder<TKey> Width(int value)
    {
        width = value;
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
}