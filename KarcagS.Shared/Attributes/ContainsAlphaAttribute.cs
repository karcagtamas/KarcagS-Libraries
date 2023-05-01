using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using KarcagS.Shared.Helpers;

namespace KarcagS.Shared.Attributes;

public class ContainsAlphaAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (ObjectHelper.IsNotNull(value) && value is string v)
        {
            var m = Regex.Match(v, @"^(?=.*[a-z]).+$", RegexOptions.None);

            return m.Success
                ? ValidationResult.Success
                : Error();
        }

        return Error();
    }

    protected string GetInvalidMessage() => "Does not contain any alpha characters.";

    private ValidationResult Error() => new(GetInvalidMessage());
}