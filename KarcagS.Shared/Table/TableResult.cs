namespace KarcagS.Shared.Table;

public class TableResult<T>
{
    public List<T> Data { get; set; } = new();
    public int AllDataCount { get; set; }
}
