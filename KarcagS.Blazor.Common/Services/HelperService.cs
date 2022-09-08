using System.Text.Json;
using KarcagS.Blazor.Common.Components.Dialogs;
using KarcagS.Blazor.Common.Enums;
using KarcagS.Blazor.Common.Models;
using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services;

public class HelperService : IHelperService
{
    protected const string NA = "N/A";
    protected readonly NavigationManager NavigationManager;
    protected readonly IToasterService ToasterService;
    protected readonly IDialogService DialogService;
    protected readonly ILocalizationService LocalizationService;

    public HelperService(NavigationManager navigationManager, IToasterService toasterService, IDialogService dialogService, ILocalizationService localizationService)
    {
        NavigationManager = navigationManager;
        ToasterService = toasterService;
        DialogService = dialogService;
        LocalizationService = localizationService;
    }

    public void Navigate(string path)
    {
        NavigationManager.NavigateTo(path);
    }

    public JsonSerializerOptions GetSerializerOptions()
    {
        return new() { PropertyNameCaseInsensitive = true };
    }

    public void AddHttpSuccessToaster(string caption)
    {
        ToasterService.Open(new ToasterSettings
        {
            Message = LocalizationService.GetValue("Server.Message.SuccessfullyAccomplished", "Event successfully accomplished"),
            Caption = caption,
            Type = ToasterType.Success
        });
    }

    public void AddHttpErrorToaster(string caption, HttpErrorResult? errorResult)
    {
        string message = LocalizationService.GetValue(errorResult?.Message.Text ?? "Server.Message.UnexpectedError", errorResult?.Message.Text ?? "Unexpected Error");
        ToasterService.Open(new ToasterSettings
        {
            Message = message,
            Caption = caption,
            Type = ToasterType.Error
        });
    }

    public decimal MinToHour(int min)
    {
        return min / (decimal)60;
    }

    public int CurrentYear()
    {
        return DateTime.Today.Year;
    }

    public int CurrentMonth()
    {
        return DateTime.Today.Month;
    }

    public DateTime CurrentWeek()
    {
        var date = DateTime.Today;
        while (date.DayOfWeek != DayOfWeek.Monday)
        {
            date = date.AddDays(-1);
        }

        return date;
    }

    public async Task OpenDialog<TComponent>(string title, Action action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase
        => await OpenDialog<TComponent, object>(title, (o) => action(), parameters, options);

    public async Task<TData?> OpenDialog<TComponent, TData>(string title, Action<TData> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase
    {
        var dialog = DialogService.Show<TComponent>(title, parameters ?? new DialogParameters { }, options);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            action((TData)result.Data);
            return (TData)result.Data;
        }
        else
        {
            return default;
        }
    }

    public async Task OpenEditorDialog<TComponent>(string title, Action<EditorDialogResult> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase => await OpenDialog<TComponent, EditorDialogResult>(title, action, parameters, options);

    public async Task<bool> IsInRole(Task<AuthenticationState> stateTask, params string[] roles) => IsInRole(await stateTask, roles);

    public bool IsInRole(AuthenticationState state, params string[] roles) => roles.ToList().Any(r => state.User.IsInRole(r));
}
