namespace KarcagS.Blazor.Common.Components.Table;

public class TablePaginationConfiguration
{
    public bool PaginationEnabled { get; set; } = false;
    public int PageSize { get; set; } = 50;

    private TablePaginationConfiguration()
    {

    }

    public static TablePaginationConfiguration Build() => new();

    public TablePaginationConfiguration IsPaginationEnabled(bool value)
    {
        PaginationEnabled = value;

        return this;
    }

    public TablePaginationConfiguration SetPageSize(int value)
    {
        PageSize = value;

        return this;
    }
}
