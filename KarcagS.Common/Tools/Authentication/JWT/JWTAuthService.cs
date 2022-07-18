using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KarcagS.Common.Tools.Authentication.JWT;

public class JWTAuthService : IJWTAuthService
{
    private readonly JWTConfiguration jwtConfigurations;

    public JWTAuthService(IOptions<JWTConfiguration> jwtOptions)
    {
        jwtConfigurations = jwtOptions.Value;
    }

    public string BuildAccessToken<T>(IUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
        claims.AddRange(roleClaims);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigurations.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new JwtSecurityToken(jwtConfigurations.Issuer, jwtConfigurations.Issuer, claims,
            expires: DateTime.Now.AddMinutes(jwtConfigurations.ExpirationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public RefreshToken BuildRefreshToken()
    {
        var randomNumber = new byte[32];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        return new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            Expires = DateTime.Now.AddMinutes(jwtConfigurations.RefreshTokenExpirationInMinutes),
            Creation = DateTime.Now
        };
    }
}
