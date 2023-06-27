using KarcagS.API.Data.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace KarcagS.API.Mongo;

public class MongoEntity : Entity<string>
{

    [BsonId]
    public new string Id { get; set; } = default!;
}