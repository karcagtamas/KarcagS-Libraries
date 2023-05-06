namespace KarcagS.API.Auth.JWT.Models;

public interface IUser
{
    string Id { get; set; }
    string UserName { get; set; }
    string Email { get; set; }
}
