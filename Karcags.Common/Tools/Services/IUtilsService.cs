using System;
using System.Collections.Generic;

namespace Karcags.Common.Tools.Services
{
    public interface IUtilsService
    {
        T GetCurrentUser<T>() where T : class, IEntity;
        string GetCurrentUserId();
        string InjectString(string baseText, params string[] args);
        string ErrorsToString<T>(IEnumerable<T> errors, Func<T, string> toString);
    }
}