using System.ComponentModel.DataAnnotations;

namespace KarcagS.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NullablePhoneAttribute : DataTypeAttribute
{
    public NullablePhoneAttribute() : base(DataType.PhoneNumber)
    {

    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue))
            {
                return true;
            }

            var attr = new PhoneAttribute();
            return attr.IsValid(value);
        }

        return false;
    }
}
