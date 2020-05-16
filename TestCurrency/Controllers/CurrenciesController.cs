using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestCurrency.Core;
using TestCurrency.Core.LoadData;
using TestCurrency.Data;
using TestCurrency.Data.Interfaces;
using TestCurrency.DTOs;
using TestCurrency.Models;

namespace TestCurrency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyRepository _repo;
       
        public CurrenciesController(ICurrencyRepository repo)
        {
            _repo = repo;
        }



        /// <summary>
        /// Gets all currencies.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUserCurrencies")]
        public async Task<IActionResult> GetAllCurrencies(int id)
        {
            var currencies = await _repo.GetUserCurrencies(id);
            return Ok(currencies);
        }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="currencyType">Type of the currency.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">user</exception>
        [HttpGet("{id}/currency/{currencyType}")]
        public async Task<ActionResult<Currency>> GetCurrency(int id, CurrencyType currencyType)
        {
            var user = _repo.GetUserById(id).Result;
            if (user is null) throw new ArgumentNullException(nameof(user));
            var currency = await _repo.GetSpecificCurrency(id, currencyType);

            if (currency == null)
            {
                return NotFound();
            }
            return Ok(currency);
        }

        /// <summary>
        /// Withes the draw currency.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="currency">The currency.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Creating currency {currency.Id} filed on create</exception>
        [HttpPost("{userid}/withdraw")]
        public async Task<ActionResult<Currency>> WithDrawCurrency(int userid,[FromBody] CurrencyDTO currency)
        {
            if (currency is null) return NoContent();
            var user = _repo.GetUserById(userid).Result;
            if (user is null) throw new ArgumentNullException(nameof(user));
            var result =    await _repo.IfExistCurrency(userid, currency.Count, currency.TypeOfCurrency);
        if (result)
        {
            var currencyForChange=  user.Currencies.FirstOrDefault(c=>c.TypeOfCurrency.Equals(currency.TypeOfCurrency));
         if (currencyForChange != null)
             currencyForChange.Count += currency.Count;
         _repo.Update(currencyForChange); 

        }
        else
        {
            await _repo.AddNewCurrency(userid, currency.Count, currency.TypeOfCurrency);
        }

            if (await _repo.SaveAll())
            {
                return CreatedAtAction("GetCurrency", new { id = currency.Id }, currency);
            }

            throw new System.Exception($"Creating currency {currency.Id} filed on create");
        }



        /// <summary>
        /// Deposits the currency.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="currency">The currency.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Creating currency {currency.Id} filed on create</exception>
        [HttpPost("{userid}/deposit")]
        public async Task<ActionResult<Currency>> DepositCurrency(int userid,[FromBody] CurrencyDTO currency)
        {
            if (currency is null) return NoContent();
            var user = _repo.GetUserById(userid).Result;
            if (user is null) throw new ArgumentNullException(nameof(user));
            var result = await _repo.IfExistCurrency(userid, currency.Count, currency.TypeOfCurrency);
            if (result)
            {
                var currencyForChange=  user.Currencies.FirstOrDefault(c=>c.TypeOfCurrency.Equals(currency.TypeOfCurrency));
                if (currencyForChange != null)
                    if (currencyForChange.Count>=currency.Count)
                    {
                        currencyForChange.Count -= currency.Count;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(nameof(currency));
                    }
                _repo.Update(currencyForChange); 
            }
            if (await _repo.SaveAll())
            {
                return CreatedAtAction("GetCurrency", new { id = currency.Id }, currency);
            }

            throw new System.Exception($"Creating currency {currency.Id} filed on create");
        }


        // DELETE: api/Currencies/5
        [HttpDelete("{userid}/delete/{id}")]
        public async Task<ActionResult<bool>> DeleteCurrency(int userid,[FromBody] CurrencyDTO currency)
        {
            if (currency == null)
            {
                return NotFound();
            }

            _repo.Remove(await _repo.DeleteCurrency(userid, currency));
            if (await _repo.SaveAll())
            {
              return true;
            }

            throw new Exception($"Deleting currency {currency.Id} filed on delete");
        }

       
    }
}
