using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCurrency.Core.LoadData
{
    public interface ILoader
    {
       Task< List<CurrencyLoader>> GetApi(string path, string tagName, string currencyName, string currencyRate);
    }
}
