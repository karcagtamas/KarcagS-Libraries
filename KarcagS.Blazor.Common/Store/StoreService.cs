using Blazored.LocalStorage;

namespace KarcagS.Blazor.Common.Store;

/// <summary>
/// Store Service
/// </summary>
public class StoreService : IStoreService
{
    private readonly ILocalStorageService localStorageService;
    private Dictionary<string, object> Store { get; set; }

    /// <summary>
    /// Store data has been changed
    /// </summary>
    public event EventHandler<StoreEventArgs>? Changed;

    public StoreService(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;
        Store = new Dictionary<string, object>();
    }

    /// <summary>
    /// Init Store Service
    /// </summary>
    /// <returns>Task</returns>
    public void Init(Action<StoreService, ILocalStorageService> action)
    {
        action(this, localStorageService);
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

        OnChanged(key, StoreEvent.Add, value);
    }

    /// <inheritdoc />
    public void Remove(string key)
    {
        Store.Remove(key);
        OnChanged(key, StoreEvent.Remove);
    }

    /// <summary>
    /// On Change event
    /// </summary>
    protected virtual void OnChanged(string key, StoreEvent type, object? value = null)
    {
        Changed?.Invoke(this, new StoreEventArgs(key, type, value, ConstructContext()));
    }

    private StoreContext ConstructContext()
    {
        return new StoreContext
        {
            Values = new Dictionary<string, object>(Store)
        };
    }

    public class StoreContext
    {
        public Dictionary<string, object> Values { get; set; } = new();
    }
}