using KarcagS.Blazor.Common.Http;

namespace KarcagS.Blazor.Common.Components.Table;

public class TableOptions
{
    public TableFilter Filter { get; set; } = new();
    public TablePagination? Pagination { get; set; }
}

public static class TableOptionsExtensions
{
    public static HttpQueryParameters AddTableParams(this HttpQueryParameters queryParams, TableOptions options)
    {
        queryParams.Add("textFilter", options.Filter.TextFilter);

        ObjectHelper.WhenNotNull(options.Pagination, pagination =>
        {
            queryParams.Add("page", pagination.Page);
            queryParams.Add("size", pagination.Size);
        });

        return queryParams;
    }
}