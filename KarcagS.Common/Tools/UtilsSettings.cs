using System.Security.Claims;

namespace KarcagS.Common.Tools;

public class UtilsSettings
{
    public string UserIdClaimName { get; set; } = ClaimTypes.NameIdentifier;
    public string UserEmailClaimName { get; set; } = ClaimTypes.Email;
    public string UserNameClaimName { get; set; } = ClaimTypes.Name;
}