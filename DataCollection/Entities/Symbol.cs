using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataCollection.Entities
{
    public class Symbol
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(25)")]
        public string BaseCurrency { get; set; }
        [Column(TypeName = "varchar(25)")]
        public string Currency { get; set; }
        public double DepositMinimum { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string Description { get; set; }
        public bool ExchangeListed { get; set; }
        public bool ExchangeTraded { get; set; }
        public double MinMovement { get; set; }
        public long PriceScale { get; set; }
        public string SessionRegular { get; set; }
        public string SymbolName { get; set; }
        public string Timezone { get; set; }
        public string Type { get; set; }
        public double WithdrawMinimum { get; set; }
        public double WithdrawalFee { get; set; }
        public bool IsActive { get; set; }
    }

    [NotMapped]
    public class SymbolRequest
    {
        [JsonPropertyName("base-currency")]
        public List<string> BaseCurrency { get; set; }
        [JsonPropertyName("currency")]
        public List<string> Currency { get; set; }
        [JsonPropertyName("deposit-minimum")]
        public List<string> DepositMinimum { get; set; }
        [JsonPropertyName("description")]
        public List<string> Description { get; set; }
        [JsonPropertyName("exchange-listed")]
        public List<bool> ExchangeListed { get; set; }
        [JsonPropertyName("exchange-traded")]
        public List<bool> ExchangeTraded { get; set; }
        [JsonPropertyName("minmovement")]
        public List<string> MinMovement { get; set; }
        [JsonPropertyName("pricescale")]
        public List<int> PriceScale { get; set; }
        [JsonPropertyName("session-regular")]
        public List<string> SessionRegular { get; set; }
        [JsonPropertyName("symbol")]
        public List<string> Symbol { get; set; }
        [JsonPropertyName("timezone")]
        public List<string> Timezone { get; set; }
        [JsonPropertyName("type")]
        public List<string> Type { get; set; }
        [JsonPropertyName("withdraw-minimum")]
        public List<string> WithdrawMinimum { get; set; }
        [JsonPropertyName("withdrawal-fee")]
        public List<string> WithdrawalFee { get; set; }
    }
}
