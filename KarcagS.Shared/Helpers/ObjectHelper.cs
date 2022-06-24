namespace KarcagS.Shared.Helpers;

public static class ObjectHelper
{
    public static bool IsNull<T>(T? obj) => obj is null;

    public static bool IsNotNull<T>(T? obj) => obj is not null;

    public static T OrElse<T>(T? obj, T orElse) => obj is null ? orElse : obj;

    public static T OrElseThrow<T, Ex>(T? obj, Ex e) where Ex : Exception, new()
    {
        if (obj is not null)
        {
            return obj;
        }

        throw e;
    }

    public static T OrElseThrow<T, Ex>(T? obj, Func<Ex> func) where Ex : Exception, new()
    {
        if (obj is not null)
        {
            return obj;
        }

        throw func();
    }

    public static V? MapOrDefault<T, V>(T? obj, Func<T, V> func)
    {
        if (obj is not null)
        {
            return func(obj);
        }

        return default;
    }

    public static void WhenNotNull<T>(T? obj, Action<T> action)
    {
        if (obj is not null)
        {
            action(obj);
        }
    }
}
