using KarcagS.Shared.Table;

namespace KarcagS.Blazor.Common.Components.Table.Styles;

public class StyleConfiguration<TKey>
{
    public bool Dense { get; set; } = true;
    public bool FixedHeader { get; set; } = true;
    public bool Hover { get; set; } = true;
    public bool Striped { get; set; } = true;
    public int Elevation { get; set; } = 2;
    public bool Bordered { get; set; } = true;
    public bool EllipsisTextOverflow { get; set; } = true;

    public Func<string, ColumnStyle<TKey>> ColumnStyleGetter = _ => ColumnStyleBuilder<TKey>.Default();
    public Func<string, ItemValue, CellStyle<TKey>> CellStyleGetter = (_, _) => CellStyleBuilder<TKey>.Default();

    private StyleConfiguration()
    {
    }

    public static StyleConfiguration<TKey> Build() => new();

    public StyleConfiguration<TKey> IsDense(bool value)
    {
        Dense = value;

        return this;
    }

    public StyleConfiguration<TKey> IsFixedHeader(bool value)
    {
        FixedHeader = value;

        return this;
    }

    public StyleConfiguration<TKey> IsHover(bool value)
    {
        Hover = value;

        return this;
    }

    public StyleConfiguration<TKey> IsStriped(bool value)
    {
        Striped = value;

        return this;
    }

    public StyleConfiguration<TKey> SetElevation(int value)
    {
        Elevation = value;

        return this;
    }

    public StyleConfiguration<TKey> IsBordered(bool value)
    {
        Bordered = value;

        return this;
    }

    public StyleConfiguration<TKey> IsEllipsisTextOverflow(bool value)
    {
        EllipsisTextOverflow = value;

        return this;
    }

    public StyleConfiguration<TKey> AddColumnStyleGetter(Func<string, ColumnStyle<TKey>> getter)
    {
        ColumnStyleGetter = getter;

        return this;
    }

    public StyleConfiguration<TKey> AddCellStyleGetter(Func<string, ItemValue, CellStyle<TKey>> getter)
    {
        CellStyleGetter = getter;

        return this;
    }
}