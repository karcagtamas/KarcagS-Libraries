namespace KarcagS.Blazor.Common.Components.Table.Styles;

public abstract class Style
{
    public static string ToProperty(string key, string value) => $"{key}: {value}";

    public static string ToProperty(string key, double value) => $"{key}: {value}px";

    public static string ToProperty(string key, NumericStyleValue value) => ToProperty(key, value.Stringify());

    public static string ConcatStyles(List<string> styles) => string.Join("; ", styles);

    public static string ConcatClasses(List<string> classes) => string.Join(" ", classes);

    public enum StyleValueUnit
    {
        Pixel,
        Percent,
        Em,
        Rem,
    }

    public record NumericStyleValue(double Value, StyleValueUnit Unit)
    {
        public string Stringify() => $"{Value}{ConvertUnit(Unit)}";

        private string ConvertUnit(StyleValueUnit unit)
        {
            return unit switch
            {
                StyleValueUnit.Pixel => "px",
                StyleValueUnit.Percent => "%",
                StyleValueUnit.Em => "em",
                StyleValueUnit.Rem => "rem",
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
            };
        }
    }
}