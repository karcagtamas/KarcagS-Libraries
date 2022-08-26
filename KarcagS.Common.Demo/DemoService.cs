using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Table.Enums;

namespace KarcagS.Common.Demo;

public class DemoService : TableService<DemoEntry, string>, IDemoService
{
    private readonly DemoContext context;

    public DemoService(DemoContext context)
    {
        this.context = context;
        Initialize();
    }

    public override Configuration<DemoEntry, string> BuildConfiguration() =>
        Configuration<DemoEntry, string>
            .Build("demo-table")
            .SetTitle("Demo Table")
            .AddColumn(Column<DemoEntry, string>.Build("id").SetTitle("Id").AddValueGetter(x => x.Id).SetWidth(80).SetAlignment(Alignment.Center))
            .AddColumn(Column<DemoEntry, string>.Build("name").SetTitle("Name").AddValueGetter(x => x.Name))
            .AddColumn(Column<DemoEntry, string>.Build("age").SetTitle("Age").AddValueGetter(x => x.Age).SetFormatter(ColumnFormatter.Number).SetWidth(50).SetAlignment(Alignment.Right))
            .AddColumn(Column<DemoEntry, string>.Build("date").SetTitle("Date").AddValueGetter(x => x.Date).SetFormatter(ColumnFormatter.Date).SetWidth(200))
            .AddColumn(Column<DemoEntry, string>.Build("male").SetTitle("Male").AddValueGetter(x => x.Male).SetFormatter(ColumnFormatter.Logic, "Yes", "No").SetWidth(60).SetAlignment(Alignment.Center))
            .AddFilter(FilterConfiguration.Build().IsTextFilterEnabled(true))
            .AddPagination(PaginationConfiguration.Build().IsPaginationEnabled(true))
            .DisableClickOn(obj => obj.Age == 12)
            .IsReadOnly(true)
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
        .SetEFFilteredEntries("Name")
        .OrderBy(x => x.Name, OrderDirection.Descend)
        .ThenBy(x => x.Id)
        .ApplyOrdering();

    public override Table<DemoEntry, string> BuildTable() =>
        ListTableBuilder<DemoEntry, string>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
}

public interface IDemoService : ITableService<DemoEntry, string>
{

}
