namespace KarcagS.Shared.Table;

public class TableResult<TKey>
{
    public List<ResultItem<TKey>> Items { get; set; } = new();
    public int AllItemCount { get; set; } = -1;
    public int FilteredAllItemCount { get; set; } = -1;

    public int All { get => AllItemCount == -1 ? Items.Count : AllItemCount; }
    public int FilteredAll { get => FilteredAllItemCount == -1 ? All : FilteredAllItemCount; }
}

public class ResultItem<TKey>
{
    public TKey ItemKey { get; set; } = default!;
    public Dictionary<string, ItemValue> Values { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public bool ClickDisabled { get; set; } = false;


}

public class ItemValue
{
    public string Value { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public bool ActionDisabled { get; set; }
}

public class ResultRowItem<TKey> : ResultItem<TKey>
{
    public bool Selected { get; set; } = false;
    public bool Disabled { get; set; } = false;

    public ResultRowItem()
    {

    }

    public ResultRowItem(ResultItem<TKey> item)
    {
        ItemKey = item.ItemKey;
        Values = item.Values;
        Tags = item.Tags;
        ClickDisabled = item.ClickDisabled;
    }
}
