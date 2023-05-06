using KarcagS.Shared.Common;

namespace KarcagS.API.Data;

public interface IEntity<T> : IIdentified<T>
{
    bool Equals(object obj);

    string? ToString();
}
