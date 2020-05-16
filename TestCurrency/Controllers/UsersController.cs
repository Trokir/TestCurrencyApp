using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestCurrency.Core;
using TestCurrency.Core.LoadData;
using TestCurrency.Data.Interfaces;
using TestCurrency.Models;

namespace TestCurrency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IEnumerable<CurrencyLoader> _currencies;
        public UsersController(IUserRepository repo)
        {
            _repo = repo;
            ICurrencyLoader loader = new CurrencyLoader(ApiType.Json);
            if (_currencies is null)
                _currencies = loader.GetCurrenciesList("Cube", "currency", "rate").ToList();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repo.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetById(id);

            return Ok(user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(User userForUpdate)
        {

            _repo.Update(userForUpdate);

            if (await _repo.SaveAll())
                return Ok(userForUpdate);

            throw new System.Exception($"Updating user {userForUpdate.Id} filed on save");
        }
        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="createdUser">The created user.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Error creating the User</exception>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User createdUser)
        {
            if (createdUser is null)
                return BadRequest();


            _repo.Add(createdUser);
            if (await _repo.SaveAll())
            {
                return CreatedAtAction(
                    nameof(GetAllUsers),
                    new { id = createdUser.Id },
                    createdUser);
            }

            throw new System.Exception("Error creating the User");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var deletedUser = await _repo.GetById(id);
            if (deletedUser == null)
            {
                return NotFound();
            }
            _repo.Remove(deletedUser);
            if (await _repo.SaveAll())
            {
                return deletedUser;
            }
            throw new System.Exception("Error deleting the User");
        }


        [HttpPut("{id}/convert/{amount}/{fromCurrencyType}/to/{toCurrencyType}")]
        public async Task<ActionResult<User>> ConvertUserCurrency(int id, decimal amount, CurrencyType fromCurrencyType,
           CurrencyType toCurrencyType)
        {
            #region Check out arguments
            if ((id <= 0) || (amount <= 0) ||
                (!Enum.IsDefined(typeof(CurrencyType), fromCurrencyType)) ||
                (!Enum.IsDefined(typeof(CurrencyType), toCurrencyType)))
                return BadRequest("Any value is incorrect");
            #endregion

            var user = await _repo.GetById(id);
            if (user is null) throw new ArgumentNullException(nameof(user));

            var currencyFromConvert = user.Currencies
                .FirstOrDefault(currency => currency.TypeOfCurrency.Equals(fromCurrencyType));
            var currencyToConvert = user.Currencies
                .FirstOrDefault(currency => currency.TypeOfCurrency.Equals(toCurrencyType));
            // If user has not enough money

            if (currencyFromConvert == null) return NoContent();
            if (currencyFromConvert.Count < amount)
                return BadRequest("You have not enough money");

            // get comparing Currency name
            var fromCurrencyTypeValue = fromCurrencyType.ToString().ToUpper();
            var toCurrencyTypeValue = toCurrencyType.ToString().ToUpper();

            // If Public API has Converting currencies rates
            if (_currencies.Any(c => c.Currency.Equals(fromCurrencyTypeValue))
                && _currencies.Any(c => c.Currency.Equals(fromCurrencyTypeValue)))
            {
                user = ExecuteTransaction(amount, toCurrencyType, fromCurrencyTypeValue,
                    currencyToConvert, toCurrencyTypeValue, user, currencyFromConvert);
                _repo.Update(user);

                if (await _repo.SaveAll())
                    return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            else
            {
                return NoContent();
            }

            return NoContent();
        }



        #region Advansed Methods
          /// <summary>
        /// Executes the transaction.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="toCurrencyType">Type of to currency.</param>
        /// <param name="fromCurrencyTypeValue">From currency type value.</param>
        /// <param name="currencyToConvert">The currency to convert.</param>
        /// <param name="toCurrencyTypeValue">To currency type value.</param>
        /// <param name="user">The user.</param>
        /// <param name="currencyFromConvert">The currency from convert.</param>
        /// <returns></returns>
        private User ExecuteTransaction(decimal amount, CurrencyType toCurrencyType, string fromCurrencyTypeValue,
            Currency currencyToConvert, string toCurrencyTypeValue, User user, Currency currencyFromConvert)
        {
            //get currency rate
            var fromCurrencyTypeRate = _currencies.Single(r => r.Currency != null
                                                               && r.Currency.Equals(fromCurrencyTypeValue)).Rate;
            // new rate for convert
            var toCurrencyTypeRate = 0M;
            //new amount for transaction
            var convertedAmount = 0M;
            // If user has to Converting currency
            if (currencyToConvert != null)
            {
                toCurrencyTypeRate = _currencies.Single(r => r != null
                                                             && r.Currency.Equals(toCurrencyTypeValue)).Rate;
                //Get Converted Value
                convertedAmount = (amount * toCurrencyTypeRate) / fromCurrencyTypeRate;
                user = StartCompleteTransaction(user, currencyFromConvert,
                    currencyToConvert, amount, convertedAmount);
            }
            else
            {
                toCurrencyTypeRate = _currencies.Single(r => r != null
                                                             && r.Currency.Equals(toCurrencyType.ToString()
                                                                 .ToUpper())).Rate;
                convertedAmount = (amount * toCurrencyTypeRate) / fromCurrencyTypeRate;
                user = StartUnCompleteTransaction(user, currencyFromConvert,
                    toCurrencyType, amount, convertedAmount);
            }

            return user;
        }

        /// <summary>
        /// Starts the un complete transaction. with creating new Currency
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="currencyFromConvert">The currency from convert.</param>
        /// <param name="toCurrencyType">Type of to currency.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="convertedAmount">The converted amount.</param>
        /// <returns></returns>
        private User StartUnCompleteTransaction(User user, Currency currencyFromConvert,
            CurrencyType toCurrencyType, in decimal amount, in decimal convertedAmount)
        {
            currencyFromConvert.Count -= amount;
            user.Currencies.Add(
                new Currency
                {
                    TypeOfCurrency = toCurrencyType,
                    Count = convertedAmount,
                    User = user,
                    UserId = user.Id
                }
                );

            return user;
        }

        /// <summary>
        /// Starts the complete transaction.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="currencyFromConvert">The currency from convert.</param>
        /// <param name="currencyToConvert">The currency to convert.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="convertedAmount">The converted amount.</param>
        /// <returns></returns>
        private User StartCompleteTransaction(User user, Currency currencyFromConvert,
            Currency currencyToConvert, in decimal amount, in decimal convertedAmount)
        {
            currencyFromConvert.Count -= amount;
            currencyToConvert.Count += convertedAmount;
            return user;
        }
        

        #endregion

      
    }
}
