using KarcagS.Client.Common.Services.Interfaces;
using KarcagS.Http;
using KarcagS.Shared.Http;

namespace KarcagS.Client.Common.Http;

public class ClientHttpService(HttpClient httpClient, HttpConfiguration configuration, ITokenHandler tokenHandler, HttpRefreshService refreshService, IHelperService helperService)
    : HttpService(httpClient, configuration, tokenHandler, refreshService)
{
    protected override void AddErrorToaster(string caption, HttpErrorResult? errorResult) => helperService.AddHttpErrorToaster(caption, errorResult);

    protected override void AddSuccessToaster(string caption) => helperService.AddHttpSuccessToaster(caption);
}