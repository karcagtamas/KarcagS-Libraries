using KarcagS.API.Shared.Exceptions;
using KarcagS.Shared.Helpers;

namespace KarcagS.API.Shared.Helpers;

public static class ExceptionHelper
{
    public static void ThrowIfIsNull<T>(T? obj, string msg, string? resourceKey = null)
    {
        if (ObjectHelper.IsNull(obj))
        {
            throw new ServerException(msg, resourceKey);
        }
    }

    public static void ThrowIfIsNull<T, Ex>(T? obj, Ex e) where Ex : Exception, new()
    {
        if (ObjectHelper.IsNull(obj))
        {
            throw e;
        }
    }

    public static void ThrowIfIsNull<T, Ex>(T? obj, Func<Ex> func) where Ex : Exception, new()
    {
        if (ObjectHelper.IsNull(obj))
        {
            throw func();
        }
    }

    public static void ThrowIfIsNull<T, Ex>(T? obj, string msg) where Ex : Exception, new()
    {
        if (ObjectHelper.IsNull(obj))
        {
            var ex = Activator.CreateInstance(typeof(Ex), msg);

            if (ex is not null)
            {
                throw (Ex)ex;
            }

            throw new Exception(msg);
        }
    }

    public static void Throw(bool expression, string msg, string? resourceKey = null)
    {
        if (expression)
        {
            throw new ServerException(msg, resourceKey);
        }
    }

    public static void Throw<Ex>(bool expression, Ex e) where Ex : Exception, new()
    {
        if (expression)
        {
            throw e;
        }
    }

    public static void Throw<Ex>(bool expression, Func<Ex> func) where Ex : Exception, new()
    {
        if (expression)
        {
            throw func();
        }
    }

    public static void Throw<Ex>(bool expression, string msg) where Ex : Exception, new()
    {
        if (expression)
        {
            var ex = Activator.CreateInstance(typeof(Ex), msg);

            if (ex is not null)
            {
                throw (Ex)ex;
            }

            throw new Exception(msg);
        }
    }

    public static void Check(bool expression, string msg, string? resourceKey = null)
    {
        if (!expression)
        {
            throw new ServerException(msg, resourceKey);
        }
    }

    public static void Check<Ex>(bool expression, Ex e) where Ex : Exception, new()
    {
        if (!expression)
        {
            throw e;
        }
    }

    public static void Check<Ex>(bool expression, Func<Ex> func) where Ex : Exception, new()
    {
        if (!expression)
        {
            throw func();
        }
    }

    public static void Check<Ex>(bool expression, string msg) where Ex : Exception, new()
    {
        if (!expression)
        {
            var ex = Activator.CreateInstance(typeof(Ex), msg);

            if (ex is not null)
            {
                throw (Ex)ex;
            }

            throw new Exception(msg);
        }
    }
}
