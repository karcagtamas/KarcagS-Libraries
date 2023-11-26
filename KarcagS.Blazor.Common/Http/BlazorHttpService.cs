using KarcagS.Client.Common.Http;
using KarcagS.Client.Common.Services.Interfaces;
using KarcagS.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;

namespace KarcagS.Blazor.Common.Http;

public class BlazorHttpService : ClientHttpService
{
    private readonly IJSRuntime jsRuntime;
    private readonly NavigationManager navigationManager;

    public BlazorHttpService(
        HttpClient httpClient,
        HttpConfiguration configuration,
        ITokenHandler tokenHandler,
        HttpRefreshService refreshService,
        IHelperService helperService,
        IJSRuntime jsRuntime,
        NavigationManager navigationManager
    ) : base(httpClient, configuration, tokenHandler, refreshService, helperService)
    {
        this.jsRuntime = jsRuntime;
        this.navigationManager = navigationManager;
    }

    protected override void HandlingUnauthorizedPathRedirection()
    {
        var query = new Dictionary<string, string>();

        if (ObjectHelper.IsNotNull(Configuration.UnauthorizedPathRedirectQueryParamName))
        {
            query.Add(Configuration.UnauthorizedPathRedirectQueryParamName, navigationManager.ToBaseRelativePath(navigationManager.Uri));
        }

        navigationManager.NavigateTo(query.Count > 0 ? QueryHelpers.AddQueryString(Configuration.UnauthorizedPath, query) : Configuration.UnauthorizedPath);
    }

    protected override bool Download(ExportResult result)
    {
        if (jsRuntime is IJSUnmarshalledRuntime unmarshalledRuntime)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            unmarshalledRuntime.InvokeUnmarshalled<string, string, byte[], bool>("manageDownload", result.FileName, result.ContentType, result.Content);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        return true;
    }

    protected override void ConsoleCallError(Exception e, string url)
    {
        try
        {
            ((IJSInProcessRuntime)jsRuntime).Invoke<object>("console.log",
                new ConsoleError { Error = $"HTTP Call Error from {url}", Exception = e.ToString() });
        }
        catch (Exception)
        {
            Console.WriteLine("CALL ERROR");
        }
    }

    protected override void ConsoleSerializationError(Exception e)
    {
        try
        {
            ((IJSInProcessRuntime)jsRuntime).Invoke<object>("console.log",
                new ConsoleError { Error = "Serialization Error", Exception = e.ToString() });
        }
        catch (Exception)
        {
            Console.WriteLine("SERIALIZATION ERROR");
        }
    }

    protected override void ConsoleTokenRefreshError(Exception e)
    {
        try
        {
            ((IJSInProcessRuntime)jsRuntime).Invoke<object>("console.log",
                new ConsoleError { Error = "HTTP Token refresh Error", Exception = e.ToString() });
        }
        catch (Exception)
        {
            Console.WriteLine("TOKEN REFRESH ERROR");
        }
    }

    private class ConsoleError
    {
        public string Error { get; init; } = string.Empty;

        public string Exception { get; init; } = string.Empty;
    }
}