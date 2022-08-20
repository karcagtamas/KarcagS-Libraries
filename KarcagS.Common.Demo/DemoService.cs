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
            .AddColumn(Column<DemoDTO, string>.Build("id").SetTitle("Id").AddValueGetter(x => x.Id));

    public override DataSource<DemoDTO, string> BuildDataSource() => ListTableDataSource<DemoDTO, string>.Build(() => new List<DemoDTO>().AsQueryable());

    public override Table<DemoDTO, string> BuildTable() =>
        ListTableBuilder<DemoDTO, string>.Construct()
            .AddDataSource(BuildDataSource())
            .AddConfiguration(BuildConfiguration())
            .Build();
}

public interface IDemoService : ITableService<DemoDTO, string>
{

}
