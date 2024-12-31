using KarcagS.Blazor.Common.Services;
using KarcagS.Client.Common.Models;
using KarcagS.Http;

namespace KarcagS.Blazor.Common.Demo.Services;

public class DemoTableService(IHttpService http) : TableService<string>(http), IDemoTableService
{
    public override string GetBaseUrl() => $"{ApplicationSettings.BaseApiUrl}/DemoTable";
}
