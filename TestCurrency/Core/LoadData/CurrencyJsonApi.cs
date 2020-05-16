using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TestCurrency.Core.LoadData
{
    public class CurrencyJsonApi : ILoader
    {
        private readonly List<CurrencyLoader> _currencyList = new List<CurrencyLoader>();
        /// <summary>
        /// Gets the API.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="currencyName">Name of the currency.</param>
        /// <param name="currencyRate">The currency rate.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// Value cannot be null or whitespace. - currencyName
        /// or
        /// Value cannot be null or whitespace. - currencyRate
        /// or
        /// Value cannot be null or whitespace. - path
        /// or
        /// Value cannot be null or whitespace. - tagName
        /// </exception>
        public async Task<List<CurrencyLoader>> GetApi(string path, string tagName = "", string currencyName = "", string currencyRate = "")
        {
            if (string.IsNullOrWhiteSpace(currencyName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(currencyName));
            if (string.IsNullOrWhiteSpace(currencyRate))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(currencyRate));

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            if (string.IsNullOrWhiteSpace(tagName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tagName));
            try
            {
                await Task.Run(() => CompleteCurrenciesList(path));
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                throw;
            }
            return  _currencyList;
        }
        /// <summary>
        /// Completes the currencies list.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private List<CurrencyLoader> CompleteCurrenciesList(string path)
        {
            var jsonStr = DataStringLoader.GetDataString(path);
            if (jsonStr is null) throw new ArgumentNullException(nameof(jsonStr));
            var jsonDictionary = JObject.Parse(jsonStr).ToDictionary();

            if (jsonDictionary.Count > 0)
            {
                foreach (var (nKey, nVal) in jsonDictionary)
                {
                    if (nKey.Equals("base"))
                        _currencyList.Add(new CurrencyLoader
                        {
                            Currency = nVal.ToString(),
                            Rate = 1
                        }
                        );

                    if (!nKey.Equals("rates")) continue;
                    var objRates = nVal.ToDictionary<string>();
                    foreach (var (rKey, rVal) in objRates)
                    {
                        decimal.TryParse(rVal, out var value);
                        _currencyList.Add(new CurrencyLoader
                        {
                            Currency = rKey.ToString(),
                            Rate = value
                        }
                        );

                    }
                }


            }
            return _currencyList;
        }
    }
}
