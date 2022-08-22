using MudBlazor;
using static KarcagS.Shared.Table.TableResult.ResultItem;

namespace KarcagS.Blazor.Common.Components.Table;

public class StyleConfiguration
{
    public bool Dense { get; set; } = true;
    public bool FixedHeader { get; set; } = true;
    public bool Hover { get; set; } = true;
    public bool Striped { get; set; } = true;
    public int Elevation { get; set; } = 2;
    public bool Bordered { get; set; } = true;

    public Func<ItemValue, Color> ColorGetter = (item) => Color.Default;
    public Func<string, Color> TitleColorGetter = (colKey) => Color.Primary;

    private StyleConfiguration()
    {

    }

    public static StyleConfiguration Build() => new();

    public StyleConfiguration IsDense(bool value)
    {
        Dense = value;

        return this;
    }

    public StyleConfiguration IsFixedHeader(bool value)
    {
        FixedHeader = value;

        return this;
    }

    public StyleConfiguration IsHover(bool value)
    {
        Hover = value;

        return this;
    }

    public StyleConfiguration IsStriped(bool value)
    {
        Striped = value;

        return this;
    }

    public StyleConfiguration SetElevation(int value)
    {
        Elevation = value;

        return this;
    }

    public StyleConfiguration IsBordered(bool value)
    {
        Bordered = value;

        return this;
    }

    public StyleConfiguration AddColorGetter(Func<ItemValue, Color> getter)
    {
        ColorGetter = getter;

        return this;
    }

    public StyleConfiguration AddTitleColorGetter(Func<string, Color> getter)
    {
        TitleColorGetter = getter;

        return this;
    }
}