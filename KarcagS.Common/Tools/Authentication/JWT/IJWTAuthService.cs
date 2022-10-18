using System.Security.Claims;

namespace KarcagS.Common.Tools.Authentication.JWT;

public interface IJWTAuthService
{
    TokenResult BuildAccessToken(IUser user, IList<string> roles);
    TokenResult BuildAccessToken(IUser user, IList<string> roles, IList<Claim> claims);
    TokenResult BuildAccessToken(IUser user, IList<string> roles, Func<IList<Claim>> claimGenerator);
    RefreshToken BuildRefreshToken();
}
