using KarcagS.Shared.Enums;
using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public class ColumnStyleBuilder
{
    private int? width;
    private Alignment textAlignment = Alignment.Left;
    private Color titleColor = Color.Primary;

    private ColumnStyleBuilder()
    {
    }

    public static ColumnStyleBuilder Init() => new();

    public static ColumnStyle Default() => Init().Build();

    public ColumnStyle Build() => new(width, textAlignment, titleColor);

    public ColumnStyleBuilder Width(int value)
    {
        width = value;
        return this;
    }

    public ColumnStyleBuilder TextAlignment(Alignment value)
    {
        textAlignment = value;
        return this;
    }

    public ColumnStyleBuilder TitleColor(Color value)
    {
        titleColor = value;
        return this;
    }
}