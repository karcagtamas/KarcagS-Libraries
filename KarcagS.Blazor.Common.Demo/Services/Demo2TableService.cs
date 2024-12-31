using KarcagS.Blazor.Common.Services;
using KarcagS.Client.Common.Models;
using KarcagS.Http;

namespace KarcagS.Blazor.Common.Demo.Services;

public class Demo2TableService(IHttpService http) : TableService<string>(http), IDemo2TableService
{
    public override string GetBaseUrl() => $"{ApplicationSettings.BaseApiUrl}/Demo2Table";
}
