namespace KarcagS.API.Auth.JWT;

public interface IUser
{
    string Id { get; set; }
    string UserName { get; set; }
    string Email { get; set; }
}
