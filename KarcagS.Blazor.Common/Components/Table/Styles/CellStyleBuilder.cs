using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public class CellStyleBuilder<TKey>
{
    private Color textColor = Color.Default;
    private string icon = Icons.Material.Filled.Info;
    private Color iconColor = Color.Primary;

    private CellStyleBuilder()
    {
    }

    public static CellStyleBuilder<TKey> Init() => new();

    public static CellStyle<TKey> Default() => Init().Build();

    public CellStyle<TKey> Build() => new(textColor, icon, iconColor);

    public CellStyleBuilder<TKey> TextColor(Color value)
    {
        textColor = value;
        return this;
    }

    public CellStyleBuilder<TKey> Icon(string value)
    {
        icon = value;
        return this;
    }

    public CellStyleBuilder<TKey> IconColor(Color value)
    {
        iconColor = value;
        return this;
    }
}