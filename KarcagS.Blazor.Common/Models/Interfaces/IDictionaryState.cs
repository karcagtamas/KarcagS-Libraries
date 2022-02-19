namespace KarcagS.Blazor.Common.Models.Interfaces;

public interface IDictionaryState
{
    public void Add<T>(string key, T value);

    public T Get<T>(string key);

    public void TryAdd<T>(string key, T value);

    public T TryGet<T>(string key);

    public int Count();

    public string ToString();
}
