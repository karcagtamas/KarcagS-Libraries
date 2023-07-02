using KarcagS.API.Table;
using KarcagS.API.Table.Configuration;
using KarcagS.API.Table.Configurations;
using KarcagS.API.Table.ListTable;
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
    }

    public override Task<Configuration<DemoEntry, string>> BuildConfigurationAsync()
    {
        var config = Configuration<DemoEntry, string>
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
            .AddTagProvider(async (obj, col) =>
            {
                var entry = await col.ValueGetter(obj);

                if (entry is bool lo)
                {
                    return lo ? "TRUE_VALUE" : "FALSE_VALUE";
                }

                return "";
            });

        return Task.FromResult(config);
    }

    public override Task<DataSource<DemoEntry, string>> BuildDataSourceAsync()
    {
        var dataSource = (DataSource<DemoEntry, string>)ListTableDataSource<DemoEntry, string>.Build(_ => Task.FromResult(context.Set<DemoEntry>().AsQueryable()))
            .SetEFFilteredEntries("Name", "Gender.Name", "OtherGender.Name")
            .OrderBy(x => x.Name, OrderDirection.Descend)
            .ThenBy(x => x.Id)
            .ApplyOrdering();

        return Task.FromResult(dataSource);
    }

    public override async Task<Table<DemoEntry, string>> BuildTableAsync() =>
        ListTableBuilder<DemoEntry, string>.Construct()
            .AddDataSource(await BuildDataSourceAsync())
            .AddConfiguration(await BuildConfigurationAsync())
            .Build();

    public override Task<bool> AuthorizeAsync(QueryModel query) => Task.FromResult(true);
}

public interface IDemoService : ITableService<DemoEntry, string>
{
}