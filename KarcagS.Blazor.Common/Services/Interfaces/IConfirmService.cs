using KarcagS.Blazor.Common.Components.Confirm;

namespace KarcagS.Blazor.Common.Services.Interfaces;

public interface IConfirmService
{
    Task<bool> Open(ConfirmDialogInput input, string title);
    Task<bool> Open(ConfirmDialogInput input, string title, Action action);
}