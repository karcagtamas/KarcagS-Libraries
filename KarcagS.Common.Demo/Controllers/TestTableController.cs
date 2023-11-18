using KarcagS.API.Table;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.Common.Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestTableController : TableController<TestEntity, string>
{
    private readonly ITestTableService testTableService;

    public TestTableController(ITestTableService testTableService)
    {
        this.testTableService = testTableService;
    }

    protected override ITableService<TestEntity, string> GetService() => testTableService;
}