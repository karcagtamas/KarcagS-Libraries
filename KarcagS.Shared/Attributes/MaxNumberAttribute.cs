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

            long? number = value switch
            {
                long longValue => longValue,
                int intValue => intValue,
                byte byteValue => byteValue,
                uint uintValue => uintValue,
                sbyte sbyteValue => sbyteValue,
                _ => null
            };

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