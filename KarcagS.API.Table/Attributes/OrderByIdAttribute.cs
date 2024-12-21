namespace KarcagS.API.Table.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class OrderByIdAttribute(bool enabled = true) : Attribute
{
    public bool Enabled { get; } = enabled;
}