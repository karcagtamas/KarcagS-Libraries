using KarcagS.Shared.Helpers;

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

    public string Msg 
    { 
        get => ObjectHelper.OrElse(Message, $"Are you sure want to {ActionName} {Name}?"); 
    }
}