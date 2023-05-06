using KarcagS.Shared.Common;

namespace KarcagS.API.Data.Entities;

public interface IEntity<T> : IIdentified<T>
{
    bool Equals(object obj);

    string? ToString();
}
