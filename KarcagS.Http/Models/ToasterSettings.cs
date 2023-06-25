using KarcagS.Http.Enums;

namespace KarcagS.Http.Models;

/// <summary>
/// Toaster settings
/// </summary>
public class ToasterSettings
{
    public string Message { get; set; } = string.Empty;
    public string Caption { get; set; }
    public ToasterType Type { get; set; }
    public bool IsNeeded => !string.IsNullOrEmpty(Caption);


    public ToasterSettings()
    {
        Caption = string.Empty;
    }

    public ToasterSettings(string caption)
    {
        Caption = caption;
    }
}