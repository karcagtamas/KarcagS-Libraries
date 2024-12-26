using Microsoft.AspNetCore.Components.Web;

namespace KarcagS.Blazor.Common.Components.Table;

public class TableRowClickArgs<T>
{
    public required T Item { get; set; }

    public SourceButton Button
    {
        get
        {
            return MouseEventArgs.Button switch
            {
                0 => SourceButton.Left,
                1 => SourceButton.Middle,
                2 => SourceButton.Right,
                _ => SourceButton.Left
            };
        }
    }

    public bool IsDoubleClick => MouseEventArgs.Detail > 1;

    public required MouseEventArgs MouseEventArgs { get; set; }

    public enum SourceButton
    {
        Left,
        Middle,
        Right,
    }
}