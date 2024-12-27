namespace KarcagS.Shared.Table;

public class TableResult<TKey>
{
    public List<ResultItem<TKey>> Items { get; set; } = [];
    public int AllItemCount { get; set; } = -1;
    public int FilteredAllItemCount { get; set; } = -1;

    public int All => AllItemCount == -1 ? Items.Count : AllItemCount;

    public int FilteredAll => FilteredAllItemCount == -1 ? All : FilteredAllItemCount;
}

public class ResultItem<TKey>
{
    public TKey ItemKey { get; set; } = default!;
    public Dictionary<string, ItemValue> Values { get; set; } = new();
    public List<string> Tags { get; set; } = [];
    public bool ClickDisabled { get; set; }
}

public class ItemValue
{
    public string Value { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public bool ActionDisabled { get; set; }
}