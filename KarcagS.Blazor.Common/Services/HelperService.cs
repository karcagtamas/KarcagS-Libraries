using System.Text.Json;
using KarcagS.Blazor.Common.Enums;
using KarcagS.Blazor.Common.Models;
using KarcagS.Shared;
using Microsoft.AspNetCore.Components;

namespace KarcagS.Blazor.Common.Services;

public class HelperService : IHelperService
{
    protected const string NA = "N/A";
    protected readonly NavigationManager NavigationManager;
    protected readonly IToasterService ToasterService;

    public HelperService(NavigationManager navigationManager, IToasterService toasterService)
    {
        NavigationManager = navigationManager;
        ToasterService = toasterService;
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
            Message = "Event successfully accomplished",
            Caption = caption,
            Type = ToasterType.Success
        });
    }

    public void AddHttpErrorToaster(string caption, HttpResultError? errorResult)
    {
        string message = errorResult?.Message ?? "Unexpected Error";
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
}
