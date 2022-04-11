namespace KarcagS.Common.Tools.Entities;

public interface IEntity<T>
{
    T Id { get; set; }

    bool Equals(object obj);

    string? ToString();
}
