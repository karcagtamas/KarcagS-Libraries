using System.Text.Json;
using KarcagS.Client.Common.Services.Interfaces;
using KarcagS.Http.Enums;
using KarcagS.Http.Models;
using KarcagS.Shared.Http;

namespace KarcagS.Client.Common.Services;

public class HelperService(IToasterService toasterService, ILocalizationService localizationService) : IHelperService
{
    protected readonly IToasterService ToasterService = toasterService;
    protected readonly ILocalizationService LocalizationService = localizationService;

    public JsonSerializerOptions GetSerializerOptions() => new() { PropertyNameCaseInsensitive = true };

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
        var message = LocalizationService.GetValue(errorResult?.Message.Text ?? "Server.Message.UnexpectedError", errorResult?.Message.Text ?? "Unexpected Error");
        ToasterService.Open(new ToasterSettings
        {
            Message = message,
            Caption = caption,
            Type = ToasterType.Error
        });
    }

    public decimal MinToHour(int min) => min / (decimal)60;

    public int CurrentYear() => DateTime.Today.Year;

    public int CurrentMonth() => DateTime.Today.Month;

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