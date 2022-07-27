namespace KarcagS.Shared.Helpers;

public static class ColorHelper
{
    public static string GetForegroundColor(string backgroundColor)
    {
        float luminance = CalculateLuminance(HexToRBG(backgroundColor));
        string inverse = (luminance < 140) ? "#fff" : "#000";
        return inverse;
    }

    private static float CalculateLuminance(List<int> rgb)
    {
        return (float)(0.2126 * rgb[0] + 0.7152 * rgb[1] + 0.0722 * rgb[2]);
    }

    private static List<int> HexToRBG(string colorStr)
    {
        var rbg = new List<int>();
        rbg.Add(Convert.ToInt32(colorStr.Substring(1, 2), 16));
        rbg.Add(Convert.ToInt32(colorStr.Substring(3, 2), 16));
        rbg.Add(Convert.ToInt32(colorStr.Substring(5, 2), 16));
        return rbg;
    }
}
