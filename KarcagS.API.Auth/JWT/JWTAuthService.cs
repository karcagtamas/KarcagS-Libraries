using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KarcagS.API.Auth.JWT.Configurations;
using KarcagS.API.Auth.JWT.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace KarcagS.API.Auth.JWT;

public class JWTAuthService : IJWTAuthService
{
    private readonly JWTConfiguration jwtConfigurations;

    public JWTAuthService(IOptions<JWTConfiguration> jwtOptions)
    {
        jwtConfigurations = jwtOptions.Value;
    }

    public TokenResult BuildAccessToken(IUser user, IList<string> roles) => BuildAccessToken(user, roles, () => new List<Claim>());

    public TokenResult BuildAccessToken(IUser user, IList<string> roles, IList<Claim> claims) => BuildAccessToken(user, roles, () => claims);

    public TokenResult BuildAccessToken(IUser user, IList<string> roles, Func<IList<Claim>> claimGenerator)
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

        claims.AddRange(claimGenerator());

        var expiration = DateTime.Now.AddMinutes(jwtConfigurations.ExpirationInMinutes);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigurations.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new JwtSecurityToken(jwtConfigurations.Issuer, jwtConfigurations.Issuer, claims,
            expires: expiration,
            signingCredentials: credentials);

        return new TokenResult { Token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor), Expiration = expiration };
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