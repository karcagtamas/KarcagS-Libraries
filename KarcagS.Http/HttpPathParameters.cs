using KarcagS.Shared.Common;

namespace KarcagS.Http;

/// <summary>
/// HTTP path parameters
/// </summary>
public class HttpPathParameters : IListState<HttpPathParameters>
{

    private readonly List<object> pathParams;

    /// <summary>
    /// HTTP path parameters
    /// </summary>
    public HttpPathParameters()
    {
        pathParams = [];
    }

    public static HttpPathParameters Build() => new HttpPathParameters();


    /// <summary>
    /// Add value to a specified index into a row (insert).
    /// If the index is equal with -1, it will add to end of the row.
    /// If the index is invalid (index out of range), it will throw errors.
    /// </summary>
    /// <param name="value">Value for adding</param>
    /// <param name="index">Destination index</param>
    /// <typeparam name="T">Type of the value</typeparam>
    public HttpPathParameters Add<T>(T value, int index)
    {
        if (value == null)
        {
            return this;
        }

        switch (index)
        {
            // Add to end of the list
            case -1:
                pathParams.Add(value);
                return this;
            // Negative index
            case < -1:
                throw new ArgumentException("Index cannot be negative");
        }

        // Out of range
        if (index > pathParams.Count)
        {
            throw new ArgumentException("Index cannot be bigger than the list");
        }

        pathParams.Insert(index, value);
        return this;
    }

    /// <summary>
    /// Get length of the row.
    /// </summary>
    /// <returns>Length number</returns>
    public int Count()
    {
        return pathParams.Count;
    }

    /// <summary>
    /// Get value by index number.
    /// If the index is invalid (index out of range), it will throw errors.
    /// </summary>
    /// <param name="index"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Get<T>(int index)
    {
        // Negative
        if (index < 0)
        {
            throw new ArgumentException("Index cannot be negative");
        }

        // Out of range
        if (index >= pathParams.Count)
        {
            throw new ArgumentException("Index cannot be larger than the list size");
        }

        return (T)pathParams[index];
    }

    /// <summary>
    /// Add value to a specified index into a row (insert).
    /// If the index is equal with -1, it will add to end of the row.
    /// If the index is invalid (index out of range), it will not throw errors, but it will not execute the adding.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index"></param>
    /// <typeparam name="T"></typeparam>
    public HttpPathParameters TryAdd<T>(T value, int index)
    {
        if (value is null)
        {
            return this;
        }

        switch (index)
        {
            // Add element end of the row
            case -1:
                pathParams.Add(value);
                return this;
            // Negative
            case < -1:
                return this;
        }

        // Out of range
        if (index > pathParams.Count)
        {
            return this;
        }

        pathParams.Insert(index, value);
        return this;
    }

    /// <summary>
    /// Get value by index number.
    /// If the index is invalid (index out of range), it will not throw errors, but it will give back default value.
    /// </summary>
    /// <param name="index"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? TryGet<T>(int index)
    {
        // Negative
        if (index < 0)
        {
            return default;
        }

        // Out of range
        if (index >= pathParams.Count)
        {
            return default;
        }

        return (T)pathParams[index];
    }

    /// <summary>
    /// List to string.
    /// </summary>
    /// <returns>String in path format</returns>
    public override string ToString() => pathParams.Aggregate("", (current, param) => current + $"/{param}");

    /// <summary>
    /// Add value to a specified index into a row (insert).
    /// If the index is equal with -1, it will add to end of the row.
    /// If the index is invalid (index out of range), it will throw errors.
    /// </summary>
    /// <param name="value">Value for adding</param>
    /// <typeparam name="T">Type of the value</typeparam>
    public HttpPathParameters Add<T>(T value) => Add(value, -1);

    /// <summary>
    /// Add value to a specified index into a row (insert).
    /// If the index is equal with -1, it will add to end of the row.
    /// If the index is invalid (index out of range), it will not throw errors, but it will not execute the adding.
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public HttpPathParameters TryAdd<T>(T value) => TryAdd(value, -1);
}
