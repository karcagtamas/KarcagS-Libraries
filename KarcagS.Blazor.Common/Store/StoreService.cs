using Blazored.LocalStorage;

namespace KarcagS.Blazor.Common.Store;

/// <summary>
/// Store Service
/// </summary>
public class StoreService : IStoreService
{
    private readonly ILocalStorageService _localStorageService;
    private Dictionary<string, object> Store { get; set; }

    /// <summary>
    /// Store data has been changed
    /// </summary>
    public event EventHandler? Changed;

    /// <summary>
    /// 
    /// </summary>
    public StoreService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
        Store = new Dictionary<string, object>();
    }

    /// <summary>
    /// Init Store Service
    /// </summary>
    /// <returns>Task</returns>
    public void Init(Action<StoreService, ILocalStorageService> action)
    {
        action(this, _localStorageService);
    }

    /// <inheritdoc />
    public T? Get<T>(string key)
    {
        return (T?)Store.GetValueOrDefault(key);
    }

    /// <inheritdoc />
    public bool IsExists(string key)
    {
        return Store.ContainsKey(key);
    }

    /// <inheritdoc />
    public void Add<T>(string key, T value)
    {
        if (Store.ContainsKey(key))
        {
            Remove(key);
        }

        if (value is not null)
        {
            Store.Add(key, value);
        }

        OnChanged();
    }

    /// <inheritdoc />
    public void Remove(string key)
    {
        Store.Remove(key);
        OnChanged();
    }

    /// <summary>
    /// On Change event
    /// </summary>
    protected virtual void OnChanged()
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
