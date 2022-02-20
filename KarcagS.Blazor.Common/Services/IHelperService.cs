using KarcagS.Shared;
using System.Text.Json;

namespace KarcagS.Blazor.Common.Services;

public interface IHelperService
{
    void Navigate(string path);
    JsonSerializerOptions GetSerializerOptions();
    void AddHttpSuccessToaster(string caption);
    void AddHttpErrorToaster(string caption, HttpResultError? error);
    decimal MinToHour(int min);
    int CurrentYear();
    int CurrentMonth();
    DateTime CurrentWeek();
}
