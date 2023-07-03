using KarcagS.API.Data.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KarcagS.API.Mongo;

public class MongoEntity : Entity<string>
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public new string Id { get; set; } = default!;
}