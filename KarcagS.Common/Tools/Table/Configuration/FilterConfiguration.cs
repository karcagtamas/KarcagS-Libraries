namespace KarcagS.Common.Tools.Table.Configuration;

public class FilterConfiguration
{
    public bool TextFilterEnabled { get; set; } = false;

    private FilterConfiguration()
    {

    }

    public static FilterConfiguration Build() => new();

    public FilterConfiguration IsTextFilterEnabled(bool value)
    {
        TextFilterEnabled = value;

        return this;
    }
}
