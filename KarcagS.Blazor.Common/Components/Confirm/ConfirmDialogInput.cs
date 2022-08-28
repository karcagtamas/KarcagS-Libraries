using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Shared.Localization;

namespace KarcagS.Blazor.Common.Components.Confirm;

public class ConfirmDialogInput
{
    /// <summary>
    /// Confirm Name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Delete function
    /// </summary>
    public Func<Task<bool>> ActionFunction { get; set; } = null!;

    public string ActionName { get; set; } = "delete";
    public string? Message { get; set; }

    public string FormattedMessage(ILocalizationService localizationService)
    {
        if (ObjectHelper.IsNotNull(Message))
        {
            return Message;
        }

        var action = localizationService.GetValue(ActionName).ToLower();

        return localizationService.GetValue(LibraryLocalizer.ConfirmMessageKey, "Are you sure want to {0} {1}?", action, Name);
    }
}