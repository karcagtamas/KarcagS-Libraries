namespace KarcagS.Blazor.Common.Components.Confirm;

public class ConfirmDialogInput
{
    public Func<Task<bool>>? ActionFunction { get; set; }

    public string Message { get; set; } = null!;
    public int MinContentWidth { get; set; } = 360;
    public bool IsYesNo { get; set; }
}