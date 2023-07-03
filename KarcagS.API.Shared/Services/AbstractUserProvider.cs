using KarcagS.API.Data.Entities;

namespace KarcagS.API.Shared.Services;

public abstract class AbstractUserProvider<TUserKey> : IUserProvider<TUserKey>
{
    public virtual Task<TUserKey?> GetCurrentUserId() => throw new NotImplementedException();
    public virtual Task<TUserKey> GetRequiredCurrentUserId() => throw new NotImplementedException();
    public virtual Task<T?> GetCurrentUser<T>() where T : Entity<TUserKey> => throw new NotImplementedException();
    public virtual Task<T> GetRequiredCurrentUser<T>() where T : Entity<TUserKey> => throw new NotImplementedException();


    public async Task WithCurrentUserId(Action<TUserKey?> action) => action(await GetCurrentUserId());

    public async Task WithRequiredCurrentUserId(Action<TUserKey> action) => action(await GetRequiredCurrentUserId());

    public async Task<T> WithCurrentUserId<T>(Func<TUserKey?, T> func) => func(await GetCurrentUserId());

    public async Task<T> WithRequiredCurrentUserId<T>(Func<TUserKey, T> func) => func(await GetRequiredCurrentUserId());
}