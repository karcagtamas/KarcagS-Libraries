using System.Security.Claims;

namespace KarcagS.API.Shared.Services;

public interface IUtilsService
{
    ClaimsPrincipal? GetUserPrincipal();
    ClaimsPrincipal GetRequiredUserPrincipal();
    string GetCurrentUserEmail();
    string GetCurrentUserName();
    string InjectString(string baseText, params string[] args);
    string ErrorsToString<T>(IEnumerable<T> errors, Func<T, string> toString);
}