namespace KarcagS.Blazor.Common.Http;

public interface IHttpService
{
    HttpSender<T> Get<T>(HttpSettings settings);
    HttpSender<string> GetString(HttpSettings settings);
    HttpSender<int> GetInt(HttpSettings settings);
    HttpSender<bool> GetBool(HttpSettings settings);

    HttpSender<object> Post<T>(HttpSettings settings, HttpBody<T> body);
    HttpSender<string> PostString<T>(HttpSettings settings, HttpBody<T> body);
    HttpSender<int> PostInt<T>(HttpSettings settings, HttpBody<T> body);
    HttpSender<T> PostWithResult<T, TBody>(HttpSettings settings, HttpBody<TBody> body);

    HttpSender<object> Put<T>(HttpSettings settings, HttpBody<T> body);
    HttpSender<T> PutWithResult<T, TBody>(HttpSettings settings, HttpBody<TBody> body);

    HttpSender<object> Delete(HttpSettings settings);

    Task<bool> Download(HttpSettings settings);
    Task<bool> Download<T>(HttpSettings settings, T model);

    string BuildUrl(string url, params string[] segments);
}
