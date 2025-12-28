using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using KarcagS.Shared.Helpers;

namespace KarcagS.Shared.Attributes;

public partial class ContainsCapitalAlphaAttribute : ValidationAttribute, IContainsAttribute
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

    public string GetInvalidMessage() => "Does not contain any capital alpha characters.";

    private ValidationResult Error() => new(GetInvalidMessage());

    [GeneratedRegex(@"^(?=.*[A-Z]).+$", RegexOptions.None)]
    private static partial Regex MyRegex();
}