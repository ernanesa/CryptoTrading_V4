using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataCollection.Entities
{
    public class Ticker
    {
        public Ticker() { }

        public Ticker(TickerRequest tickerRequest)
        {
            Buy = decimal.Parse(tickerRequest.Buy!);
            Date = tickerRequest.Date;
            High = decimal.Parse(tickerRequest.High!);
            Last = decimal.Parse(tickerRequest.Last!);
            Low = decimal.Parse(tickerRequest.Low!);
            Open = decimal.Parse(tickerRequest.Open!);
            Pair = tickerRequest.Pair!;
            Sell = decimal.Parse(tickerRequest.Sell!);
            Volume = decimal.Parse(tickerRequest.Volume!);
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("buy")]
        public decimal Buy { get; set; }

        [BsonElement("date")]
        public long Date { get; set; }

        [BsonElement("high")]
        public decimal High { get; set; }

        [BsonElement("last")]
        public decimal Last { get; set; }

        [BsonElement("low")]
        public decimal Low { get; set; }

        [BsonElement("open")]
        public decimal Open { get; set; }

        [BsonElement("pair")]
        public string Pair { get; set; }

        [BsonElement("sell")]
        public decimal Sell { get; set; }

        [BsonElement("vol")]
        public decimal Volume { get; set; }
    }

    public class TickerRequest
    {
        [JsonPropertyName("buy")]
        public string Buy { get; set; }
        [JsonPropertyName("date")]
        public long Date { get; set; }
        [JsonPropertyName("high")]
        public string High { get; set; }
        [JsonPropertyName("last")]
        public string Last { get; set; }
        [JsonPropertyName("low")]
        public string Low { get; set; }
        [JsonPropertyName("open")]
        public string Open { get; set; }
        [JsonPropertyName("pair")]
        public string Pair { get; set; }
        [JsonPropertyName("sell")]
        public string Sell { get; set; }
        [JsonPropertyName("vol")]
        public string Volume { get; set; }
    }
}