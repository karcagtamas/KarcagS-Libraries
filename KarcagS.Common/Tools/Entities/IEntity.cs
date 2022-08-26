using KarcagS.Shared.Common;

namespace KarcagS.Common.Tools.Entities;

public interface IEntity<T> : IIdentified<T>
{
    bool Equals(object obj);

    string? ToString();
}
