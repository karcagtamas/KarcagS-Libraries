using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Services;

namespace KarcagS.Blazor.Common.Demo.Services;

public class DemoTableService : TableService, IDemoTableService
{
    public DemoTableService(IHttpService http) : base(http)
    {
    }

    public override string GetBaseUrl() => $"{ApplicationSettings.BaseApiUrl}/DemoTable";
}
