using KarcagS.Shared.Common;
using KarcagS.Shared.Helpers;

namespace KarcagS.Http;

/// <summary>
/// HTTP query parameters
/// </summary>
public class HttpQueryParameters : IDictionaryState<HttpQueryParameters>
{
    private readonly Dictionary<string, object> queryParams;

    /// <summary>
    /// Query parameters
    /// </summary>
    public HttpQueryParameters()
    {
        queryParams = new Dictionary<string, object>();
    }

    public static HttpQueryParameters Build() => new();

    /// <summary>
    /// Add key with the given value.
    /// If the given key already exists it will throw an error
    /// </summary>
    /// <param name="key">Key value</param>
    /// <param name="value">Value</param>
    /// <typeparam name="T">Type of the value</typeparam>
    public HttpQueryParameters Add<T>(string key, T value)
    {
        if (value is null)
        {
            return this;
        }

        return !queryParams.TryAdd(key, value) ? throw new ArgumentException("Key already exists") : this;
    }

    /// <summary>
    /// Add key with the given value by a predicate function.
    /// If the predicate is true, the query will be added
    /// </summary>
    /// <param name="key">Key value</param>
    /// <param name="value">Value</param>
    /// <param name="predicate">Add optional value by given predicate</param>
    /// <typeparam name="T">Type of the value</typeparam>
    public HttpQueryParameters AddOptional<T>(string key, T value, Predicate<T> predicate)
    {
        return predicate(value) ? Add(key, value) : this;
    }

    /// <summary>
    /// Add multiple query items under one key
    /// </summary>
    /// <typeparam name="T">Type of list items</typeparam>
    /// <param name="key">Key value</param>
    /// <param name="values">Value list</param>
    public HttpQueryParameters AddMultiple<T>(string key, List<T> values) => Add(key, values.ToArray());

    /// <summary>
    /// Get value by the given key.
    /// If the given key does not exist it will throw an error
    /// </summary>
    /// <param name="key">Key value</param>
    /// <typeparam name="T">Type of object</typeparam>
    /// <returns>Value for the given key</returns>
    public T Get<T>(string key)
    {
        if (!queryParams.TryGetValue(key, out var param))
        {
            throw new ArgumentException("Key does not exist");
        }

        return (T)param;
    }

    /// <summary>
    /// Try add key with the given value.
    /// Will not throw errors, but will not execute the adding.
    /// </summary>
    /// <param name="key">Key value</param>
    /// <param name="value">Value</param>
    /// <typeparam name="T">Type of the value</typeparam>
    public HttpQueryParameters TryAdd<T>(string key, T value)
    {
        if (value is null)
        {
            return this;
        }

        queryParams.TryAdd(key, value);
        return this;
    }

    /// <summary>
    /// Try add multiple query items under one key
    /// </summary>
    /// <param name="key">Key value</param>
    /// <param name="values">Value list</param>
    /// <typeparam name = "T" > Type of list items</typeparam>
    public HttpQueryParameters TryAddMultiple<T>(string key, List<T> values) => TryAdd(key, values.ToArray());

    /// <summary>
    /// Try get value by the given key.
    /// Will not throw errors, but will not execute the adding.
    /// </summary>
    /// <param name="key">Key value</param>
    /// <typeparam name="T">Type of the object</typeparam>
    /// <returns>Value for the given key</returns>
    public T? TryGet<T>(string key)
    {
        if (!queryParams.TryGetValue(key, out var param))
        {
            return default;
        }

        return (T)param;
    }

    /// <summary>
    /// Get length of the dictionary.
    /// </summary>
    /// <returns>Count number</returns>
    public int Count() => queryParams.Keys.Count;


    /// <summary>
    /// Create string from the dictionary.
    /// Key - value pairs concatenated into string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        List<string> values = [];

        foreach (var key in queryParams.Keys)
        {
            var value = queryParams[key];

            if (ObjectHelper.IsNull(value))
            {
                continue;
            }

            switch (value)
            {
                case Array arr:
                {
                    values.AddRange(from object? i in arr select $"{key}={i}");

                    break;
                }
                case Enum e:
                    values.Add($"{key}={Convert.ToInt32(e)}");
                    break;
                default:
                    values.Add($"{key}={value}");
                    break;
            }
        }

        return string.Join("&", values);
    }
}