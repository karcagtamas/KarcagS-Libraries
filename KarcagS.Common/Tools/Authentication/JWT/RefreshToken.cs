namespace KarcagS.Common.Tools.Authentication.JWT;

public class RefreshToken
{
    public string Token { get; set; } = default!;
    public DateTime Expires { get; set; }
    public DateTime Creation { get; set; }
}
