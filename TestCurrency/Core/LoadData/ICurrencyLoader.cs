using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestCurrency.Core.LoadData
{
    public interface ICurrencyLoader
    {
       List<CurrencyLoader> GetCurrenciesList(string tagName, string currencyName, string currencyRate);
    }
}
