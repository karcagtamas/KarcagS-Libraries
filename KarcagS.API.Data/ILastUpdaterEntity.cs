namespace KarcagS.API.Data;

public interface ILastUpdaterEntity<TKey>
{
    TKey LastUpdaterId { get; set; }
}