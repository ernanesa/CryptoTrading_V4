using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Trading.Entities;
public class Balance
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [JsonPropertyName("symbol")]
    [BsonElement("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("available")]
    [BsonElement("available")]
    public string Available { get; set; }

    [JsonPropertyName("on_hold")]
    [BsonElement("on_hold")]
    public string OnHold { get; set; }

    [JsonPropertyName("total")]
    [BsonElement("total")]
    public string Total { get; set; }
}

