using static KarcagS.Blazor.Common.Store.StoreService;

namespace KarcagS.Blazor.Common.Store;

public class StoreEventArgs : EventArgs
{
    public string Key { get; set; }
    public StoreEvent Type { get; set; }
    public object? Value { get; set; }

    public StoreContext Context { get; set; }

    public StoreEventArgs(string key, StoreEvent type, object? value, StoreContext context)
    {
        Key = key;
        Type = type;
        Value = value;
        Context = context;
    }
}
