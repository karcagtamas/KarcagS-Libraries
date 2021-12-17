using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Karcags.Common.Tools.Services
{
    public class UtilsService<TContext> : IUtilsService where TContext : DbContext
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly TContext _context;

        /// <summary>
        /// Utils Service constructor
        /// </summary>
        /// <param name="contextAccessor">Context Accessor</param>
        /// <param name="context">Context</param>
        public UtilsService(IHttpContextAccessor contextAccessor, TContext context)
        {
            this._contextAccessor = contextAccessor;
            this._context = context;
        }
        
        /// <summary>
        /// Get current user from the HTTP Context
        /// </summary>
        /// <returns>Current user</returns>
        public T GetCurrentUser<T>() where T : class, IEntity
        {
            string userId = this.GetCurrentUserId();
            var user = this._context.Set<T>().Find(userId);
            if (user == null)
            {
                throw new Exception("Invalid user Id");
            }

            return user;
        }

        /// <summary>
        /// Get current user's Id from the HTTP Context
        /// </summary>
        /// <returns>Current user's Id</returns>
        public string GetCurrentUserId()
        {
            string userId = this._contextAccessor.HttpContext.User.Claims.First(c => c.Type == "UserId").Value;
            return userId;
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
    }
}