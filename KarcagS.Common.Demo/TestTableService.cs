using KarcagS.API.Table;
using KarcagS.API.Table.AutoListTable;
using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;

namespace KarcagS.Common.Demo;

public class TestTableService : TableService<TestEntity, string>, ITestTableService
{
    public override TableBuilder<TestEntity, string> Builder() => AutoListTableBuilder<TestEntity, string>.Construct();

    public override Task<DataSource<TestEntity, string>> BuildDataSourceAsync()
    {
        var dataSource = (DataSource<TestEntity, string>)AutoListTableDataSource<TestEntity, string>.Build(_ => Task.FromResult(new List<TestEntity>
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Alma"
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Korte"
                }
            }.AsQueryable()))
            .SetTextFilteredColumns("Name")
            .OrderBy(x => x.Name, OrderDirection.Descend)
            .ThenBy(x => x.Id)
            .ApplyOrdering();

        return Task.FromResult(dataSource);
    }

    public override Task<Configuration<TestEntity, string>> BuildConfigurationAsync()
    {
        var config = Configuration<TestEntity, string>
            .Build("test-table")
            .SetTitle("Test Table")
            .AddColumn(Column<TestEntity, string>.Build("id").SetTitle("Id").AddValueGetter(x => x.Id))
            .AddColumn(Column<TestEntity, string>.Build("name").SetTitle("Name").AddValueGetter(x => x.Name).MarkAsSortable("Name"))
            .AddFilter(FilterConfiguration.Build().IsTextFilterEnabled(true))
            .AddPagination(PaginationConfiguration.Build().IsPaginationEnabled(true))
            .AddOrdering(OrderingConfiguration.Build().IsEnabled());

        return Task.FromResult(config);
    }
}

public interface ITestTableService : ITableService<TestEntity, string>
{
}

public record TestEntity : IIdentified<string>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}