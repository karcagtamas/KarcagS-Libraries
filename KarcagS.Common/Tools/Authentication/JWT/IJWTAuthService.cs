namespace KarcagS.Common.Tools.Authentication.JWT;

public interface IJWTAuthService
{
    string BuildAccessToken<T>(IUser user, IList<string> roles);
    RefreshToken BuildRefreshToken();
}
