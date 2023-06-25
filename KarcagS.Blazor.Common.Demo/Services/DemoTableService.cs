using KarcagS.Blazor.Common.Services;
using KarcagS.Client.Common.Models;
using KarcagS.Http;

namespace KarcagS.Blazor.Common.Demo.Services;

public class DemoTableService : TableService<string>, IDemoTableService
{
    public DemoTableService(IHttpService http) : base(http)
    {
    }

    public override string GetBaseUrl() => $"{ApplicationSettings.BaseApiUrl}/DemoTable";
}
