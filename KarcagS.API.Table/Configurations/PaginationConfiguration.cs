namespace KarcagS.API.Table.Configuration;

public class PaginationConfiguration
{
    public bool PaginationEnabled { get; set; }
    public int PageSize { get; set; } = 50;

    private PaginationConfiguration()
    {

    }

    public static PaginationConfiguration Build() => new();

    public PaginationConfiguration IsPaginationEnabled(bool value)
    {
        PaginationEnabled = value;

        return this;
    }

    public PaginationConfiguration SetPageSize(int value)
    {
        PageSize = value;

        return this;
    }
}
