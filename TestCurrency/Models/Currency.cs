
using TestCurrency.Core;

namespace TestCurrency.Models
{
    public class Currency: BaseEntity
    {
        public CurrencyType TypeOfCurrency { get; set; }
        public decimal Count { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
