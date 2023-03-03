using System.ComponentModel.DataAnnotations;

namespace KarcagS.Shared.Attributes;

/// <summary>
/// Maximum number checked annotation
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class MaxNumberAttribute : ValidationAttribute
{
    protected long Max { get; }

    /// <summary>
    /// Add annotation
    /// </summary>
    /// <param name="max">Max value parameter</param>
    public MaxNumberAttribute(long max)
    {
        Max = max;
    }

    /// <summary>
    /// Check current value is valid or not
    /// </summary>
    /// <param name="value">Checked value</param>
    /// <param name="validationContext">Context</param>
    /// <returns>Validation result</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        try
        {
            // Ignore null values
            if (value is null)
            {
                return ValidationResult.Success;
            }

            long? number = null;

            if (value is long longValue)
            {
                number = longValue;
            }
            else if (value is int intValue)
            {
                number = intValue;
            }
            else if (value is byte byteValue)
            {
                number = byteValue;
            }
            else if (value is uint uintValue)
            {
                number = uintValue;
            }
            else if (value is sbyte sbyteValue)
            {
                number = sbyteValue;
            }

            if (number is null)
            {
                throw new InvalidCastException(GetNotSupportedNumberMessage());
            }

            // Check maximum (explicit)
            if (number > Max)
            {
                return new ValidationResult(GetMessage());
            }
        }
        catch (Exception)
        {
            return new ValidationResult(GetNotANumberMessage());
        }

        return ValidationResult.Success;
    }

    protected virtual string GetNotANumberMessage() => "Field is not a number";

    protected virtual string GetNotSupportedNumberMessage() => "Number is not supported";

    protected virtual string GetMessage() => $"Value is bigger than {Max}";
}