namespace KarcagS.Shared.Table;

public class PaginatedResult<T>
{
    public List<T> Entries { get; set; } = new();
    public int AllEntry { get; set; }
}
