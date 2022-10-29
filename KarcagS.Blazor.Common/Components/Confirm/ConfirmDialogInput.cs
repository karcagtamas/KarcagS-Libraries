namespace KarcagS.Blazor.Common.Components.Confirm;

public class ConfirmDialogInput
{
    /// <summary>
    /// Delete function
    /// </summary>
    public Func<Task<bool>>? ActionFunction { get; set; }

    public string Message { get; set; } = default!;
    public int MinContentWidth { get; set; } = 360;
}