namespace KarcagS.API.Auth.JWT.Models;

public class TokenResult
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
}
