using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TestCurrency.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            var mess = "Application-Error";
            response.Headers.Add(mess, message);
            response.Headers.Add("Access-Control-Expose-Headers", mess);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
       
    }
}
