namespace KarcagS.Http;

public interface ITokenHandler
{
    Task<string> GetAccessToken(HttpConfiguration configuration);
    Task<string> GetRefreshToken(HttpConfiguration configuration);
    Task<string> GetClientId(HttpConfiguration configuration);
    Task SetAccessToken(string value, HttpConfiguration configuration);
    Task SetRefreshToken(string value, HttpConfiguration configuration);
    Task SetClientId(string value, HttpConfiguration configuration);
    Task RemoveAccessToken(HttpConfiguration configuration);
    Task RemoveRefreshToken(HttpConfiguration configuration);
    Task RemoveClientId(HttpConfiguration configuration);
}