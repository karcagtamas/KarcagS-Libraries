using KarcagS.API.Table;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.Common.Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestTableController(ITestTableService testTableService) : TableController<TestEntity, string>
{
    protected override ITableService<TestEntity, string> GetService() => testTableService;
}