using KarcagS.API.Data.Entities;

namespace KarcagS.API.Shared.Services;

public interface IUserProvider<TUserKey>
{
    Task<TUserKey?> GetCurrentUserId();
    Task<TUserKey> GetRequiredCurrentUserId();
    Task<T?> GetCurrentUser<T>() where T : Entity<TUserKey>;
    Task<T> GetRequiredCurrentUser<T>() where T : Entity<TUserKey>;

    Task WithCurrentUserId(Action<TUserKey?> action);
    Task WithRequiredCurrentUserId(Action<TUserKey> action);
    Task<T> WithCurrentUserId<T>(Func<TUserKey?, T> func);
    Task<T> WithRequiredCurrentUserId<T>(Func<TUserKey, T> func);
}