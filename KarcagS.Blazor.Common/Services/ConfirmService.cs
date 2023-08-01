using KarcagS.Blazor.Common.Components.Confirm;
using KarcagS.Blazor.Common.Services.Interfaces;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services;

public class ConfirmService : IConfirmService
{
    private readonly IDialogService dialogService;

    public ConfirmService(IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    public Task<bool> Open(ConfirmDialogInput input, string title) => Open(input, title, () => { });

    public Task<bool> Open(ConfirmDialogInput input, string title, Action action, DialogOptions? options = null)
    {
        return Open(input, title, () =>
        {
            action();
            return Task.CompletedTask;
        }, options);
    }

    public async Task<bool> Open(ConfirmDialogInput input, string title, Func<Task> action, DialogOptions? options = null)
    {
        var parameters = new DialogParameters
        {
            {
                "Input",
                input
            }
        };
        var dialog = await dialogService.ShowAsync<Confirm>(title, parameters, options);
        var result = await dialog.Result;

        if (result.Canceled) return false;

        await action();
        return true;
    }
}