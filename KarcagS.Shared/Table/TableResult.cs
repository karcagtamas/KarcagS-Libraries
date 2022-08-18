namespace KarcagS.Shared.Table;

public class TableResult<T>
{
    public List<T> Items { get; set; } = new();
    public int AllItemCount { get; set; } = -1;

    public int All { get => AllItemCount == -1 ? Items.Count : AllItemCount; }
}
