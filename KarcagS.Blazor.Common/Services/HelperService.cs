using System.Text.Json;
using KarcagS.Blazor.Common.Enums;
using KarcagS.Blazor.Common.Models;
using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services;

public class HelperService : IHelperService
{
    protected const string NA = "N/A";
    protected readonly NavigationManager navigationManager;
    protected readonly IToasterService toasterService;
    protected readonly IDialogService dialogService;

    public HelperService(NavigationManager navigationManager, IToasterService toasterService, IDialogService dialogService)
    {
        this.navigationManager = navigationManager;
        this.toasterService = toasterService;
        this.dialogService = dialogService;
    }

    public void Navigate(string path)
    {
        navigationManager.NavigateTo(path);
    }

    public JsonSerializerOptions GetSerializerOptions()
    {
        return new() { PropertyNameCaseInsensitive = true };
    }

    public void AddHttpSuccessToaster(string caption)
    {
        toasterService.Open(new ToasterSettings
        {
            Message = "Event successfully accomplished",
            Caption = caption,
            Type = ToasterType.Success
        });
    }

    public void AddHttpErrorToaster(string caption, HttpResultError? errorResult)
    {
        string message = errorResult?.Message ?? "Unexpected Error";
        toasterService.Open(new ToasterSettings
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
    {
        await OpenDialog<TComponent, object>(title, (o) => action(), parameters, options);
    }

    public async Task<TData?> OpenDialog<TComponent, TData>(string title, Action<TData> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase
    {
        var dialog = dialogService.Show<TComponent>(title, parameters, options);
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
}
