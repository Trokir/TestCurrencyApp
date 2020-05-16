using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestCurrency.Core
{
    public static class JsonConversionExtensions
    {

        public static IDictionary<string, TValue> ToDictionary<TValue>(this object obj)
        {       
            var json = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json);   
            return dictionary;
        }


        /// <summary>
        /// Converts to dictionary.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this JObject json)
        {
            var propertyValuePairs = json.ToObject<Dictionary<string, object>>();
            ProcessJObjectProperties(propertyValuePairs);
            ProcessJArrayProperties(propertyValuePairs);
            return propertyValuePairs;
        }
        /// <summary>
        /// Processes the j object properties.
        /// </summary>
        /// <param name="propertyValuePairs">The property value pairs.</param>
        private static void ProcessJObjectProperties(IDictionary<string, object> propertyValuePairs)
        {
            var objectPropertyNames = (from property in propertyValuePairs
                let propertyName = property.Key
                let value = property.Value
                where value is JObject
                select propertyName).ToList();

            objectPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToDictionary((JObject) propertyValuePairs[propertyName]));
        }
        /// <summary>
        /// Processes the j array properties.
        /// </summary>
        /// <param name="propertyValuePairs">The property value pairs.</param>
        private static void ProcessJArrayProperties(IDictionary<string, object> propertyValuePairs)
        {
            var arrayPropertyNames = (from property in propertyValuePairs
                let propertyName = property.Key
                let value = property.Value
                where value is JArray
                select propertyName).ToList();

            arrayPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToArray((JArray) propertyValuePairs[propertyName]));
        }

        private static object[] ToArray(this JArray array)
        {
            return array.ToObject<object[]>().Select(ProcessArrayEntry).ToArray();
        }
        /// <summary>
        /// Processes the array entry.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static object ProcessArrayEntry(object value)
        {
            if (value is JObject jObject) return ToDictionary(jObject);
            if (value is JArray array)
            {
                return ToArray(array);
            }

            return value;

        }
    }
}
