
using Microsoft.ML.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Recommendation.Entities
{
    public class Ticker
    {
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

    public class TickerMLInput
    {
        [LoadColumn(0)]
        [ColumnName(@"Coin")]
        public string Coin { get; set; }

        [LoadColumn(1)]
        [ColumnName(@"High")]
        public float High { get; set; }

        [LoadColumn(2)]
        [ColumnName(@"Low")]
        public float Low { get; set; }

        [LoadColumn(3)]
        [ColumnName(@"Vol")]
        public float Vol { get; set; }

        [LoadColumn(4)]
        [ColumnName(@"Last")]
        public float Last { get; set; }

        [LoadColumn(5)]
        [ColumnName(@"Buy")]
        public float Buy { get; set; }

        [LoadColumn(6)]
        [ColumnName(@"Sell")]
        public float Sell { get; set; }

        [LoadColumn(7)]
        [ColumnName(@"Open")]
        public float Open { get; set; }

        [LoadColumn(8)]
        [ColumnName(@"Date")]
        public float Date { get; set; }

    }

    public class TickerMLOutput
    {
        [ColumnName(@"Coin")]
        public float[] Coin { get; set; }

        [ColumnName(@"High")]
        public float High { get; set; }

        [ColumnName(@"Low")]
        public float Low { get; set; }

        [ColumnName(@"Vol")]
        public float Vol { get; set; }

        [ColumnName(@"Last")]
        public float Last { get; set; }

        [ColumnName(@"Buy")]
        public float Buy { get; set; }

        [ColumnName(@"Sell")]
        public float Sell { get; set; }

        [ColumnName(@"Open")]
        public float Open { get; set; }

        [ColumnName(@"Date")]
        public float Date { get; set; }

        [ColumnName(@"Features")]
        public float[] Features { get; set; }

        [ColumnName(@"Score")]
        public float Score { get; set; }
    }
}