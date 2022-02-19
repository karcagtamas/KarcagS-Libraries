using System.Text.Json;

namespace KarcagS.Blazor.Common.Services;

public interface IHelperService
{
    void Navigate(string path);
    JsonSerializerOptions GetSerializerOptions();
    Task AddToaster(HttpResponseMessage response, string caption);
    decimal MinToHour(int min);
    int CurrentYear();
    int CurrentMonth();
    DateTime CurrentWeek();
}
