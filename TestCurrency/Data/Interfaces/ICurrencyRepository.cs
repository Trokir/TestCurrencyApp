
using System.Collections.Generic;
using System.Threading.Tasks;
using TestCurrency.Core;
using TestCurrency.DTOs;
using TestCurrency.Models;

namespace TestCurrency.Data.Interfaces
{
  public  interface ICurrencyRepository:IAsyncRepository<Currency>
  {
      Task<User> GetUserById(int userid);
      Task<IEnumerable<Currency>> GetUserCurrencies(int id);
      Task<Currency> GetSpecificCurrency(int id, CurrencyType currencyValueByName);
     
      Task<bool> IfExistCurrency(int id, decimal count, CurrencyType currencyValueByName);
      Task<Currency> AddNewCurrency(int id, decimal count, CurrencyType currencyValueByName);

      Task<Currency> DeleteCurrency(int userid, CurrencyDTO currency);
  }
}
