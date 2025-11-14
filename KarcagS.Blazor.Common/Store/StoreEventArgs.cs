using static KarcagS.Blazor.Common.Store.StoreService;

namespace KarcagS.Blazor.Common.Store;

public class StoreEventArgs(string key, StoreEvent type, object? value, StoreContext context)
    : EventArgs
{
    public string Key { get; set; } = key;
    public StoreEvent Type { get; set; } = type;
    public object? Value { get; set; } = value;

    public StoreContext Context { get; set; } = context;
}
