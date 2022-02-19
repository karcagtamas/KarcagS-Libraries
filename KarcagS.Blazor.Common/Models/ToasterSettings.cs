using KarcagS.Blazor.Common.Enums;

namespace KarcagS.Blazor.Common.Models;

/// <summary>
/// Toaster settings
/// </summary>
public class ToasterSettings
{
    public string Message { get; set; } = string.Empty;
    public string Caption { get; set; }
    public ToasterType Type { get; set; }
    public bool IsNeeded { get; set; }


    public ToasterSettings()
    {
        IsNeeded = false;
        Caption = string.Empty;
    }

    public ToasterSettings(string caption)
    {
        Caption = caption;
        IsNeeded = true;
    }
}
