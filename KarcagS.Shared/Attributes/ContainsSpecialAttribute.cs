using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using KarcagS.Shared.Helpers;

namespace KarcagS.Shared.Attributes;

public partial class ContainsSpecialAttribute : ValidationAttribute, IContainsAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (ObjectHelper.IsNotNull(value) && value is string v)
        {
            var m = MyRegex().Match(v);

            return m.Success
                ? ValidationResult.Success
                : Error();
        }

        return Error();
    }

    public virtual string GetInvalidMessage() => "Does not contain any special characters.";

    private ValidationResult Error() => new(GetInvalidMessage());

    [GeneratedRegex(@"^(?=.*[-+_!@#$%^&*.,?]).+$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex MyRegex();
}