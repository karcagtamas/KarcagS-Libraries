namespace KarcagS.Shared.Common;

public interface IListState<out U>
{
    public U Add<T>(T value, int index);

    public U Add<T>(T value);

    public T Get<T>(int index);

    public U TryAdd<T>(T value, int index);

    public U TryAdd<T>(T value);

    public T? TryGet<T>(int index);

    public int Count();

    public string ToString();
}
