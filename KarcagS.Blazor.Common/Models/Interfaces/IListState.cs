namespace KarcagS.Blazor.Common.Models.Interfaces;

public interface IListState<TL>
{
    public TL Add<T>(T value, int index);

    public TL Add<T>(T value);

    public T Get<T>(int index);

    public TL TryAdd<T>(T value, int index);

    public TL TryAdd<T>(T value);

    public T TryGet<T>(int index);

    public int Count();

    public string ToString();
}
