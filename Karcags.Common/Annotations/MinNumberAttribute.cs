using System;
using System.ComponentModel.DataAnnotations;

namespace Karcags.Common.Annotations;

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
            // Try convert to nullable int
            var number = (long?)value;

            // Ignore null values
            if (number is null)
            {
                return ValidationResult.Success;
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
