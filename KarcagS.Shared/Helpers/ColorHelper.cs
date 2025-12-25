namespace KarcagS.Shared.Helpers;

public static class ColorHelper
{
    public static string GetForegroundColor(string backgroundColor)
    {
        var luminance = CalculateLuminance(HexToRBG(backgroundColor));
        return luminance < 140 ? "#fff" : "#000";
    }

    private static float CalculateLuminance(List<int> rgb)
    {
        return (float)(0.2126 * rgb[0] + 0.7152 * rgb[1] + 0.0722 * rgb[2]);
    }

    private static List<int> HexToRBG(string colorStr)
    {
        var rbg = new List<int>
        {
            Convert.ToInt32(colorStr.Substring(1, 2), 16),
            Convert.ToInt32(colorStr.Substring(3, 2), 16),
            Convert.ToInt32(colorStr.Substring(5, 2), 16)
        };
        return rbg;
    }
}