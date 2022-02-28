using KarcagS.Common.Tools.Entities;

namespace KarcagS.Common.Tools.Services;

public interface IUtilsService
{
    T GetCurrentUser<T, TKey>() where T : class, IEntity<TKey>;
    TKey? GetCurrentUserId<TKey>();
    TKey GetRequiredCurrentUserId<TKey>();
    string GetCurrentUserEmail();
    string GetCurrentUserName();
    string InjectString(string baseText, params string[] args);
    string ErrorsToString<T>(IEnumerable<T> errors, Func<T, string> toString);
}
