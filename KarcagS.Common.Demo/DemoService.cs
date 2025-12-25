using KarcagS.API.Table;
using KarcagS.API.Table.AutoListTable;
using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Table;
using KarcagS.Shared.Table.Enums;

namespace KarcagS.Common.Demo;

public class DemoService(DemoContext context) : TableService<DemoEntry, string>, IDemoService
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

    public override TableBuilder<DemoEntry, string> Builder() => AutoListTableBuilder<DemoEntry, string>.Construct();

    public override Task<DataSource<DemoEntry, string>> BuildDataSourceAsync()
    {
        var dataSource = (DataSource<DemoEntry, string>)AutoListTableDataSource<DemoEntry, string>.Build(_ => Task.FromResult(context.Set<DemoEntry>().AsQueryable()))
            .SetEFFilteredEntries("Name", "Gender.Name", "OtherGender.Name")
            .OrderBy(x => x.Name, OrderDirection.Descend)
            .ThenBy(x => x.Id)
            .ApplyOrdering();

        return Task.FromResult(dataSource);
    }

    public override Task<bool> AuthorizeAsync(QueryModel query) => Task.FromResult(true);
}

public interface IDemoService : ITableService<DemoEntry, string>;