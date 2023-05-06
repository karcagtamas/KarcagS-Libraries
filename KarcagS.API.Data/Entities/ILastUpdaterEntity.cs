namespace KarcagS.API.Data.Entities;

public interface ILastUpdaterEntity<TKey>
{
    TKey LastUpdaterId { get; set; }
}