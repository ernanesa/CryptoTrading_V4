using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation.Entities
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
}
