namespace KarcagS.Shared.Common;

public interface IDictionaryState<out TD>
{
    public TD Add<T>(string key, T value);

    public T Get<T>(string key);

    public TD TryAdd<T>(string key, T value);

    public T? TryGet<T>(string key);

    public int Count();

    public string ToString();
}
