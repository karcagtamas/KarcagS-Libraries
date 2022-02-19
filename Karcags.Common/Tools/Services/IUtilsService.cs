using System;
using System.Collections.Generic;
using Karcags.Common.Tools.Entities;

namespace Karcags.Common.Tools.Services;

public interface IUtilsService
{
    T GetCurrentUser<T, TKey>() where T : class, IEntity<TKey>;
    TKey? GetCurrentUserId<TKey>();
    string GetCurrentUserEmail();
    string GetCurrentUserName();
    string InjectString(string baseText, params string[] args);
    string ErrorsToString<T>(IEnumerable<T> errors, Func<T, string> toString);
}
