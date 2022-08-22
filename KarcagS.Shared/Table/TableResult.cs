namespace KarcagS.Shared.Table;

public class TableResult
{
    public List<ResultItem> Items { get; set; } = new();
    public int AllItemCount { get; set; } = -1;
    public int FilteredAllItemCount { get; set; } = -1;

    public int All { get => AllItemCount == -1 ? Items.Count : AllItemCount; }
    public int FilteredAll { get => FilteredAllItemCount == -1 ? All : FilteredAllItemCount; }

    public class ResultItem
    {
        public string ItemKey { get; set; } = string.Empty;
        public Dictionary<string, ItemValue> Values { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public bool ClickDisabled { get; set; } = false;

        public class ItemValue
        {
            public string Value { get; set; } = string.Empty;
            public List<string> Tags { get; set; } = new();
        }
    }

    public class ResultRowItem : ResultItem
    {
        public bool Selected { get; set; }
        public bool Disabled { get; set; }
        public bool Hidden { get; set; }

        public ResultRowItem()
        {

        }

        public ResultRowItem(ResultItem item)
        {
            ItemKey = item.ItemKey;
            Values = item.Values;
            Tags = item.Tags;
            ClickDisabled = item.ClickDisabled;
        }
    }
}
