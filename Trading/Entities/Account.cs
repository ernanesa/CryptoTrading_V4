using System.Text.Json.Serialization;

namespace Trading.Entities;
public class Account
{
    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("currencySign")]
    public string CurrencySign { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}