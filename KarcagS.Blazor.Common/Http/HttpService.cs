using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using KarcagS.Blazor.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using KarcagS.Shared.Http;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using KarcagS.Blazor.Common.Services.Interfaces;

namespace KarcagS.Blazor.Common.Http;

public class HttpService : IHttpService
{
    protected readonly HttpClient HttpClient;

    protected readonly IHelperService HelperService;

    protected readonly IJSRuntime JsRuntime;

    protected readonly HttpConfiguration Configuration;

    protected readonly ILocalStorageService LocalStorageService;

    protected readonly NavigationManager NavigationManager;

    /// <summary>
    /// HTTP Service Injector
    /// </summary>
    /// <param name="httpClient">HTTP Client</param>
    /// <param name="helperService">Helper Service</param>
    /// <param name="jsRuntime">JS Runtime</param>
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
    /// GET request
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <typeparam name="T">Type of the result</typeparam>
    /// <returns>HttpSender</returns>
    public HttpSender<T> Get<T>(HttpSettings settings) => new(async () => await SendRequest<T>(settings, HttpMethod.Get, null));

    /// <summary>
    /// GET request with string result
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>HttpSender</returns>
    public HttpSender<string> GetString(HttpSettings settings) => Get<string>(settings);

    /// <summary>
    /// GET request with int result
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>HttpSender</returns>
    public HttpSender<int> GetInt(HttpSettings settings) => Get<int>(settings);

    /// <summary>
    /// GET request with bool result
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>HttpSender</returns>
    public HttpSender<bool> GetBool(HttpSettings settings) => Get<bool>(settings);

    /// <summary>
    /// POST request with any result
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <param name="body">Body of post request</param>
    /// <typeparam name="TBody">Type of the body</typeparam>
    /// <returns>HttpSender</returns>
    public HttpSender<object?> Post<TBody>(HttpSettings settings, TBody body) => PostWithResult<object?, TBody>(settings, body);

    /// <summary>
    /// POST request with string result
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <param name="body">Body of post request</param>
    /// <typeparam name="TBody">Type of the body</typeparam>
    /// <returns>HttpSender</returns>
    public HttpSender<string> PostString<TBody>(HttpSettings settings, TBody body) => PostWithResult<string, TBody>(settings, body);

    /// <summary>
    /// POST request with int result
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <param name="body">Body of post request</param>
    /// <typeparam name="TBody">Type of the body</typeparam>
    /// <returns>HttpSender</returns>
    public HttpSender<int> PostInt<TBody>(HttpSettings settings, TBody body) => PostWithResult<int, TBody>(settings, body);

    /// <summary>
    /// POST request
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <param name="body">Body of post request</param>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <typeparam name="TBody">Type of the body</typeparam>
    /// <returns>HttpSender</returns>
    public HttpSender<TResult> PostWithResult<TResult, TBody>(HttpSettings settings, TBody body) => new(async () => await SendRequest<TResult>(settings, HttpMethod.Post, new HttpBody<TBody>(body).GetStringContent()));

    /// <summary>
    /// POST request without body
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>HttpSender</returns>
    public HttpSender<object?> PostWithoutBody(HttpSettings settings) => PostWithResult<object?, object?>(settings, null);

    /// <summary>
    /// PUT request with any result
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <param name="body">Body of put request</param>
    /// <typeparam name="TBody">Type of the body</typeparam>
    /// <returns>HttpSender</returns>
    public HttpSender<object?> Put<TBody>(HttpSettings settings, TBody body) => PutWithResult<object?, TBody>(settings, body);

    /// <summary>
    /// PUT request
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <param name="body">Body of put request</param>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <typeparam name="TBody">Type of the body</typeparam>
    /// <returns>HttpSender</returns>
    public HttpSender<TResult> PutWithResult<TResult, TBody>(HttpSettings settings, TBody body) => new(async () => await SendRequest<TResult>(settings, HttpMethod.Put, new HttpBody<TBody>(body).GetStringContent()));

    /// <summary>
    /// PUT request without body
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>HttpSender</returns>
    public HttpSender<object?> PutWithoutBody(HttpSettings settings) => PutWithResult<object?, object?>(settings, null);

    /// <summary>
    /// DELETE request
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>HttpSender</returns>
    public HttpSender<object?> Delete(HttpSettings settings) => new(async () => await SendRequest<object?>(settings, HttpMethod.Delete, null));

    public async Task<bool> Download(HttpSettings settings)
    {
        return await Get<ExportResult>(settings)
            .Success((res) =>
            {
                if (res is not null)
                {
                    Download(res);
                }
            })
            .Execute();
    }

    public async Task<bool> Download<T>(HttpSettings settings, T model)
    {
        return await PutWithResult<ExportResult, T>(settings, model)
            .Success((res) =>
            {
                if (res is not null)
                {
                    Download(res);
                }
            })
            .Execute();
    }


    /// <summary>
    /// Build URL
    /// </summary>
    /// <param name="url">Url</param>
    /// <param name="segments"></param>
    /// <returns>Created url</returns>
    public string BuildUrl(string url, params string[] segments)
    {
        List<string> parts = new() { url };
        parts.AddRange(segments);
        return string.Join("/", parts);
    }

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

                    HandlingUnauthorizedPathRedirection();
                    return await MakeResult<T>(response, settings.ToasterSettings);
                }

                HandlingUnauthorizedPathRedirection();
                return await MakeResult<T>(response, settings.ToasterSettings);
            }

            try
            {
                return await MakeResult<T>(response, settings.ToasterSettings);
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

    private async Task<HttpResult<T>?> MakeResult<T>(HttpResponseMessage? response, ToasterSettings toasterSettings)
    {
        if (response is null)
        {
            if (toasterSettings.IsNeeded)
            {
                HelperService.AddHttpErrorToaster(toasterSettings.Caption, null);
            }
            return null;
        }
        else
        {
            var result = await Parse<T>(response);

            if (toasterSettings.IsNeeded)
            {
                if (result is null)
                {
                    HelperService.AddHttpErrorToaster(toasterSettings.Caption, null);
                }
                else
                {
                    if (result.IsSuccess)
                    {
                        HelperService.AddHttpSuccessToaster(toasterSettings.Caption);
                    }
                    else
                    {
                        HelperService.AddHttpErrorToaster(toasterSettings.Caption, result.Error);
                    }
                }
            }

            return result;
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
            Console.WriteLine("SERIALIZATION ERROR");
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
            Console.WriteLine("CALL ERROR");
        }
    }

    private void ConsoleTokenRefreshError(Exception e)
    {
        try
        {
            ((IJSInProcessRuntime)JsRuntime).Invoke<object>("console.log",
                new ConsoleError { Error = $"HTTP Token refresh Error", Exception = e.ToString() });
        }
        catch (Exception)
        {
            Console.WriteLine("TOKEN REFRESH ERROR");
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
            var parsedResponse = await Parse<HttpRefreshResult>(response);

            if (ObjectHelper.IsNull(parsedResponse))
            {
                return false;
            }

            var res = parsedResponse.Result;

            if (ObjectHelper.IsNull(res) || string.IsNullOrEmpty(res.AccessToken) || string.IsNullOrEmpty(res.RefreshToken))
            {
                return false;
            }

            await LocalStorageService.SetItemAsync(Configuration.AccessTokenName, res.AccessToken);
            await LocalStorageService.SetItemAsync(Configuration.RefreshTokenName, res.RefreshToken);
            await LocalStorageService.SetItemAsync(Configuration.ClientIdName, res.ClientId);

            return true;
        }
        catch (Exception e)
        {
            ConsoleTokenRefreshError(e);
            return false;
        }
    }

    private void HandlingUnauthorizedPathRedirection()
    {
        var query = new Dictionary<string, string>();

        if (ObjectHelper.IsNotNull(Configuration.UnauthorizedPathRedirectQueryParamName))
        {
            query.Add(Configuration.UnauthorizedPathRedirectQueryParamName, NavigationManager.ToBaseRelativePath(NavigationManager.Uri));
        }

        if (query.Count > 0)
        {
            NavigationManager.NavigateTo(QueryHelpers.AddQueryString(Configuration.UnauthorizedPath, query));
        }
        else
        {
            NavigationManager.NavigateTo(Configuration.UnauthorizedPath);
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
