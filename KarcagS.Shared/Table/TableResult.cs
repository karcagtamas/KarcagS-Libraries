namespace KarcagS.Shared.Table;

public class TableResult
{
    public List<ResultItem> Items { get; set; } = new();
    public int AllItemCount { get; set; } = -1;

    public int All { get => AllItemCount == -1 ? Items.Count : AllItemCount; }

    public class ResultItem
    {
        public string ItemKey { get; set; } = string.Empty;
        public Dictionary<string, string> Values { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public bool ClickDisabled { get; set; } = false;
    }
}
