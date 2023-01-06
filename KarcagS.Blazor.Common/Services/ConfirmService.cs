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

    public async Task<bool> Open(ConfirmDialogInput input, string title) => await Open(input, title, () => { });

    public async Task<bool> Open(ConfirmDialogInput input, string title, Action action, DialogOptions? options = null)
    {
        var parameters = new DialogParameters
        {
            {
                "Input",
                input
            }
        };
        var dialog = dialogService.Show<Confirm>(title, parameters, options);
        var result = await dialog.Result;

        if (result.Canceled) return false;

        action();
        return true;
    }
}