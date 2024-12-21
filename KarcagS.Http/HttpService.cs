using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reactive.Threading.Tasks;
using KarcagS.Http.Exceptions;
using KarcagS.Http.Models;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Http;

namespace KarcagS.Http;

public class HttpService : IHttpService
{
    protected readonly HttpClient HttpClient;

    protected readonly HttpConfiguration Configuration;
    protected readonly ITokenHandler TokenHandler;
    private readonly HttpRefreshService refreshService;

    /// <summary>
    /// HTTP Service Injector
    /// </summary>
    /// <param name="httpClient">HTTP Client</param>
    /// <param name="configuration">HTTP Configuration</param>
    /// <param name="tokenHandler">Token Handler</param>
    /// <param name="refreshService">HTTP Refresh Service</param>
    public HttpService(HttpClient httpClient, HttpConfiguration configuration, ITokenHandler tokenHandler, HttpRefreshService refreshService)
    {
        HttpClient = httpClient;
        Configuration = configuration;
        TokenHandler = tokenHandler;
        this.refreshService = refreshService;
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
            .Success(res =>
            {
                if (ObjectHelper.IsNotNull(res))
                {
                    Download(res);
                }
            })
            .Execute();
    }

    public async Task<bool> Download<T>(HttpSettings settings, T model)
    {
        return await PutWithResult<ExportResult, T>(settings, model)
            .Success(res =>
            {
                if (ObjectHelper.IsNotNull(res))
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
        List<string> parts = [url];
        parts.AddRange(segments);
        return string.Join("/", parts);
    }

    private async Task<HttpResult<T>?> SendRequest<T>(HttpSettings settings, HttpMethod method, HttpContent? content, bool afterRefresh = false)
    {
        CheckSettings(settings);

        var url = CreateUrl(settings);
        var request = await BuildRequest(method, content, url);

        try
        {
            var response = await HttpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (Configuration.IsTokenRefresher && !afterRefresh)
                {
                    try
                    {
                        await RefreshWhenPossible();
                        return await SendRequest<T>(settings, method, content, true);
                    }
                    catch (HttpTokenRefreshException)
                    {
                        HandlingUnauthorizedPathRedirection();
                        return await MakeResult<T>(response, settings.ToasterSettings);
                    }
                }

                HandlingUnauthorizedPathRedirection();
                return await MakeResult<T>(response, settings.ToasterSettings);
            }

            try
            {
                return await MakeResult<T>(response, settings.ToasterSettings);
            }
            catch (HttpResponseParseException e)
            {
                ConsoleSerializationError(e);
                throw;
            }
            catch (Exception e)
            {
                throw new HttpException("The HTTP response read was unsuccessful", e);
            }
        }
        catch (HttpResponseParseException e)
        {
            ConsoleSerializationError(e);
            throw;
        }
        catch (HttpException)
        {
            throw;
        }
        catch (Exception e)
        {
            ConsoleCallError(e, url);
            throw;
        }
    }

    private async Task<HttpResult<T>?> MakeResult<T>(HttpResponseMessage? response, ToasterSettings toasterSettings)
    {
        if (ObjectHelper.IsNull(response))
        {
            if (toasterSettings.IsNeeded)
            {
                // HelperService.AddHttpErrorToaster(toasterSettings.Caption, null);
                AddErrorToaster(toasterSettings.Caption, null);
            }

            return null;
        }

        HttpResult<T>? result;

        try
        {
            result = await Parse<T>(response);
        }
        catch (Exception e)
        {
            throw new HttpResponseParseException("The response is not parseable", e);
        }

        if (toasterSettings.IsNeeded)
        {
            if (ObjectHelper.IsNull(result))
            {
                // HelperService.AddHttpErrorToaster(toasterSettings.Caption, null);
                AddErrorToaster(toasterSettings.Caption, null);
            }
            else
            {
                if (result.IsSuccess)
                {
                    // HelperService.AddHttpSuccessToaster(toasterSettings.Caption);
                    AddSuccessToaster(toasterSettings.Caption);
                }
                else
                {
                    // HelperService.AddHttpErrorToaster(toasterSettings.Caption, result.Error);
                    AddErrorToaster(toasterSettings.Caption, result.Error);
                }
            }
        }

        return result;
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
            var token = await TokenHandler.GetAccessToken(Configuration);
            var isApiUrl = request.RequestUri?.IsAbsoluteUri ?? false;

            if (!string.IsNullOrEmpty(token) && isApiUrl)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return request;
    }

    protected virtual bool Download(ExportResult result) => throw new NotImplementedException();

    private static async Task<HttpResult<T>?> Parse<T>(HttpResponseMessage response) => await response.Content.ReadFromJsonAsync<HttpResult<T>>();

    /// <summary>
    /// Create URL from HTTP settings
    /// Concatenate URL, path parameters and query parameters
    /// </summary>
    /// <param name="settings">HTTP settings</param>
    /// <returns>Created URL</returns>
    private static string CreateUrl(HttpSettings settings)
    {
        var url = settings.Url;

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
        if (ObjectHelper.IsNull(settings))
        {
            throw new ArgumentException("Settings cannot be null");
        }
    }

    protected virtual void ConsoleSerializationError(Exception e) => Console.WriteLine($"SERIALIZATION ERROR: {e.Message}");

    protected virtual void ConsoleCallError(Exception e, string url) => Console.WriteLine($"CALL ERROR: {e.Message}");

    protected virtual void ConsoleTokenRefreshError(Exception e) => Console.WriteLine($"TOKEN REFRESH ERROR: {e.Message}");

    protected virtual void AddErrorToaster(string caption, HttpErrorResult? errorResult)
    {
    }

    protected virtual void AddSuccessToaster(string caption)
    {
    }

    private async Task RefreshWhenPossible()
    {
        if (refreshService.Current().InProgress)
        {
            var result = await refreshService.RefreshInProgressSubject.ToTask();

            if (!result.LastSuccess)
            {
                await Refresh();
            }
        }
        else
        {
            refreshService.RefreshInProgressSubject.OnNext(HttpRefreshService.RefreshState.ProgressState(refreshService.Current().LastSuccess));
            try
            {
                await Refresh();
            }
            catch (Exception)
            {
                refreshService.RefreshInProgressSubject.OnNext(HttpRefreshService.RefreshState.FinishState(false));
                throw;
            }

            refreshService.RefreshInProgressSubject.OnNext(HttpRefreshService.RefreshState.FinishState(true));
        }
    }

    private async Task Refresh()
    {
        if (string.IsNullOrEmpty(Configuration.RefreshUri))
        {
            throw new HttpTokenRefreshException("Missing Refresh Uri configuration");
        }

        var refreshToken = await TokenHandler.GetRefreshToken(Configuration);
        var clientId = await TokenHandler.GetClientId(Configuration);

        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new HttpTokenRefreshException("Refresh token is not set");
        }

        if (string.IsNullOrEmpty(clientId))
        {
            throw new HttpTokenRefreshException("Client Id is not set");
        }

        var request = await BuildRequest(HttpMethod.Get, null, Configuration.RefreshUri.Replace(Configuration.RefreshTokenPlaceholder, refreshToken).Replace(Configuration.ClientIdPlaceholder, clientId));

        try
        {
            var response = await HttpClient.SendAsync(request);
            var parsedResponse = await Parse<HttpRefreshResult>(response);

            if (ObjectHelper.IsNull(parsedResponse) || ObjectHelper.IsNull(parsedResponse.Result))
            {
                throw new HttpTokenRefreshException("Parsed response is empty");
            }

            if (string.IsNullOrEmpty(parsedResponse.Result.AccessToken))
            {
                throw new HttpTokenRefreshException("Parsed Access Token is empty");
            }

            if (string.IsNullOrEmpty(parsedResponse.Result.RefreshToken))
            {
                throw new HttpTokenRefreshException("Parsed Refresh Token is empty");
            }

            await TokenHandler.SetAccessToken(parsedResponse.Result.AccessToken, Configuration);
            await TokenHandler.SetRefreshToken(parsedResponse.Result.RefreshToken, Configuration);
            await TokenHandler.SetClientId(parsedResponse.Result.ClientId, Configuration);
        }
        catch (HttpTokenRefreshException)
        {
            throw;
        }
        catch (Exception e)
        {
            ConsoleTokenRefreshError(e);
            throw new HttpTokenRefreshException("Error during the token refresh", e);
        }
    }

    protected virtual void HandlingUnauthorizedPathRedirection()
    {
    }

    private class HttpRefreshResult
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string ClientId { get; set; } = string.Empty;
    }
}