using System.Text.Json;
using KarcagS.Blazor.Common.Enums;
using KarcagS.Blazor.Common.Models;
using Microsoft.AspNetCore.Components;

namespace KarcagS.Blazor.Common.Services;

public class HelperService : IHelperService
{
    protected const string NA = "N/A";
    protected readonly NavigationManager NavigationManager;
    protected readonly IToasterService ToasterService;

    public HelperService(NavigationManager navigationManager, IToasterService toasterService)
    {
        this.NavigationManager = navigationManager;
        this.ToasterService = toasterService;
    }

    public void Navigate(string path)
    {
        this.NavigationManager.NavigateTo(path);
    }

    public JsonSerializerOptions GetSerializerOptions()
    {
        return new() {PropertyNameCaseInsensitive = true};
    }

    public async Task AddToaster(HttpResponseMessage response, string caption)
    {
        if (response.IsSuccessStatusCode)
        {
            ToasterService.Open(new ToasterSettings
                {Message = "Event successfully accomplished", Caption = caption, Type = ToasterType.Success});
        }
        else
        {
            await using var sr = await response.Content.ReadAsStreamAsync();
            var e = await JsonSerializer.DeserializeAsync<ErrorResponse>(sr, this.GetSerializerOptions());
            ToasterService.Open(new ToasterSettings
                {Message = e?.Message ?? "-", Caption = caption, Type = ToasterType.Error});
        }
    }

    public decimal MinToHour(int min)
    {
        return min / (decimal) 60;
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
