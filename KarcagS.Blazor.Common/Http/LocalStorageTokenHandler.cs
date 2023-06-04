using Blazored.LocalStorage;
using KarcagS.Client.Common.Http;

namespace KarcagS.Blazor.Common.Http;

public class LocalStorageTokenHandler : ITokenHandler
{
    private readonly ILocalStorageService localStorageService;

    public LocalStorageTokenHandler(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;
    }

    public Task<string> GetAccessToken(HttpConfiguration configuration) => GetFromLocalStorage(configuration.AccessTokenName);
    public Task<string> GetRefreshToken(HttpConfiguration configuration) => GetFromLocalStorage(configuration.RefreshTokenName);

    public Task<string> GetClientId(HttpConfiguration configuration) => GetFromLocalStorage(configuration.ClientIdName);

    public Task SetAccessToken(string value, HttpConfiguration configuration) => SetToLocalStorage(configuration.AccessTokenName, value);

    public Task SetRefreshToken(string value, HttpConfiguration configuration) => SetToLocalStorage(configuration.RefreshTokenName, value);

    public Task SetClientId(string value, HttpConfiguration configuration) => SetToLocalStorage(configuration.ClientIdName, value);

    public Task RemoveAccessToken(HttpConfiguration configuration) => RemoveFromLocalStorage(configuration.AccessTokenName);

    public Task RemoveRefreshToken(HttpConfiguration configuration) => RemoveFromLocalStorage(configuration.RefreshTokenName);

    public Task RemoveClientId(HttpConfiguration configuration) => RemoveFromLocalStorage(configuration.ClientIdName);

    private Task<string> GetFromLocalStorage(string key) => localStorageService.GetItemAsync<string>(key).AsTask();
    private Task SetToLocalStorage(string key, string value) => localStorageService.SetItemAsync(key, value).AsTask();
    private Task RemoveFromLocalStorage(string key) => localStorageService.RemoveItemAsync(key).AsTask();
}