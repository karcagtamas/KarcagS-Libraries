using KarcagS.Shared.Table;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public class StyleConfiguration
{
    public bool Dense { get; set; } = true;
    public bool FixedHeader { get; set; } = true;
    public bool Hover { get; set; } = true;
    public bool Striped { get; set; } = true;
    public int Elevation { get; set; } = 2;
    public bool Bordered { get; set; } = true;

    public Func<string, ColumnStyle> ColumnStyleGetter = _ => ColumnStyleBuilder.Default();
    public Func<string, ItemValue, CellStyle> CellStyleGetter = (_, _) => CellStyleBuilder.Default();

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

    public StyleConfiguration AddColumnStyleGetter(Func<string, ColumnStyle> getter)
    {
        ColumnStyleGetter = getter;

        return this;
    }

    public StyleConfiguration AddCellStyleGetter(Func<string, ItemValue, CellStyle> getter)
    {
        CellStyleGetter = getter;

        return this;
    }
}