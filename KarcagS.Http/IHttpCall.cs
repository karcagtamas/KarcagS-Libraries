namespace KarcagS.Http;

public interface IHttpCall<in TKey>
{
    Task<List<T>> GetAll<T>();
    Task<List<T>> GetAll<T>(string orderBy, string direction = "asc");
    Task<T?> Get<T>(TKey id);
    Task<bool> Create<T>(T model);
    Task<bool> Update<T>(TKey id, T model);
    Task<bool> Delete(TKey id);
}
