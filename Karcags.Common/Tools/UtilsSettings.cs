using System.Security.Claims;

namespace Karcags.Common.Tools;

public class UtilsSettings
{
    public string UserIdClaimName { get; set; } = ClaimTypes.NameIdentifier;
    public string UserEmailClaimName { get; set; } = ClaimTypes.Email;
    public string UserNameClaimName { get; set; } = ClaimTypes.Name;
}