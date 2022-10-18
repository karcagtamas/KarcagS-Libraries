namespace KarcagS.Common.Tools.Authentication.JWT;

public class TokenResult
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
}
