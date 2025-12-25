using KarcagS.API.Shared.Configurations;
using KarcagS.API.Shared.Exceptions;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace KarcagS.API.Shared.Services;

public class HttpUserProvider<TUserKey>(IHttpContextAccessor contextAccessor, IOptions<UtilsSettings> utilsOptions) : AbstractUserProvider<TUserKey>
{
    private readonly UtilsSettings settings = utilsOptions.Value;

    public override Task<TUserKey?> GetCurrentUserId()
    {
        var claim = GetClaimByName(settings.UserIdClaimName);

        return !string.IsNullOrEmpty(claim)
            ? Task.FromResult((TUserKey?)(object?)claim)
            : Task.FromResult(default(TUserKey?));
    }

    public override async Task<TUserKey> GetRequiredCurrentUserId() => ObjectHelper.OrElseThrow(await GetCurrentUserId(), () => new ArgumentException("Current user is required"));

    public override Task<T?> GetCurrentUser<T>() where T : class => Task.FromResult(default(T?));

    public override async Task<T> GetRequiredCurrentUser<T>()
    {
        var userId = GetRequiredCurrentUserId();

        return ObjectHelper.OrElseThrow(await GetCurrentUser<T>(), () => new ServerException($"User not found with this id: {userId}", "Server.Message.UserNotFound"));
    }

    private string GetClaimByName(string name)
    {
        if (contextAccessor.HttpContext?.User is null)
        {
            return string.Empty;
        }

        return contextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == name)?.Value ?? string.Empty;
    }
}