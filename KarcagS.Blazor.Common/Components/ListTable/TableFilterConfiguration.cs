namespace KarcagS.Blazor.Common.Components.ListTable;

public class TableFilterConfiguration
{
    public bool TextFilterEnabled { get; set; }

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
