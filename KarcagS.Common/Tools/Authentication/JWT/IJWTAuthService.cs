using System.Security.Claims;

namespace KarcagS.Common.Tools.Authentication.JWT;

public interface IJWTAuthService
{
    string BuildAccessToken(IUser user, IList<string> roles);
    string BuildAccessToken(IUser user, IList<string> roles, IList<Claim> claims);
    string BuildAccessToken(IUser user, IList<string> roles, Func<IList<Claim>> claimGenerator);
    RefreshToken BuildRefreshToken();
}
