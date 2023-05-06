using System.Security.Claims;
using KarcagS.API.Data;
using KarcagS.API.Data.Entities;
using KarcagS.API.Shared.Configurations;
using KarcagS.API.Shared.Exceptions;
using KarcagS.API.Shared.Helpers;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KarcagS.API.Shared.Services;

public class UtilsService<TContext, TUserKey> : IUtilsService<TUserKey> where TContext : DbContext
{
    private readonly IHttpContextAccessor contextAccessor;
    private readonly TContext context;
    private readonly UtilsSettings settings;

    /// <summary>
    /// Utils Service constructor
    /// </summary>
    /// <param name="contextAccessor">Context Accessor</param>
    /// <param name="context">Context</param>
    /// <param name="utilsOptions">Utils Options</param>
    public UtilsService(IHttpContextAccessor contextAccessor, TContext context, IOptions<UtilsSettings> utilsOptions)
    {
        this.contextAccessor = contextAccessor;
        this.context = context;
        settings = utilsOptions.Value;
    }

    /// <summary>
    /// Get current user from the HTTP Context
    /// </summary>
    /// <returns>Current user</returns>
    public T GetCurrentUser<T>() where T : class, IEntity<TUserKey>
    {
        TUserKey? userId = GetCurrentUserId();

        ExceptionHelper.ThrowIfIsNull(userId, "User key not found", "Server.Message.UserKeyNotFound");

        return ObjectHelper.OrElseThrow(context.Set<T>().Find(userId), () => new ServerException($"User not found with this id: {userId}", "Server.Message.UserNotFound"));
    }

    /// <summary>
    /// Get current user's Id from the HTTP Context
    /// </summary>
    /// <returns>Current user's Id</returns>
    public TUserKey? GetCurrentUserId()
    {
        string claim = GetClaimByName(settings.UserIdClaimName);

        if (!string.IsNullOrEmpty(claim))
        {
            return (TUserKey)(object)claim;
        }

        return default;
    }

    /// <summary>
    /// Get current user's Id from the HTTP Context
    /// </summary>
    /// <returns>Current user's Id</returns>
    public TUserKey GetRequiredCurrentUserId() => ObjectHelper.OrElseThrow(GetCurrentUserId(), () => new ArgumentException("Current user is required"));

    /// <summary>
    /// Get current user's Email from the HTTP Context
    /// </summary>
    /// <returns>Current user's Email</returns>
    public string GetCurrentUserEmail() => GetClaimByName(settings.UserEmailClaimName);

    /// <summary>
    /// Get current user's Name from the HTTP Context
    /// </summary>
    /// <returns>Current user's Name</returns>
    public string GetCurrentUserName() => GetClaimByName(settings.UserNameClaimName);

    /// <summary>
    /// Identity errors to string.
    /// </summary>
    /// <param name="errors">Error list</param>
    /// <param name="toString">To string function</param>
    /// <returns>First error's description</returns>
    public string ErrorsToString<T>(IEnumerable<T> errors, Func<T, string> toString)
    {
        var list = errors.ToList();
        return list.Count > 0
            ? toString(list.First())
            : string.Empty;
    }

    public void WithCurrentUserId(Action<TUserKey?> action) => action(GetCurrentUserId());

    public void WithRequiredCurrentUserId(Action<TUserKey> action) => action(GetRequiredCurrentUserId());

    public T WithCurrentUserId<T>(Func<TUserKey?, T> func) => func(GetCurrentUserId());

    public T WithRequiredCurrentUserId<T>(Func<TUserKey, T> func) => func(GetRequiredCurrentUserId());

    /// <summary>
    /// Inject params into string.
    /// </summary>
    /// <param name="baseText">Base text with number placeholders.</param>
    /// <param name="args">Injectable params.</param>
    /// <returns>Base text with injected params.</returns>
    public string InjectString(string baseText, params string[] args)
    {
        string res = baseText;

        for (int i = 0; i < args.Length; i++)
        {
            var number = i;
            // Get placeholder from the current interaction
            string placeholder = "{i}".Replace('i', number.ToString()[0]);

            // Placeholder does not exist in the base text

            ExceptionHelper.Check(res.Contains(placeholder), () => new ArgumentException($"Placer holder is missing with number: {number}"));

            // Inject params instead of placeholder
            res = res.Replace(placeholder, $"{args[number]}");
        }

        return res;
    }

    private string GetClaimByName(string name)
    {
        if (contextAccessor.HttpContext?.User is null)
        {
            return string.Empty;
        }

        return contextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == name)?.Value ?? string.Empty;
    }

    public ClaimsPrincipal? GetUserPrincipal() => contextAccessor.HttpContext?.User;

    public ClaimsPrincipal GetRequiredUserPrincipal() => ObjectHelper.OrElseThrow(GetUserPrincipal(), () => new ArgumentException("User is required"));
}