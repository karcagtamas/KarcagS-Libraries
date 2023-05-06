using KarcagS.API.Repository;
using KarcagS.API.Shared.Services;

namespace KarcagS.Common.Demo;

public class UserProvider : IUserProvider<string>
{
    private readonly IUtilsService<string> utilsService;

    public UserProvider(IUtilsService<string> utilsService)
    {
        this.utilsService = utilsService;
    }

    public string? GetCurrentUserId() => utilsService.GetCurrentUserId();

    public string GetRequiredCurrentUserId() => utilsService.GetRequiredCurrentUserId();
}