using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Table.Enums;

namespace KarcagS.Common.Demo;

public class DemoService : TableService<DemoEntry, string>, IDemoService
{
    private readonly DemoContext context;

    public DemoService(DemoContext context)
    {
        this.context = context;
        Initalize();
    }

    public override Configuration<DemoEntry, string> BuildConfiguration() =>
        Configuration<DemoEntry, string>
            .Build("demo-table")
            .SetTitle("Demo Table")
            .AddColumn(Column<DemoEntry, string>.Build("id").SetTitle("Id").AddValueGetter(x => x.Id))
            .AddColumn(Column<DemoEntry, string>.Build("name").SetTitle("Name").AddValueGetter(x => x.Name))
            .AddColumn(Column<DemoEntry, string>.Build("age").SetTitle("Age").AddValueGetter(x => x.Age).SetFormatter(ColumnFormatter.Number))
            .AddColumn(Column<DemoEntry, string>.Build("date").SetTitle("Date").AddValueGetter(x => x.Date).SetFormatter(ColumnFormatter.Date))
            .AddColumn(Column<DemoEntry, string>.Build("male").SetTitle("Male").AddValueGetter(x => x.Male).SetFormatter(ColumnFormatter.Logic, "Yes", "No"))
            .AddFilter(FilterConfiguration.Build().IsTextFilterEnabled(true))
            .AddPagination(PaginationConfiguration.Build().IsPaginationEnabled(true))
            .AddTagProvider((obj, col) => 
            {
                var entry = col.ValueGetter(obj);

                if (entry is bool lo)
                {
                    return lo ? "TRUE_VALUE" : "FALSE_VALUE";
                }

                return "";
            });

    public override DataSource<DemoEntry, string> BuildDataSource() => ListTableDataSource<DemoEntry, string>.Build((query) =>
        context.Set<DemoEntry>().AsQueryable())
        .SetEFFilteredEntries("Name");

    public override Table<DemoEntry, string> BuildTable() =>
        ListTableBuilder<DemoEntry, string>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
}

public interface IDemoService : ITableService<DemoEntry, string>
{

}
