using System.Diagnostics.CodeAnalysis;

namespace KarcagS.Shared.Helpers;

public static class ObjectHelper
{
    public static bool IsNull<T>([NotNullWhen(false)] T? obj) => obj is null;

    public static bool IsNotNull<T>([NotNullWhen(true)] T? obj) => obj is not null;

    public static bool IsEmpty<T>(IEnumerable<T> e) => !e.Any();

    public static bool IsNotEmpty<T>(IEnumerable<T> e) => e.Any();

    public static T OrElse<T>(T? obj, T orElse) => IsNull(obj) ? orElse : obj;

    public static T OrElseThrow<T, Ex>(T? obj, Ex e) where Ex : Exception, new()
    {
        if (IsNotNull(obj))
        {
            return obj;
        }

        throw e;
    }

    public static T OrElseThrow<T, Ex>(T? obj, Func<Ex> func) where Ex : Exception, new()
    {
        if (IsNotNull(obj))
        {
            return obj;
        }

        throw func();
    }

    public static V? MapOrDefault<T, V>(T? obj, Func<T, V> func)
    {
        return IsNotNull(obj) ? func(obj) : default;
    }

    public static void WhenNotNull<T>(T? obj, Action<T> action)
    {
        if (IsNotNull(obj))
        {
            action(obj);
        }
    }
}
