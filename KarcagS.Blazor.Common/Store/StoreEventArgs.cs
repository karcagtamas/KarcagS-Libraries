namespace KarcagS.Blazor.Common.Store;

public class StoreEventArgs : EventArgs
{
    public string Key { get; set; }
    public StoreEvent Type { get; set; }

    public StoreEventArgs(string key, StoreEvent type)
    {
        Key = key;
        Type = type;
    }
}
