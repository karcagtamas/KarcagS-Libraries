using KarcagS.Client.Common.Models;

namespace KarcagS.Client.Common.Http;

/// <summary>
/// HTTP Settings
/// </summary>
public class HttpSettings
{
    public string Url { get; private set; } = string.Empty;
    public HttpQueryParameters QueryParameters { get; private set; }
    public HttpPathParameters PathParameters { get; private set; }
    public ToasterSettings ToasterSettings { get; private set; }

    /// <summary>
    /// Settings only with url.
    /// </summary>
    /// <param name="url">Url</param>
    public HttpSettings(string url)
    {
        SetUrl(url);
        QueryParameters = new HttpQueryParameters();
        PathParameters = new HttpPathParameters();
        ToasterSettings = new ToasterSettings();
    }

    /// <summary>
    /// Add Query parameters
    /// </summary>
    /// <param name="queryParams">Query Parameters</param>
    /// <returns>Settings</returns>
    public HttpSettings AddQueryParams(HttpQueryParameters queryParams)
    {
        QueryParameters = queryParams;

        return this;
    }

    /// <summary>
    /// Add Path parameters
    /// </summary>
    /// <param name="pathParams">Query Parameters</param>
    /// <returns>Settings</returns>
    public HttpSettings AddPathParams(HttpPathParameters pathParams)
    {
        PathParameters = pathParams;

        return this;
    }

    /// <summary>
    /// Add Toaster settings
    /// </summary>
    /// <param name="settings">Toaster Settings</param>
    /// <returns>Settings</returns>
    public HttpSettings AddToaster(ToasterSettings settings)
    {
        ToasterSettings = settings;

        return this;
    }

    /// <summary>
    /// Add Toaster settings by caption
    /// </summary>
    /// <param name="toasterCaption">Toaster Caption</param>
    /// <returns>Settings</returns>
    public HttpSettings AddToaster(string toasterCaption)
    {
        if (!string.IsNullOrEmpty(toasterCaption))
        {
            ToasterSettings = new ToasterSettings(toasterCaption);
        }

        return this;
    }

    /// <summary>
    /// Set Url. 
    /// If it is invalid, throw an exception.
    /// </summary>
    /// <param name="url">New url</param>
    private void SetUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentException("Invalid url");
        }

        Url = url;
    }
}