namespace KarcagS.Blazor.Common.Components.Table;

public class TableFilterConfiguration
{
    public bool TextFilterEnabled { get; set; } = false;

    private TableFilterConfiguration()
    {

    }

    public static TableFilterConfiguration Build() => new();

    public TableFilterConfiguration IsTextFilterEnabled(bool value)
    {
        TextFilterEnabled = value;

        return this;
    }
}
