namespace KarcagS.API.Auth.JWT.Configurations;

public class JWTConfiguration
{
    public string Secret { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ExpirationInMinutes { get; set; } = 60;

    public int RefreshTokenExpirationInMinutes { get; set; } = 3 * 24 * 60;
}
