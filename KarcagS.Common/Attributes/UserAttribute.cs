namespace KarcagS.Common.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class UserAttribute : Attribute
{
    public bool OnlyInit { get; }
    public bool Force { get; }

    public UserAttribute(bool onlyInit = false, bool force = true)
    {
        OnlyInit = onlyInit;
        Force = force;
    }
}