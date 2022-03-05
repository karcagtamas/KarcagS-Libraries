using System.ComponentModel.DataAnnotations;

namespace KarcagS.Shared.Attributes;

/// <summary>
/// Minimum number checked annotation
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class MinNumberAttribute : ValidationAttribute
{
    private long Min { get; }

    /// <summary>
    /// Add annotation
    /// </summary>
    /// <param name="min">Min value parameter</param>
    public MinNumberAttribute(long min)
    {
        Min = min;
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

            if (value is long)
            {
                number = (long)value;
            }
            else if (value is int)
            {
                number = (int)value;
            }
            else if (value is byte)
            {
                number = (byte)value;
            }
            else if (value is uint)
            {
                number = (uint)value;
            }
            else if (value is sbyte)
            {
                number = (sbyte)value;
            }

            if (number is null)
            {
                throw new InvalidCastException("Number is not supported");
            }

            // Check minimum (explicit)
            if (number < Min)
            {
                return new ValidationResult($"Value is less than {Min}");
            }
        }
        catch (Exception)
        {
            return new ValidationResult("Field is not a number");
        }

        return ValidationResult.Success;
    }
}
