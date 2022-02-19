using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Karcags.Common.Tools.ErrorHandling;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KarcagS.Blazor.Common.Http;

public class HttpService : IHttpService
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
    public async Task<bool> Post<T>(HttpSettings settings, HttpBody<T> body) => (await SendRequest<object>(settings, HttpMethod.Post, body.GetStringContent()))?.IsSuccess ?? false;


    /// <summary>
    /// POST request where we want string response
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <param name="body">Body of post request</param>
    /// <typeparam name="T">Type of the body</typeparam>
    /// <returns>Response string value</returns>
    public async Task<string> PostString<T>(HttpSettings settings, HttpBody<T> body) => await PostWithResult<string, T>(settings, body);


    /// <summary>
    /// DELETE request
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>The request was success or not</returns>
    public async Task<bool> Delete(HttpSettings settings) => (await SendRequest<object>(settings, HttpMethod.Delete, null))?.IsSuccess ?? false;


    /// <summary>
    /// GET request
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <typeparam name="T">Type of the result</typeparam>
    /// <returns>Response as T type</returns>
    public async Task Get<T>(HttpSettings settings, Action<T?> success, Action<HttpResultError?> error)
    {
        var response = await SendRequest<T>(settings, HttpMethod.Get, null);

        if (response is not null)
        {
            if (response.IsSuccess)
            {
                success(response.Result);
            }
            else
            {
                error(response.Error);
            }
        }
        else
        {
            error(null);
        }
    }


    /// <summary>
    /// Get number
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>Number response</returns>
    public async Task GetInt(HttpSettings settings, Action<int?> success, Action<HttpResultError?> error) => await Get(settings, success, error);


    /// <summary>
    /// Get string
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>String response</returns>
    public async Task GetString(HttpSettings settings, Action<string?> success, Action<HttpResultError?> error) => await Get(settings, success, error);

    /// <summary>
    /// Get bool
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>Boolean response</returns>
    public async Task GetBool(HttpSettings settings, Action<bool?> success, Action<HttpResultError?> error) => await Get(settings, success, error);


    /// <summary>
    /// PUT request
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <param name="body">Body of put request</param>
    /// <typeparam name="T">Type of the body</typeparam>
    /// <returns>The request was success or not</returns>
    public async Task<bool> Put<T>(HttpSettings settings, HttpBody<T> body) => (await SendRequest<object>(settings, HttpMethod.Put, body.GetStringContent()))?.IsSuccess ?? false;


    public async Task<T> PutWithResult<T, TBody>(HttpSettings settings, HttpBody<TBody> body) => (await SendRequest<T>(settings, HttpMethod.Put, body.GetStringContent())).Content;


    public async Task<int> PostInt<T>(HttpSettings settings, HttpBody<T> body) => await PostWithResult<int, T>(settings, body);

    public async Task<T> PostWithResult<T, TBody>(HttpSettings settings, HttpBody<TBody> body) => (await SendRequest<T>(settings, HttpMethod.Post, body.GetStringContent())).Content;

    public async Task<bool> Download(HttpSettings settings)
    {
        await Get<ExportResult>(settings,
            (res) =>
            {
                if (res is not null)
                {
                    Download(res);
                }
            },
            (err) => { });
    }

    public async Task<bool> Download<T>(HttpSettings settings, T model)
    {
        var body = new HttpBody<T>(model);

        return Download(await PutWithResult<ExportResult, T>(settings, body));
    }


    /// <summary>
    /// Build URL
    /// </summary>
    /// <param name="url">Url</param>
    /// <param name="segments"></param>
    /// <returns>Created url</returns>
    public string BuildUrl(string url, params string[] segments) => $"{url}/{string.Join("/", segments)}";

    private async Task<HttpResult<T>?> SendRequest<T>(HttpSettings settings, HttpMethod method, HttpContent? content) => await SendRequest<T>(settings, method, content, false);


    private async Task<HttpResult<T>?> SendRequest<T>(HttpSettings settings, HttpMethod method, HttpContent? content, bool afterRefresh = false)
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
                        return await SendRequest<T>(settings, method, content, true);
                    }

                    NavigationManager.NavigateTo(Configuration.UnauthorizedPath);
                    return await Parse<T>(response);
                }

                NavigationManager.NavigateTo(Configuration.UnauthorizedPath);
                return await Parse<T>(response);
            }

            if (settings.ToasterSettings.IsNeeded)
            {
                await HelperService.AddToaster(response, settings.ToasterSettings.Caption);
            }

            try
            {
                return await Parse<T>(response);
            }
            catch (Exception e)
            {
                ConsoleSerializationError(e);
                return null;
            }
        }
        catch (Exception e)
        {
            ConsoleCallError(e, url);
            return null;
        }
    }

    private async Task<HttpRequestMessage> BuildRequest(HttpMethod method, HttpContent? content, string url)
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

    private static async Task<HttpResult<T>?> Parse<T>(HttpResponseMessage response) => await response.Content.ReadFromJsonAsync<HttpResult<T>>();


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

        var request = await BuildRequest(HttpMethod.Get, null, Configuration.RefreshUri.Replace(Configuration.RefreshTokenPlaceholder, refreshToken).Replace(Configuration.ClientIdPlaceholder, clientId));

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




    private class ConsoleError
    {
        public string Error { get; set; } = string.Empty;

        public string Exception { get; set; } = string.Empty;
    }

    private class HttpRefreshResult
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string ClientId { get; set; } = string.Empty;
    }
}
