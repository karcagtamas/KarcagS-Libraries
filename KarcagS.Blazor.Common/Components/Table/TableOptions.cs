using KarcagS.Http;

namespace KarcagS.Blazor.Common.Components.Table;

public class TableOptions
{
    public TableFilter Filter { get; set; } = new();
    public TablePagination? Pagination { get; set; }
    public List<Order> Ordering { get; set; } = [];
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

        if (options.Ordering.Count > 0)
        {
            queryParams.AddMultiple("ordering", options.Ordering);
        }

        return queryParams;
    }
}