using Microsoft.AspNetCore.Components.Authorization;

namespace KarcagS.Blazor.Common.Services.Interfaces;

public interface IAuthHelper
{
    Task<bool> IsInRole(Task<AuthenticationState> stateTask, params string[] roles);
    bool IsInRole(AuthenticationState state, params string[] roles);
}