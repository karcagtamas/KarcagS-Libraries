using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public class CellStyleBuilder
{
    private Color textColor = Color.Default;
    private string icon = Icons.Material.Filled.Info;
    private Color iconColor = Color.Primary;

    private CellStyleBuilder()
    {
    }

    public static CellStyleBuilder Init() => new();

    public static CellStyle Default() => Init().Build();

    public CellStyle Build() => new(textColor, icon, iconColor);

    public CellStyleBuilder TextColor(Color value)
    {
        textColor = value;
        return this;
    }

    public CellStyleBuilder Icon(string value)
    {
        icon = value;
        return this;
    }

    public CellStyleBuilder IconColor(Color value)
    {
        iconColor = value;
        return this;
    }
}