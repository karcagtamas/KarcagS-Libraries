using System.Text.Json;
using KarcagS.Shared.Http;

namespace KarcagS.Client.Common.Services.Interfaces;

public interface IHelperService
{
    JsonSerializerOptions GetSerializerOptions();
    void AddHttpSuccessToaster(string caption);
    void AddHttpErrorToaster(string caption, HttpErrorResult? error);
    decimal MinToHour(int min);
    int CurrentYear();
    int CurrentMonth();
    DateTime CurrentWeek();
}