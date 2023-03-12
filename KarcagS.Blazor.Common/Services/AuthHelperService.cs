using KarcagS.Blazor.Common.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace KarcagS.Blazor.Common.Services;

public class AuthHelperService : IAuthHelperService
{
    public async Task<bool> IsInRole(Task<AuthenticationState> stateTask, params string[] roles) => IsInRole(await stateTask, roles);

    public bool IsInRole(AuthenticationState state, params string[] roles) => roles.ToList().Any(r => state.User.IsInRole(r));
}