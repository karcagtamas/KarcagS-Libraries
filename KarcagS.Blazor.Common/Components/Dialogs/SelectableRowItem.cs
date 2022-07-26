using KarcagS.Shared.Common;

namespace KarcagS.Blazor.Common.Components.Dialogs;

public class SelectableRowItem<T, TKey> where T : IIdentified<TKey>
{
    public T Data { get; set; } = default!;
    public bool Selected { get; set; }
}
