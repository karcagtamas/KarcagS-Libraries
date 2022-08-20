namespace KarcagS.Common.Tools.Table;

public class TableOptions
{
    public TableFilter Filter { get; set; } = new();
    public TablePagination? Pagination { get; set; }
}

public class TableFilter
{
    public string? TextFilter { get; set; }
}

public class TablePagination
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 50;
}
