using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace TestCurrency.Core.LoadData
{
    public class CurrencyXmlApi : ILoader
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
        public async Task<List<CurrencyLoader>> GetApi(string path="", string tagName="", string currencyName="", string currencyRate="")
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
                 await Task.Run(() => CompleteCurrenciesList(path,tagName,currencyName,currencyRate));
              
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
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="currencyName">Name of the currency.</param>
        /// <param name="currencyRate">The currency rate.</param>
        /// <returns></returns>
        private List<CurrencyLoader> CompleteCurrenciesList(string path ,string tagName, string currencyName, string currencyRate)
        {
            var xmlStr = DataStringLoader.GetDataString(path);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            var nList = xmlDoc.GetElementsByTagName(tagName);
            if (nList.Count == 0) return _currencyList;
            foreach (XmlNode node in nList)
            {
                var currAttr = node.Attributes.GetNamedItem(currencyName);
                var rateAttr = node.Attributes.GetNamedItem(currencyRate);
                if (currAttr == null || rateAttr == null) continue;
                decimal.TryParse(rateAttr.Value, out var value);
                _currencyList.Add
                (
                    new CurrencyLoader
                    {
                        Currency = currAttr.Value,
                        Rate = value
                    }
                );
            }
            return  _currencyList;
        }
        
    }
}
