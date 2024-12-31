using KarcagS.API.Table;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.Common.Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Demo2TableController(IDemo2Service demoService) : TableController<DemoEntry, string>
{
    protected override ITableService<DemoEntry, string> GetService() => demoService;
}