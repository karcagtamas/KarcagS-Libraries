using KarcagS.Blazor.Common.Services;
using KarcagS.Client.Common.Http;
using KarcagS.Client.Common.Models;

namespace KarcagS.Blazor.Common.Demo.Services;

public class DemoTableService : TableService<string>, IDemoTableService
{
    public DemoTableService(IHttpService http) : base(http)
    {
    }

    public override string GetBaseUrl() => $"{ApplicationSettings.BaseApiUrl}/DemoTable";
}
