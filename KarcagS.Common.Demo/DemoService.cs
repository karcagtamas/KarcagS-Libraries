using KarcagS.Common.Demo.Controllers;
using KarcagS.Common.Tools.Table;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Common.Tools.Table.ListTable;

namespace KarcagS.Common.Demo;

public class DemoService : TableService<DemoDTO, string>, IDemoService
{
    public override Configuration<DemoDTO, string> BuildConfiguration() =>
        Configuration<DemoDTO, string>
            .Build("demo-table")
            .AddTitle("Demo Table")
            .AddColumn(Column<DemoDTO, string>.Build("id").SetTitle("Id").AddValueGetter(x => x.Id))
            .AddColumn(Column<DemoDTO, string>.Build("name").SetTitle("Name").AddValueGetter(x => x.Name))
            .AddColumn(Column<DemoDTO, string>.Build("age").SetTitle("Age").AddValueGetter(x => x.Age).SetFormatter(ColumnFormatter.Number))
            .AddColumn(Column<DemoDTO, string>.Build("date").SetTitle("Date").AddValueGetter(x => x.Date).SetFormatter(ColumnFormatter.Date))
            .AddColumn(Column<DemoDTO, string>.Build("male").SetTitle("Male").AddValueGetter(x => x.Male).SetFormatter(ColumnFormatter.Logic, "Yes", "No"));

    public override DataSource<DemoDTO, string> BuildDataSource() => ListTableDataSource<DemoDTO, string>.Build(() =>
        new List<DemoDTO>()
        {
            new()
            {
                Name = "Tamas",
                Age = 12,
                Date = DateTime.Now,
                Male = true
            },
            new()
            {
                Name = "Tobias",
                Age = 22,
                Date = new DateTime(1999, 7, 20),
                Male = true
            },
            new()
            {
                Name = "Juliat",
                Age = 44,
                Date = new DateTime(2000, 10, 10, 1, 1, 1)
            }
        }.AsQueryable())
        .SetListFilteredColumns("name");

    public override Table<DemoDTO, string> BuildTable() =>
        ListTableBuilder<DemoDTO, string>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
}

public interface IDemoService : ITableService<DemoDTO, string>
{

}
