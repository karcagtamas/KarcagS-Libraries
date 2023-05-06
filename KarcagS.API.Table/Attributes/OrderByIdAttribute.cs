namespace KarcagS.API.Table.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class OrderByIdAttribute : Attribute
{
    public bool Enabled { get; }

    public OrderByIdAttribute(bool enabled = true)
    {
        Enabled = enabled;
    }
}