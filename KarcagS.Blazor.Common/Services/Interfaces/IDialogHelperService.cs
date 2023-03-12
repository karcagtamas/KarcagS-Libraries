using KarcagS.Blazor.Common.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services.Interfaces;

public interface IDialogHelperService
{
    Task OpenDialog<TComponent>(string title, Action action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase;
    Task<TData?> OpenDialog<TComponent, TData>(string title, Action<TData> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase;
    Task OpenEditorDialog<TComponent>(string title, Action<EditorDialogResult> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase;
}