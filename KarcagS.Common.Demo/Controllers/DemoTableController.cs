using KarcagS.Common.Tools.Table;
using KarcagS.Shared.Common;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.Common.Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DemoTableController : TableController<DemoDTO, string>
{
    private readonly IDemoService demoService;

    public DemoTableController(IDemoService demoService)
    {
        this.demoService = demoService;
    }

    public override ITableService<DemoDTO, string> GetService() => demoService;
}

public class DemoDTO : IIdentified<string>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
