using KarcagS.API.Table;
using KarcagS.API.Table.Configurations;
using KarcagS.API.Table.ListTable;
using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;

namespace KarcagS.Common.Demo;

public class TestTableService : TableService<TestEntity, string>, ITestTableService
{
    public override async Task<Table<TestEntity, string>> BuildTableAsync()
    {
        return ListTableBuilder<TestEntity, string>.Construct()
            .AddDataSource(await BuildDataSourceAsync())
            .AddConfiguration(await BuildConfigurationAsync())
            .Build();
    }

    public override Task<DataSource<TestEntity, string>> BuildDataSourceAsync()
    {
        var dataSource = (DataSource<TestEntity, string>)ListTableDataSource<TestEntity, string>.Build(_ =>
            {
                return Task.FromResult(new List<TestEntity>
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
                }.AsQueryable());
            })
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
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}