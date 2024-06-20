using System.Text.Json.Serialization;

namespace Trading.Entities;

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
