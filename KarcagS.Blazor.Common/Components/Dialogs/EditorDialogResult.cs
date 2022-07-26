namespace KarcagS.Blazor.Common.Components.Dialogs;

public class EditorDialogResult
{
    public EditorCloseEvent? Event { get; set; }
    public bool Performed { get; set; }

    public static EditorDialogResult PerformedResult(EditorCloseEvent e)
    {
        return new EditorDialogResult
        {
            Event = e,
            Performed = true
        };
    }

    public static EditorDialogResult ClosedResult()
    {
        return new EditorDialogResult
        {
            Performed = false
        };
    }
}

public enum EditorCloseEvent
{
    Edit,
    Remove
}
