namespace KarcagS.Client.Common.Http;

public interface IHttpService
{
    HttpSender<T> Get<T>(HttpSettings settings);
    HttpSender<string> GetString(HttpSettings settings);
    HttpSender<int> GetInt(HttpSettings settings);
    HttpSender<bool> GetBool(HttpSettings settings);

    HttpSender<object?> Post<TBody>(HttpSettings settings, TBody body);
    HttpSender<string> PostString<TBody>(HttpSettings settings, TBody body);
    HttpSender<int> PostInt<TBody>(HttpSettings settings, TBody body);
    HttpSender<TResult> PostWithResult<TResult, TBody>(HttpSettings settings, TBody body);
    HttpSender<object?> PostWithoutBody(HttpSettings settings);

    HttpSender<object?> Put<TBody>(HttpSettings settings, TBody body);
    HttpSender<TResult> PutWithResult<TResult, TBody>(HttpSettings settings, TBody body);
    HttpSender<object?> PutWithoutBody(HttpSettings settings);

    HttpSender<object?> Delete(HttpSettings settings);

    Task<bool> Download(HttpSettings settings);
    Task<bool> Download<T>(HttpSettings settings, T model);

    string BuildUrl(string url, params string[] segments);
}
