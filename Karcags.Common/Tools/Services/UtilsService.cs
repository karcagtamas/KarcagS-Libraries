using System;
using System.Collections.Generic;
using System.Linq;
using Karcags.Common.Tools.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Karcags.Common.Tools.Services;

public class UtilsService<TContext> : IUtilsService where TContext : DbContext
{
    private readonly IHttpContextAccessor contextAccessor;
    private readonly TContext context;
    private readonly UtilsSettings settings;

    /// <summary>
    /// Utils Service constructor
    /// </summary>
    /// <param name="contextAccessor">Context Accessor</param>
    /// <param name="context">Context</param>
    public UtilsService(IHttpContextAccessor contextAccessor, TContext context, IOptions<UtilsSettings> utilsOptions)
    {
        this.contextAccessor = contextAccessor;
        this.context = context;
        this.settings = utilsOptions.Value;
    }

    /// <summary>
    /// Get current user from the HTTP Context
    /// </summary>
    /// <returns>Current user</returns>
    public T GetCurrentUser<T, TKey>() where T : class, IEntity<TKey>
    {
        TKey userId = this.GetCurrentUserId<TKey>();

        if (userId is null)
        {
            throw new UserKeyNotFoundException("User key not found");
        }

        var user = this.context.Set<T>().Find(userId);
        if (user == null)
        {
            throw new UserNotFoundException($"User not found with this id: {userId}");
        }

        return user;
    }

    /// <summary>
    /// Get current user's Id from the HTTP Context
    /// </summary>
    /// <returns>Current user's Id</returns>
    public TKey GetCurrentUserId<TKey>()
    {
        string claim = GetClaimByName(settings.UserIdClaimName);

        if (!string.IsNullOrEmpty(claim))
        {
            return (TKey)(object)claim;
        }

        return default;
    }

    /// <summary>
    /// Get current user's Email from the HTTP Context
    /// </summary>
    /// <returns>Current user's Email</returns>
    public string GetCurrentUserEmail()
    {
        return GetClaimByName(settings.UserEmailClaimName);
    }

    /// <summary>
    /// Get current user's Name from the HTTP Context
    /// </summary>
    /// <returns>Current user's Name</returns>
    public string GetCurrentUserName()
    {
        return GetClaimByName(settings.UserNameClaimName);
    }

    /// <summary>
    /// Identity errors to string.
    /// </summary>
    /// <param name="errors">Error list</param>
    /// <param name="toString">To string function</param>
    /// <returns>First error's description</returns>
    public string ErrorsToString<T>(IEnumerable<T> errors, Func<T, string> toString)
    {
        var list = errors.ToList();
        return toString(list.FirstOrDefault());
    }

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
            // Get placeholder from the current interaction
            string placeholder = "{i}".Replace('i', i.ToString()[0]);

            // Placeholder does not exist in the base text
            if (!res.Contains(placeholder))
            {
                throw new ArgumentException($"Placer holder is missing with number: {i}");
            }

            // Inject params instead of placeholder
            res = res.Replace(placeholder, $"{args[i]}");
        }

        return res;
    }

    private string GetClaimByName(string name)
    {
        if (this.contextAccessor.HttpContext.User is null)
        {
            return "";
        }

        return this.contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == name).Value;
    }

    public class UserKeyNotFoundException : Exception
    {
        public UserKeyNotFoundException(string msg) : base(msg)
        {

        }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string msg) : base(msg)
        {

        }
    }
}