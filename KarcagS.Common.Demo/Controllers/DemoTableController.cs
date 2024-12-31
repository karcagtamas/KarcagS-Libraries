using KarcagS.API.Table;
using KarcagS.Shared.Common;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.Common.Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DemoTableController(IDemoService demoService) : TableController<DemoEntry, string>
{
    protected override ITableService<DemoEntry, string> GetService() => demoService;
}

public class DemoDTO : IIdentified<string>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool Male { get; set; }
    public DateTime Date { get; set; }
}