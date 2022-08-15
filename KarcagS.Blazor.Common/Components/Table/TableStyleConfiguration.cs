namespace KarcagS.Blazor.Common.Components.Table;

public class TableStyleConfiguration
{
    public bool Dense { get; set; } = true;
    public bool FixedHeader { get; set; } = true;
    public bool Hover { get; set; } = true;
    public bool Striped { get; set; } = true;
    public int Elevation { get; set; } = 2;
    public bool Bordered { get; set; } = true;

    private TableStyleConfiguration()
    {

    }

    public static TableStyleConfiguration Build() => new();

    public TableStyleConfiguration IsDense(bool value)
    {
        Dense = value;

        return this;
    }

    public TableStyleConfiguration IsFixedHeader(bool value)
    {
        FixedHeader = value;

        return this;
    }

    public TableStyleConfiguration IsHover(bool value)
    {
        Hover = value;

        return this;
    }

    public TableStyleConfiguration IsStriped(bool value)
    {
        Striped = value;

        return this;
    }

    public TableStyleConfiguration SetElevation(int value)
    {
        Elevation = value;

        return this;
    }

    public TableStyleConfiguration IsBordered(bool value)
    {
        Bordered = value;

        return this;
    }
}