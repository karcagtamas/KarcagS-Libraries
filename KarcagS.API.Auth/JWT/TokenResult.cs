namespace KarcagS.API.Auth.JWT;

public class TokenResult
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
}
