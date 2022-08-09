namespace KarcagS.Blazor.Common.Http;

public class HttpConfiguration
{
    public bool IsTokenBearer { get; set; } = false;
    public bool IsTokenRefresher { get; set; } = false;

    public string AccessTokenName { get; set; } = "access-token";
    public string RefreshTokenName { get; set; } = "refresh-token";
    public string RefreshUri { get; set; } = "";
    public string RefreshTokenPlaceholder { get; set; } = ":refreshToken";

    public string ClientIdName { get; set; } = "client-id";
    public string ClientIdPlaceholder { get; set; } = ":clientId";

    public string UnauthorizedPath { get; set; } = "unauthorized";
    public string? UnauthorizedPathRedirectQueryParamName { get; set; }
    public string[] IgnoredPaths { get; set; } = Array.Empty<string>();

    public bool UseErrorDialog { get; set; } = false;
}
