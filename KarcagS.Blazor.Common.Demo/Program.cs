using Blazored.LocalStorage;
using KarcagS.Blazor.Common;
using KarcagS.Blazor.Common.Demo;
using KarcagS.Blazor.Common.Demo.Services;
using KarcagS.Blazor.Common.Http;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Services;
using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Blazor.Common.Store;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpService(config =>
{
    config.IsTokenBearer = true;
    config.UnauthorizedPath = "/logout";
    config.AccessTokenName = "access-token";
    config.IsTokenRefresher = true;
    config.RefreshTokenName = "refresh-token";
    config.RefreshUri = builder.Configuration.GetSection("RefreshUri").Value;
    config.RefreshTokenPlaceholder = ":refreshToken";
    config.ClientIdName = "client-id";
    config.ClientIdPlaceholder = ":clientId";
});
builder.Services.AddStoreService(async (storeService, localStorage) =>
{
    var user = await localStorage.GetItemAsync<object>("user");

    if (user != null)
    {
        storeService.Add("user", user);
    }
});
builder.Services.AddScoped<IHelperService, HelperService>();
builder.Services.AddScoped<IToasterService, ToasterService>();

builder.Services.AddScoped<IDemoTableService, DemoTableService>();

builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

ApplicationSettings.BaseUrl = builder.Configuration.GetSection("SecureApi").Value;
ApplicationSettings.BaseApiUrl = $"{ApplicationSettings.BaseUrl}/api";

ApplicationContext.ApplicationName = "Papyrus";

await builder.Build().RunAsync();
