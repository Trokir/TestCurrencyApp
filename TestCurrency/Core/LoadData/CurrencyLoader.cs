using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCurrency.Core.LoadData
{
    public class CurrencyLoader:ICurrencyLoader
    {
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        private readonly string _path;
        private readonly ILoader _loader;

        public CurrencyLoader(){}
        
        public CurrencyLoader(ApiType type)
        {
            switch (type)
            {
                case ApiType.Json:
                    _path = "https://api.exchangeratesapi.io/latest";
                    _loader = new CurrencyJsonApi();
                    break;

                case ApiType.Xml:
                    _path = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
                    _loader = new CurrencyXmlApi();
                    break;
            }
        }
        /// <summary>
        /// Gets the currencies list.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="currencyName">Name of the currency.</param>
        /// <param name="currencyRate">The currency rate.</param>
        /// <returns></returns>
        public List<CurrencyLoader> GetCurrenciesList(string tagName, string currencyName, string currencyRate)
        {
            return _loader.GetApi(_path, tagName, currencyName, currencyRate).Result;
        }
    }
}
