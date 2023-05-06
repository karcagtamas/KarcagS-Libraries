using System.Security.Claims;
using KarcagS.API.Auth.JWT.Models;

namespace KarcagS.API.Auth.JWT;

public interface IJWTAuthService
{
    TokenResult BuildAccessToken(IUser user, IList<string> roles);
    TokenResult BuildAccessToken(IUser user, IList<string> roles, IList<Claim> claims);
    TokenResult BuildAccessToken(IUser user, IList<string> roles, Func<IList<Claim>> claimGenerator);
    RefreshToken BuildRefreshToken();
}