using KarcagS.Blazor.Common.Models.Interfaces;

namespace KarcagS.Blazor.Common.Http;

/// <summary>
/// HTTP query parameters
/// </summary>
public class HttpQueryParameters : IDictionaryState
{
    private readonly Dictionary<string, object> _queryParams;

    /// <summary>
    /// Query parameters
    /// </summary>
    public HttpQueryParameters()
    {
        this._queryParams = new Dictionary<string, object>();
    }
    /// 
    /// <summary>
    /// Add key with the given value.
    /// If the given key already exists it will throw an error
    /// </summary>
    /// <param name="key">Key value</param>
    /// <param name="value">Value</param>
    /// <typeparam name="T">Type of the value</typeparam>
    public void Add<T>(string key, T value)
    {
        if (this._queryParams.ContainsKey(key))
        {
            throw new ArgumentException("Key already exists");
        }
        this._queryParams[key] = value;
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
        if (!this._queryParams.ContainsKey(key))
        {
            throw new ArgumentException("Key does not exist");
        }
        return (T)this._queryParams[key];
    }

    /// <summary>
    /// Try add key with the given value.
    /// Will not throw errors, but will not execute the adding.
    /// </summary>
    /// <param name="key">Key value</param>
    /// <param name="value">Value</param>
    /// <typeparam name="T">Type of the value</typeparam>
    public void TryAdd<T>(string key, T value)
    {
        if (this._queryParams.ContainsKey(key))
        {
            return;
        }
        this._queryParams[key] = value;
    }

    /// <summary>
    /// Try get value by the given key.
    /// Will not throw errors, but will not execute the adding.
    /// </summary>
    /// <param name="key">Key value</param>
    /// <typeparam name="T">Type of the object</typeparam>
    /// <returns>Value for the given key</returns>
    public T TryGet<T>(string key)
    {
        if (!this._queryParams.ContainsKey(key))
        {
            return default;
        }
        return (T)this._queryParams[key];
    }

    /// <summary>
    /// Get length of the dictionary.
    /// </summary>
    /// <returns>Count number</returns>
    public int Count()
    {
        return this._queryParams.Keys.Count;
    }

    /// <summary>
    /// Create string from the dictionary.
    /// Key - value pairs concatenated into string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string val = "";

        foreach (string key in this._queryParams.Keys)
        {
            val += val != "" ? "&" : "";
            val += $"{key}={this._queryParams[key]}";
        }

        return val;
    }
}
