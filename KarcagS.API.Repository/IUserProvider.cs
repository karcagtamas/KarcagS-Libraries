namespace KarcagS.API.Repository;

public interface IUserProvider<TKey>
{
    TKey? GetCurrentUserId();
    TKey GetRequiredCurrentUserId();
}