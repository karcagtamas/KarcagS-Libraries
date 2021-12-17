namespace Karcags.Common.Tools
{
    public interface IEntity
    {
        int Id { get; set; }

        bool Equals(object obj);

        string ToString();
    }
}