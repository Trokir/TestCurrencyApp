using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestCurrency.Core;
using TestCurrency.Data.Interfaces;
using TestCurrency.DTOs;
using TestCurrency.Models;

namespace TestCurrency.Data.Repos
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        private readonly DataContext _context;
        public CurrencyRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public async Task<User> GetUserById(int userid)
        {
            if (userid <= 0) throw new ArgumentOutOfRangeException(nameof(userid));
            var user = await _context.Set<User>().FindAsync(userid);
            if (user is null) throw new ArgumentNullException(nameof(user));
            return user;
        }

        /// <summary>
        /// Adds the new currency.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="count">The count.</param>
        /// <param name="currencyValueByName">Name of the currency value by.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// id
        /// or
        /// count
        /// or
        /// currencyValueByName
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// User
        /// or
        /// user
        /// </exception>
        public async Task<Currency> AddNewCurrency(int id, decimal count, CurrencyType currencyValueByName)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (currencyValueByName <= 0) throw new ArgumentOutOfRangeException(nameof(currencyValueByName));
            var user = await _context.Set<User>().FindAsync(id);
            if (user == null || user.Currencies.Count < 0) throw new ArgumentNullException(nameof(User));

            var newCurrency = new Currency
            {
                TypeOfCurrency = currencyValueByName,
                Count = count,
                UserId = id
            };
            user.Currencies.Add(newCurrency);
            return newCurrency;

            throw new ArgumentNullException(nameof(user));
        }

        /// <summary>
        /// Gets the user currencies.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">User</exception>
        public async Task<IEnumerable<Currency>> GetUserCurrencies(int id)
        {
            var user = await _context.Set<User>().FindAsync(id);
            if (user != null)
            {
                return user.Currencies;
            }
            throw new ArgumentNullException(nameof(user));
        }

        /// <summary>
        /// Gets the specific currency.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="currencyValueByName">Name of the currency value by.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">currencyValueByName</exception>
        /// <exception cref="ArgumentNullException">User</exception>
        public async Task<Currency> GetSpecificCurrency(int id, CurrencyType currencyValueByName)
        {
            if (currencyValueByName <= 0) throw new ArgumentOutOfRangeException(nameof(currencyValueByName));
            var user = await _context.Set<User>().FindAsync(id);
            if (user != null && user.Currencies.Count > 0 && Enum.IsDefined(typeof(CurrencyType), currencyValueByName))
            {
                //   var enumName = Enum.GetName(typeof(CurrencyType), currencyName)?.ToUpper();
                return user.Currencies.FirstOrDefault(c => c.TypeOfCurrency == currencyValueByName);
            }
            throw new ArgumentNullException(nameof(User));
        }
        /// <summary>
        /// Ifs the exist currency.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="count">The count.</param>
        /// <param name="currencyValueByName">Name of the currency value by.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// id
        /// or
        /// count
        /// or
        /// currencyValueByName
        /// </exception>
        /// <exception cref="ArgumentNullException">User</exception>
        public async Task<bool> IfExistCurrency(int id, decimal count, CurrencyType currencyValueByName)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (currencyValueByName <= 0) throw new ArgumentOutOfRangeException(nameof(currencyValueByName));
            var user = await _context.Set<User>().FindAsync(id);
            if (user == null || user.Currencies.Count <= 0) throw new ArgumentNullException(nameof(User));
            if (user.Currencies.Any(x => x.TypeOfCurrency == currencyValueByName))
            
                return true;
            return false;

        }
        /// <summary>
        /// Deletes the currency.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="currency">The currency.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// currency
        /// or
        /// user
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">userid</exception>
        public async Task<Currency> DeleteCurrency(int userid,CurrencyDTO currency)
        {
            if (currency is null) throw new ArgumentNullException(nameof(currency));
            if (userid <= 0) throw new ArgumentOutOfRangeException(nameof(userid));
         
            var user = await _context.Set<User>().FindAsync(userid);
            if (user == null) throw new ArgumentNullException(nameof(user));
            var currencyForDelete = user.Currencies.FirstOrDefault(c => c.TypeOfCurrency == currency.TypeOfCurrency);
          
            return currencyForDelete;
        }
    }
}
