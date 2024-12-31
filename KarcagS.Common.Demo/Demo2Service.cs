using KarcagS.API.Table;
using KarcagS.API.Table.Configurations;
using KarcagS.API.Table.ListTable;
using KarcagS.Shared.Table;
using KarcagS.Shared.Table.Enums;

namespace KarcagS.Common.Demo;

public class Demo2Service(DemoContext context) : TableService<DemoEntry, string>, IDemo2Service
{
    public override Configuration<DemoEntry, string> BuildConfiguration()
    {
        return Configuration<DemoEntry, string>
            .Build("demo-table")
            .SetTitle("Demo Table")
            .AddColumn(Column<DemoEntry, string>.Build("id").SetTitle("Id").AddValueGetter(x => x.Id))
            .AddColumn(Column<DemoEntry, string>.Build("name").SetTitle("Name").AddValueGetter(x => x.Name).MarkAsSortable("Name"))
            .AddColumn(Column<DemoEntry, string>.Build("name-s").SetTitle("Name").AddValueGetter(x => x.Name).MarkAsSortable("Name"))
            .AddColumn(Column<DemoEntry, string>.Build("age").SetTitle("Age").AddValueGetter(x => x.Age).SetFormatter(ColumnFormatter.Number))
            .AddColumn(Column<DemoEntry, string>.Build("date").SetTitle("Date").AddValueGetter(x => x.Date).SetFormatter(ColumnFormatter.Date).MarkAsSortable("Date"))
            .AddColumn(Column<DemoEntry, string>.Build("gender").SetTitle("Gender").AddValueGetter(x => x.Gender.Name).MarkAsSortable("Gender.Name"))
            .AddColumn(Column<DemoEntry, string>.Build("other-gender").SetTitle("Other Gender").AddValueGetter(x => x.OtherGender?.Name ?? "N/A"))
            .AddColumn(Column<DemoEntry, string>.Build("open").SetTitle("Open").MarkAsAction())
            .AddFilter(FilterConfiguration.Build().IsTextFilterEnabled(true))
            .AddPagination(PaginationConfiguration.Build().IsPaginationEnabled(true))
            .AddOrdering(OrderingConfiguration.Build().IsEnabled())
            .DisableClickOn(obj => obj.Age == 12)
            .ActionsDisabledOn((_, _) => false)
            .AddTagProvider(async (obj, col) =>
            {
                var entry = await col.ValueGetter(obj);

                if (entry is bool lo)
                {
                    return lo ? "TRUE_VALUE" : "FALSE_VALUE";
                }

                return "";
            });
    }

    public override TableBuilder<DemoEntry, string> Builder() => ListTableBuilder<DemoEntry, string>.Construct();

    public override Task<DataSource<DemoEntry, string>> BuildDataSourceAsync()
    {
        var dataSource = (DataSource<DemoEntry, string>)ListTableDataSource<DemoEntry, string>.Build(_ => Task.FromResult(context.Set<DemoEntry>().AsQueryable()));

        return Task.FromResult(dataSource);
    }

    public override Task<bool> AuthorizeAsync(QueryModel query) => Task.FromResult(true);
}

public interface IDemo2Service : ITableService<DemoEntry, string>
{
}