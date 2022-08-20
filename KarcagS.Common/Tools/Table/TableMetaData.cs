using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table.Enums;

namespace KarcagS.Common.Tools.Table;

public class TableMetaData<T, TKey> where T : class, IIdentified<TKey>
{
    public string Key { get; set; }
    public string Title { get; set; }

    public FilterData FilterData { get; set; }
    public PaginationData PaginationData { get; set; }
    public ColumnsData<T, TKey> ColumnsData { get; set; }

    public TableMetaData(Configuration<T, TKey> configuration)
    {
        Key = configuration.Key;
        Title = configuration.Title;
        FilterData = new(configuration.Filter);
        PaginationData = new(configuration.Pagination);
        ColumnsData = new(configuration.Columns);
    }
}

public class FilterData
{
    public bool TextFilterEnabled { get; set; }

    public FilterData(FilterConfiguration configuration)
    {
        TextFilterEnabled = configuration.TextFilterEnabled;
    }
}

public class PaginationData
{
    public bool PaginationEnabled { get; set; }
    public int PageSize { get; set; }

    public PaginationData(PaginationConfiguration configuration)
    {
        PaginationEnabled = configuration.PaginationEnabled;
        PageSize = configuration.PageSize;
    }
}

public class ColumnsData<T, TKey> where T : class, IIdentified<TKey>
{
    public List<ColumnData> Columns { get; set; }
    public int ColumnNumber { get => Columns.Count; }

    public ColumnsData(List<Column<T, TKey>> columns)
    {
        Columns = columns.Select(x => new ColumnData(x)).ToList();
    }

    public class ColumnData
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public Alignment Alignment { get; set; }
        public ColumnFormatter Formatter { get; set; }
        public int? Width { get; set; }

        public ColumnData(Column<T, TKey> column)
        {
            Key = column.Key;
            Title = column.Title;
            Alignment = column.Alignment;
            Formatter = column.Formatter;
            Width = column.Width;
        }
    }
}
