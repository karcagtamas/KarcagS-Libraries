namespace KarcagS.API.Repository.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class UserAttribute(bool onlyInit = false, bool force = true) : Attribute
{
    public bool OnlyInit { get; } = onlyInit;
    public bool Force { get; } = force;
}