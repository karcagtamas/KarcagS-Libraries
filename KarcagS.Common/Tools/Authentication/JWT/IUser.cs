namespace KarcagS.Common.Tools.Authentication.JWT;

public interface IUser
{
    string Id { get; set; }
    string UserName { get; set; }
    string Email { get; set; }
}
