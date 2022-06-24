using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;

namespace KarcagS.Blazor.Common.Services;

public interface IHelperService
{
    void Navigate(string path);
    Task<TData> OpenDialog<TComponent, TData>(string title, DialogParameters? parameters, DialogOptions? options, Action action) where TComponent : ComponentBase;
    JsonSerializerOptions GetSerializerOptions();
    void AddHttpSuccessToaster(string caption);
    void AddHttpErrorToaster(string caption, HttpResultError? error);
    decimal MinToHour(int min);
    int CurrentYear();
    int CurrentMonth();
    DateTime CurrentWeek();
}
