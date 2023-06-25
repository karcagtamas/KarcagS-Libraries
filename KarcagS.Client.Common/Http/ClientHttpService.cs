using KarcagS.Client.Common.Services.Interfaces;
using KarcagS.Http;
using KarcagS.Shared.Http;

namespace KarcagS.Client.Common.Http;

public class ClientHttpService : HttpService
{
    private readonly IHelperService helperService;

    public ClientHttpService(HttpClient httpClient, HttpConfiguration configuration, ITokenHandler tokenHandler, IHelperService helperService) : base(httpClient, configuration, tokenHandler)
    {
        this.helperService = helperService;
    }

    protected override void AddErrorToaster(string caption, HttpErrorResult? errorResult) => helperService.AddHttpErrorToaster(caption, errorResult);

    protected override void AddSuccessToaster(string caption) => helperService.AddHttpSuccessToaster(caption);
}