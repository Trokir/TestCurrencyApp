using System.Net;

namespace TestCurrency.Core.LoadData
{
    public static class DataStringLoader
    {
        public static string GetDataString(string path)
        {
            string xmlStr;
            using (var wc = new WebClient())
            {
                xmlStr = wc.DownloadString(path);
            }

            return xmlStr;
        }
    }
}
