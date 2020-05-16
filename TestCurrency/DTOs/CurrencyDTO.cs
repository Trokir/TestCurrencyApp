using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using TestCurrency.Core;
using TestCurrency.Models;

namespace TestCurrency.DTOs
{
    public class CurrencyDTO:BaseEntity
    {
        
        public CurrencyType TypeOfCurrency { get; set; }
        public decimal Count { get; set; }
        public int UserId { get; set; }
    }
}
