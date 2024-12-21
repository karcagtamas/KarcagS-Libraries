using System.ComponentModel.DataAnnotations;
using KarcagS.Shared.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace KarcagS.API.Data.Entities;

public abstract class Entity<T> : IIdentified<T>
{
    [Key]
    [Required]
    [BsonIgnore]
    public abstract T Id { get; set; }

    public override string ToString() => $"[Id = {Id}]";

    public override int GetHashCode() => Id!.GetHashCode();

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            Entity<T> { Id: not null } entity => entity.Id.Equals(Id),
            _ => false
        };
    }
}