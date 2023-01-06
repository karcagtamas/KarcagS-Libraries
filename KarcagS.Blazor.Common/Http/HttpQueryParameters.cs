using KarcagS.Shared.Common;

namespace KarcagS.Blazor.Common.Http;

/// <summary>
/// HTTP query parameters
/// </summary>
public class HttpQueryParameters : IDictionaryState<HttpQueryParameters>
{
    private readonly Dictionary<string, object> _queryParams;

    /// <summary>
    /// Query parameters
    /// </summary>
    public HttpQueryParameters()
    {
        _queryParams = new Dictionary<string, object>();
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

        if (_queryParams.ContainsKey(key))
        {
            throw new ArgumentException("Key already exists");
        }

        _queryParams[key] = value;
        return this;
    }

    /// <summary>
    /// Add key with the given value by a predicate function.
    /// If the predicate is true, the query will be added
    /// </summary>
    /// <param name="key">Key value</param>
    /// <param name="value">Value</param>
    /// <typeparam name="T">Type of the value</typeparam>
    public HttpQueryParameters AddOptional<T>(string key, T value, Predicate<T> predicate)
    {
        if (predicate(value))
        {
            return Add(key, value);
        }

        return this;
    }

    /// <summary>
    /// Add multiple query items under one key
    /// </summary>
    /// <typeparam name="T">Type of list items</typeparam>
    /// <param name="key">Key value</param>
    /// <param name="values">Value list</param>
    public HttpQueryParameters AddMultiple<T>(string key, List<T> values)
    {
        return Add(key, values.ToArray());
    }

    /// <summary>
    /// Get value by the given key.
    /// If the given key does not exist it will throw an error
    /// </summary>
    /// <param name="key">Key value</param>
    /// <typeparam name="T">Type of object</typeparam>
    /// <returns>Value for the given key</returns>
    public T Get<T>(string key)
    {
        if (!_queryParams.ContainsKey(key))
        {
            throw new ArgumentException("Key does not exist");
        }
        return (T)_queryParams[key];
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

        if (_queryParams.ContainsKey(key))
        {
            return this;
        }
        _queryParams[key] = value;
        return this;
    }

    /// <summary>
    /// Try add multiple query items under one key
    /// </summary>
    /// <param name="key">Key value</param>
    /// <param name="values">Value list</param>
    /// <typeparam name = "T" > Type of list items</typeparam>
    public HttpQueryParameters TryAddMultiple<T>(string key, List<T> values)
    {
        return TryAdd(key, values.ToArray());
    }

    /// <summary>
    /// Try get value by the given key.
    /// Will not throw errors, but will not execute the adding.
    /// </summary>
    /// <param name="key">Key value</param>
    /// <typeparam name="T">Type of the object</typeparam>
    /// <returns>Value for the given key</returns>
    public T? TryGet<T>(string key)
    {
        if (!_queryParams.ContainsKey(key))
        {
            return default;
        }
        return (T)_queryParams[key];
    }

    /// <summary>
    /// Get length of the dictionary.
    /// </summary>
    /// <returns>Count number</returns>
    public int Count() => _queryParams.Keys.Count;


    /// <summary>
    /// Create string from the dictionary.
    /// Key - value pairs concatenated into string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        List<string> values = new();

        foreach (string key in _queryParams.Keys)
        {
            var value = _queryParams[key];

            if (value is null)
            {
                continue;
            }

            if (value is Array arr)
            {
                foreach (var i in arr)
                {
                    values.Add($"{key}={i}");

                }
            }
            else if (value is Enum e)
            {
                values.Add($"{key}={Convert.ToInt32(e)}");
            }
            else
            {
                values.Add($"{key}={value}");
            }
        }

        return string.Join("&", values);
    }
}
