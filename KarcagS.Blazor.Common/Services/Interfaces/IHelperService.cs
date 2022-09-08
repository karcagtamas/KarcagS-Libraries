using KarcagS.Blazor.Common.Components.Dialogs;
using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Text.Json;

namespace KarcagS.Blazor.Common.Services.Interfaces;

public interface IHelperService
{
    void Navigate(string path);
    Task OpenDialog<TComponent>(string title, Action action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase;
    Task<TData?> OpenDialog<TComponent, TData>(string title, Action<TData> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase;
    Task OpenEditorDialog<TComponent>(string title, Action<EditorDialogResult> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase;
    JsonSerializerOptions GetSerializerOptions();
    void AddHttpSuccessToaster(string caption);
    void AddHttpErrorToaster(string caption, HttpErrorResult? error);
    decimal MinToHour(int min);
    int CurrentYear();
    int CurrentMonth();
    DateTime CurrentWeek();
    Task<bool> IsInRole(Task<AuthenticationState> stateTask, params string[] roles);
    bool IsInRole(AuthenticationState state, params string[] roles);
}
