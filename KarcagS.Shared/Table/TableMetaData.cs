using KarcagS.Shared.Enums;
using KarcagS.Shared.Table.Enums;

namespace KarcagS.Shared.Table;

public class TableMetaData
{
    public string Key { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? ResourceKey { get; set; }

    public FilterData FilterData { get; set; } = default!;
    public OrderingData OrderingData { get; set; } = default!;
    public PaginationData PaginationData { get; set; } = default!;
    public ColumnsData ColumnsData { get; set; } = default!;
}

public class FilterData
{
    public bool TextFilterEnabled { get; set; }
}

public class OrderingData
{
    public bool OrderingEnabled { get; set; }
}

public class PaginationData
{
    public bool PaginationEnabled { get; set; }
    public int PageSize { get; set; }
}

public class ColumnsData
{
    public List<ColumnData> Columns { get; set; } = [];
    public int ColumnNumber => Columns.Count;
}

public class ColumnData
{
    public string Key { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? ResourceKey { get; set; }
    public ColumnFormatter Formatter { get; set; }
    public bool IsAction { get; set; }
    public bool IsSortable { get; set; }
}