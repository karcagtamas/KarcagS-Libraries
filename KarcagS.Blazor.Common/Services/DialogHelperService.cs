using KarcagS.Blazor.Common.Components.Dialogs;
using KarcagS.Blazor.Common.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services;

public class DialogHelperService : IDialogHelperService
{
    private readonly IDialogService dialogService;

    public DialogHelperService(IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    public async Task OpenDialog<TComponent>(string title, Action action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase
        => await OpenDialog<TComponent, object>(title, _ => action(), parameters, options);

    public async Task<TData?> OpenDialog<TComponent, TData>(string title, Action<TData> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase
    {
        var dialog = await dialogService.ShowAsync<TComponent>(title, parameters ?? new DialogParameters(), options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            action((TData)result.Data);
            return (TData)result.Data;
        }

        return default;
    }

    public async Task OpenEditorDialog<TComponent>(string title, Action<EditorDialogResult> action, DialogParameters? parameters = null, DialogOptions? options = null) where TComponent : ComponentBase =>
        await OpenDialog<TComponent, EditorDialogResult>(title, action, parameters, options);
}