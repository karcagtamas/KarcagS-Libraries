using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using KarcagS.Blazor.Common.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KarcagS.Blazor.Common.Http;

public class HttpService<TError, TValidationError> : IHttpService
{
    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly HttpClient HttpClient;

    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly IHelperService HelperService;

    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly IJSRuntime JsRuntime;

    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly HttpConfiguration Configuration;

    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly ILocalStorageService LocalStorageService;

    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly NavigationManager NavigationManager;

    /// <summary>
    /// HTTP Service Injector
    /// </summary>
    /// <param name="httpClient">HTTP Client</param>
    /// <param name="helperService">Helper Service</param>
    /// <param name="jsRuntime">Js Runtime</param>
    /// <param name="configuration">HTTP Configuration</param>
    /// <param name="localStorageService">Local Storage Service</param>
    /// <param name="navigationManager">Navigation Manager</param>
    public HttpService(HttpClient httpClient, IHelperService helperService, IJSRuntime jsRuntime,
        HttpConfiguration configuration, ILocalStorageService localStorageService,
        NavigationManager navigationManager)
    {
        HttpClient = httpClient;
        HelperService = helperService;
        JsRuntime = jsRuntime;
        Configuration = configuration;
        LocalStorageService = localStorageService;
        NavigationManager = navigationManager;
    }

    /// <summary>
    /// POST request
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <param name="body">Body of post request</param>
    /// <typeparam name="T">Type of the body</typeparam>
    /// <returns>The request was success or not</returns>
    public async Task<bool> Post<T>(HttpSettings settings, HttpBody<T> body)
    {
        return (await SendRequest<object>(settings, HttpMethod.Post, body.GetStringContent())).IsSuccess;
    }

    /// <summary>
    /// POST request where we want string response
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <param name="body">Body of post request</param>
    /// <typeparam name="T">Type of the body</typeparam>
    /// <returns>Response string value</returns>
    public async Task<string> PostString<T>(HttpSettings settings, HttpBody<T> body)
    {
        return await PostWithResult<string, T>(settings, body);
    }

    /// <summary>
    /// DELETE request
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>The request was success or not</returns>
    public async Task<bool> Delete(HttpSettings settings)
    {
        return (await SendRequest<object>(settings, HttpMethod.Delete, null)).IsSuccess;
    }

    /// <summary>
    /// GET request
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <typeparam name="T">Type of the result</typeparam>
    /// <returns>Response as T type</returns>
    public async Task<T> Get<T>(HttpSettings settings)
    {
        return (await SendRequest<T>(settings, HttpMethod.Get, null, true)).Content;
    }

    /// <summary>
    /// Get number
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>Number response</returns>
    public async Task<int?> GetInt(HttpSettings settings)
    {
        return await Get<int?>(settings);
    }

    /// <summary>
    /// Get string
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>String response</returns>
    public async Task<string> GetString(HttpSettings settings)
    {
        return await Get<string>(settings);
    }

    /// <summary>
    /// PUT request
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <param name="body">Body of put request</param>
    /// <typeparam name="T">Type of the body</typeparam>
    /// <returns>The request was success or not</returns>
    public async Task<bool> Put<T>(HttpSettings settings, HttpBody<T> body)
    {
        return (await SendRequest<object>(settings, HttpMethod.Put, body.GetStringContent())).IsSuccess;
    }

    public async Task<T> PutWithResult<T, TBody>(HttpSettings settings, HttpBody<TBody> body)
    {
        return (await SendRequest<T>(settings, HttpMethod.Put, body.GetStringContent(), true)).Content;
    }

    public async Task<int> PostInt<T>(HttpSettings settings, HttpBody<T> body)
    {
        return await PostWithResult<int, T>(settings, body);
    }

    public async Task<bool> Download(HttpSettings settings)
    {
        return Download(await Get<ExportResult>(settings));
    }

    public async Task<bool> Download<T>(HttpSettings settings, T model)
    {
        var body = new HttpBody<T>(model);

        return Download(await PutWithResult<ExportResult, T>(settings, body));
    }

    public async Task<T> PostWithResult<T, TBody>(HttpSettings settings, HttpBody<TBody> body)
    {
        return (await SendRequest<T>(settings, HttpMethod.Post, body.GetStringContent(), true)).Content;
    }

    /// <summary>
    /// Build URL
    /// </summary>
    /// <param name="url">Url</param>
    /// <param name="segments"></param>
    /// <returns>Created url</returns>
    public string BuildUrl(string url, params string[] segments)
    {
        return $"{url}/{string.Join("/", segments)}";
    }

    private async Task<HttpResponse<T>> SendRequest<T>(HttpSettings settings, HttpMethod method,
        HttpContent content)
    {
        return await SendRequest<T>(settings, method, content, false);
    }

    private async Task<HttpResponse<T>> SendRequest<T>(HttpSettings settings, HttpMethod method,
        HttpContent content, bool parseResult, bool afterRefresh = false)
    {
        CheckSettings(settings);

        var url = CreateUrl(settings);
        var request = await BuildRequest(method, content, url);

        try
        {
            using var response = await HttpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (Configuration.IsTokenRefresher && !afterRefresh)
                {
                    if (await Refresh())
                    {
                        return await SendRequest<T>(settings, method, content, parseResult, true);
                    }

                    NavigationManager.NavigateTo(Configuration.UnauthorizedPath);
                    return new HttpResponse<T> { IsSuccess = false, HasErrorContent = false };
                }

                NavigationManager.NavigateTo(Configuration.UnauthorizedPath);
                return new HttpResponse<T> { IsSuccess = false, HasErrorContent = false };
            }

            if (settings.ToasterSettings.IsNeeded)
            {
                await HelperService.AddToaster(response, settings.ToasterSettings.Caption);
            }

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    return new HttpResponse<T>
                    {
                        IsSuccess = false,
                        HasErrorContent = true,
                        ErrorContent = new ErrorContent
                        { Type = ErrorContentType.Error, Content = Parse<TError>(response) }
                    };
                }
                catch (Exception)
                {
                    try
                    {
                        return new HttpResponse<T>
                        {
                            IsSuccess = false,
                            HasErrorContent = true,
                            ErrorContent = new ErrorContent
                            {
                                Type = ErrorContentType.ValidationError,
                                Content = Parse<TValidationError>(response)
                            }
                        };
                    }
                    catch
                    {
                        return new HttpResponse<T> { IsSuccess = false, HasErrorContent = false };
                    }
                }
            }

            try
            {
                if (parseResult)
                {
                    return await Parse<T>(response);
                }

                return new HttpResponse<T> { IsSuccess = true, HasErrorContent = false };
            }
            catch (Exception e)
            {
                ConsoleSerializationError(e);
                return new HttpResponse<T> { IsSuccess = false, HasErrorContent = false };
            }
        }
        catch (Exception e)
        {
            ConsoleCallError(e, url);
            return new HttpResponse<T> { IsSuccess = false, HasErrorContent = false };
        }
    }

    private async Task<HttpRequestMessage> BuildRequest(HttpMethod method, HttpContent content, string url)
    {
        var request = new HttpRequestMessage(method, url);
        if (content != null)
        {
            request.Content = content;
        }

        if (Configuration.IsTokenBearer)
        {
            var token = await LocalStorageService.GetItemAsync<string>(Configuration.AccessTokenName);
            var isApiUrl = request.RequestUri?.IsAbsoluteUri ?? false;

            if (!string.IsNullOrEmpty(token) && isApiUrl)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return request;
    }

    private bool Download(ExportResult result)
    {
        if (JsRuntime is IJSUnmarshalledRuntime unmarshalledRuntime)
        {
            unmarshalledRuntime.InvokeUnmarshalled<string, string, byte[], bool>("manageDownload", result.FileName,
                result.ContentType, result.Content);
        }

        return true;
    }

    private static async Task<HttpResponse<T>> Parse<T>(HttpResponseMessage response)
    {
        return new HttpResponse<T>
        { IsSuccess = true, HasErrorContent = false, Content = await response.Content.ReadFromJsonAsync<T>() };
    }

    /// <summary>
    /// Create URL from HTTP settings
    /// Concatenate URL, path parameters and query parameters
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>Created URL</returns>
    private static string CreateUrl(HttpSettings settings)
    {
        string url = settings.Url;

        if (settings.PathParameters.Count() > 0)
        {
            url += settings.PathParameters.ToString();
        }

        if (settings.QueryParameters.Count() > 0)
        {
            url += $"?{settings.QueryParameters}";
        }

        return url;
    }

    private static void CheckSettings(HttpSettings settings)
    {
        if (settings == null)
        {
            throw new ArgumentException("Settings cannot be null");
        }
    }

    private void ConsoleSerializationError(Exception e)
    {
        try
        {
            ((IJSInProcessRuntime)JsRuntime).Invoke<object>("console.log",
                new ConsoleError { Error = "Serialization Error", Exception = e.ToString() });
        }
        catch (Exception)
        {
            Console.WriteLine("ERROR");
        }
    }

    private void ConsoleCallError(Exception e, string url)
    {
        try
        {
            ((IJSInProcessRuntime)JsRuntime).Invoke<object>("console.log",
                new ConsoleError { Error = $"HTTP Call Error from {url}", Exception = e.ToString() });
        }
        catch (Exception)
        {
            Console.WriteLine("ERROR");
        }
    }

    private async Task<bool> Refresh()
    {
        if (string.IsNullOrEmpty(Configuration.RefreshUri))
        {
            return false;
        }

        var refreshToken = await LocalStorageService.GetItemAsync<string>(Configuration.RefreshTokenName);
        var clientId = await LocalStorageService.GetItemAsync<string>(Configuration.ClientIdName);

        if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(clientId))
        {
            return false;
        }

        var request = await BuildRequest(HttpMethod.Get, null,
            Configuration.RefreshUri.Replace(Configuration.RefreshTokenPlaceholder, refreshToken).Replace(Configuration.ClientIdPlaceholder, clientId));

        try
        {
            using var response = await HttpClient.SendAsync(request);
            var res = await response.Content.ReadFromJsonAsync<HttpRefreshResult>();

            if (res is null || string.IsNullOrEmpty(res.AccessToken) || string.IsNullOrEmpty(res.RefreshToken))
            {
                return false;
            }

            await LocalStorageService.SetItemAsync(Configuration.AccessTokenName, res.AccessToken);
            await LocalStorageService.SetItemAsync(Configuration.RefreshTokenName, res.RefreshToken);
            await LocalStorageService.SetItemAsync(Configuration.ClientIdName, res.ClientId);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Get bool
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>Boolean response</returns>
    public async Task<bool> GetBool(HttpSettings settings)
    {
        return await Get<bool>(settings);
    }

    private class ConsoleError
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string Error { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string Exception { get; set; }
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private class HttpRefreshResult
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string AccessToken { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string RefreshToken { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string ClientId { get; set; }
    }
}
