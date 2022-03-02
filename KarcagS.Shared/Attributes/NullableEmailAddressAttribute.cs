using System.ComponentModel.DataAnnotations;

namespace KarcagS.Shared.Attributes;

public class NullableEmailAddressAttribute : DataTypeAttribute
{
    public NullableEmailAddressAttribute() : base(DataType.EmailAddress)
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

            var attr = new EmailAddressAttribute();
            return attr.IsValid(value);
        }

        return false;
    }
}
