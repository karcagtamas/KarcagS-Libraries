using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Table;
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
            .AddColumn(Column<DemoEntry, string>.Build("name").SetTitle("Name").AddValueGetter(x => x.Name).MarkAsSortable("Name"))
            .AddColumn(Column<DemoEntry, string>.Build("name-s").SetTitle("Name").AddValueGetter(x => x.Name).MarkAsSortable("Name"))
            .AddColumn(Column<DemoEntry, string>.Build("age").SetTitle("Age").AddValueGetter(x => x.Age).SetFormatter(ColumnFormatter.Number).SetWidth(50).SetAlignment(Alignment.Right))
            .AddColumn(Column<DemoEntry, string>.Build("date").SetTitle("Date").AddValueGetter(x => x.Date).SetFormatter(ColumnFormatter.Date).SetWidth(200).MarkAsSortable("Date"))
            .AddColumn(Column<DemoEntry, string>.Build("gender").SetTitle("Gender").AddValueGetter(x => x.Gender.Name).SetWidth(120).MarkAsSortable("Gender.Name"))
            .AddColumn(Column<DemoEntry, string>.Build("other-gender").SetTitle("Other Gender").AddValueGetter(x => x.OtherGender?.Name ?? "N/A").SetWidth(120))
            .AddColumn(Column<DemoEntry, string>.Build("open").SetTitle("Open").MarkAsAction())
            .AddFilter(FilterConfiguration.Build().IsTextFilterEnabled(true))
            .AddPagination(PaginationConfiguration.Build().IsPaginationEnabled(true))
            .AddOrdering(OrderingConfiguration.Build().IsEnabled())
            .DisableClickOn(obj => obj.Age == 12)
            .ActionsDisabledOn((obj, col) => false)
            .AddTagProvider((obj, col) =>
            {
                var entry = col.ValueGetter(obj);

                if (entry is bool lo)
                {
                    return lo ? "TRUE_VALUE" : "FALSE_VALUE";
                }

                return "";
            });

    public override DataSource<DemoEntry, string> BuildDataSource() => ListTableDataSource<DemoEntry, string>.Build(_ =>
            context.Set<DemoEntry>().AsQueryable())
        .SetEFFilteredEntries("Name", "Gender.Name", "OtherGender.Name")
        .OrderBy(x => x.Name, OrderDirection.Descend)
        .ThenBy(x => x.Id)
        .ApplyOrdering();

    public override Table<DemoEntry, string> BuildTable() =>
        ListTableBuilder<DemoEntry, string>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();

    public override Task<bool> Authorize(QueryModel query)
    {
        return Task.FromResult(true);
    }
}

public interface IDemoService : ITableService<DemoEntry, string>
{
}