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
}