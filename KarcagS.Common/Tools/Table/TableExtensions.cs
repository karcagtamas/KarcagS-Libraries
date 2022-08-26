using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.Common.Tools.Table;

public static class TableExtensions
{
    public static TableMetaData Convert<T, TKey>(this Configuration<T, TKey> configuration) where T : class, IIdentified<TKey>
    {
        return new TableMetaData
        {
            Key = configuration.Key,
            Title = configuration.Title,
            FilterData = configuration.Filter.Convert(),
            PaginationData = configuration.Pagination.Convert(),
            ColumnsData = configuration.Columns.Convert()
        };
    }

    public static FilterData Convert(this FilterConfiguration configuration)
    {
        return new FilterData
        {
            TextFilterEnabled = configuration.TextFilterEnabled
        };
    }

    public static PaginationData Convert(this PaginationConfiguration configuration)
    {
        return new PaginationData
        {
            PaginationEnabled = configuration.PaginationEnabled,
            PageSize = configuration.PageSize
        };
    }

    public static ColumnsData Convert<T, TKey>(this List<Column<T, TKey>> columns) where T : class, IIdentified<TKey>
    {
        return new ColumnsData
        {
            Columns = columns.Select(x => x.Convert()).ToList()
        };
    }

    public static ColumnData Convert<T, TKey>(this Column<T, TKey> column) where T : class, IIdentified<TKey>
    {
        return new ColumnData
        {
            Key = column.Key,
            Title = column.Title,
            Alignment = column.Alignment,
            Formatter = column.Formatter,
            Width = column.Width,
        };
    }
}
