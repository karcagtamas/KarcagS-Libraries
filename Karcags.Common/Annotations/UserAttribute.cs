using System;

namespace Karcags.Common.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public class UserAttribute : Attribute
{
    public UserAttribute()
    {

    }
}