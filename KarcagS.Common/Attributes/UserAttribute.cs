using System;

namespace KarcagS.Common.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class UserAttribute : Attribute
{
    public UserAttribute(bool onlyInit = false)
    {
        OnlyInit = onlyInit;
    }

    public bool OnlyInit { get; }
}