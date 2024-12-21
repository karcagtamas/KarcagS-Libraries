namespace KarcagS.API.Table.Configurations;

public class OrderingConfiguration
{
    public bool OrderingEnabled { get; set; }

    private OrderingConfiguration()
    {

    }

    public static OrderingConfiguration Build() => new();

    public OrderingConfiguration IsEnabled(bool value = true)
    {
        OrderingEnabled = value;

        return this;
    }
}
