namespace KarcagS.Blazor.Common.Http;

public interface IHttpService
{
    Task<T> Get<T>(HttpSettings settings);
    Task<string> GetString(HttpSettings settings);
    Task<int?> GetInt(HttpSettings settings);
    Task<bool> GetBool(HttpSettings settings);

    Task<bool> Post<T>(HttpSettings settings, HttpBody<T> body);
    Task<string> PostString<T>(HttpSettings settings, HttpBody<T> body);
    Task<int> PostInt<T>(HttpSettings settings, HttpBody<T> body);
    Task<T> PostWithResult<T, TBody>(HttpSettings settings, HttpBody<TBody> body);

    Task<bool> Put<T>(HttpSettings settings, HttpBody<T> body);
    Task<T> PutWithResult<T, TBody>(HttpSettings settings, HttpBody<TBody> body);

    Task<bool> Delete(HttpSettings settings);

    Task<bool> Download(HttpSettings settings);
    Task<bool> Download<T>(HttpSettings settings, T model);

    string BuildUrl(string url, params string[] segments);
}
