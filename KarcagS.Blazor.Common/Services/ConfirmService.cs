using KarcagS.Blazor.Common.Components.Confirm;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services;

public class ConfirmService : IConfirmService
{
    private readonly IDialogService dialogService;

    public ConfirmService(IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    public async Task<bool> Open(ConfirmDialogInput input, string title)
    {
        return await Open(input, title, () => { });
    }

    public async Task<bool> Open(ConfirmDialogInput input, string title, Action action)
    {
        var parameters = new DialogParameters
        {
            {
                "Input",
                input
            }
        };
        var dialog = dialogService.Show<Confirm>(title, parameters);
        var result = await dialog.Result;

        if (result.Cancelled) return false;

        action();
        return true;
    }
}