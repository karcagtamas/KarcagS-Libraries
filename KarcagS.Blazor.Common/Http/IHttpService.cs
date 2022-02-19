using Karcags.Common.Tools.ErrorHandling;

namespace KarcagS.Blazor.Common.Http;

public interface IHttpService
{
    Task Get<T>(HttpSettings settings, Action<T?> success, Action<HttpResultError?> error);
    Task GetString(HttpSettings settings, Action<string?> success, Action<HttpResultError?> error);
    Task GetInt(HttpSettings settings, Action<int?> success, Action<HttpResultError?> error);
    Task GetBool(HttpSettings settings, Action<bool?> success, Action<HttpResultError?> error);

    HttpSender<T> Get<T>(HttpSettings settings);

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
