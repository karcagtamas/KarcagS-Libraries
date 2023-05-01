using KarcagS.Common.Tools.Entities;
using System.Security.Claims;

namespace KarcagS.Common.Tools.Services;

public interface IUtilsService<TUserKey>
{
    ClaimsPrincipal? GetUserPrincipal();
    ClaimsPrincipal GetRequiredUserPrincipal();
    T GetCurrentUser<T>() where T : class, IEntity<TUserKey>;
    TUserKey? GetCurrentUserId();
    TUserKey GetRequiredCurrentUserId();
    string GetCurrentUserEmail();
    string GetCurrentUserName();
    string InjectString(string baseText, params string[] args);
    string ErrorsToString<T>(IEnumerable<T> errors, Func<T, string> toString);
    void WithCurrentUserId(Action<TUserKey?> action);
    T WithCurrentUserId<T>(Func<TUserKey?, T> func);
    T WithRequiredCurrentUserId<T>(Func<TUserKey, T> func);
}