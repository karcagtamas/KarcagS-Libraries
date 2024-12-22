using KarcagS.Blazor.Common.Components.Dialogs;
using KarcagS.Blazor.Common.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services;

public class DialogHelperService(IDialogService dialogService) : IDialogHelperService
{
    public Task OpenDialog<TComponent>(string title, Action action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase
        => OpenDialog<TComponent, object>(title, _ => action(), parameters, options);

    public Task<TData?> OpenDialog<TComponent, TData>(string title, Action<TData?> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase
    {
        return OpenDialog<TComponent, TData>(title, data =>
        {
            action(data);
            return Task.CompletedTask;
        }, parameters, options);
    }

    public async Task<TData?> OpenDialog<TComponent, TData>(string title, Func<TData?, Task> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase
    {
        var dialog = await dialogService.ShowAsync<TComponent>(title, parameters ?? new DialogParameters(), options);
        var result = await dialog.Result;

        if (ObjectHelper.IsNotNull(result) && !result.Canceled)
        {
            await action((TData?)result.Data);
            return (TData?)result.Data;
        }

        return default;
    }

    public Task OpenEditorDialog<TComponent>(string title, Action<EditorDialogResult> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase =>
        OpenDialog<TComponent, EditorDialogResult>(title, result =>
        {
            if (ObjectHelper.IsNotNull(result))
            {
                action(result);
            }
        }, parameters, options);

    public Task OpenEditorDialog<TComponent>(string title, Func<EditorDialogResult, Task> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase =>
        OpenDialog<TComponent, EditorDialogResult>(title, result => ObjectHelper.IsNotNull(result) ? action(result) : Task.CompletedTask, parameters, options);
}