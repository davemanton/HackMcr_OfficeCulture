using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OfficeCulture.Chuck.Coin
{
    public class BitcoinClient
    {
        public async Task<string> GetPrice()
        {
            var client = new HttpClient();

            var response = await client.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");

            var responseJson = await response.Content.ReadAsStringAsync();

            var coinData = JsonConvert.DeserializeObject<RootObject>(responseJson);

            return coinData.bpi.GBP.rate;
        }
    }
        public class Time
        {
            public string updated { get; set; }
            public DateTime updatedISO { get; set; }
            public string updateduk { get; set; }
        }

        public class USD
        {
            public string code { get; set; }
            public string symbol { get; set; }
            public string rate { get; set; }
            public string description { get; set; }
            public double rate_float { get; set; }
        }

        public class GBP
        {
            public string code { get; set; }
            public string symbol { get; set; }
            public string rate { get; set; }
            public string description { get; set; }
            public double rate_float { get; set; }
        }

        public class EUR
        {
            public string code { get; set; }
            public string symbol { get; set; }
            public string rate { get; set; }
            public string description { get; set; }
            public double rate_float { get; set; }
        }

        public class Bpi
        {
            public USD USD { get; set; }
            public GBP GBP { get; set; }
            public EUR EUR { get; set; }
        }

        public class RootObject
        {
            public Time time { get; set; }
            public string disclaimer { get; set; }
            public string chartName { get; set; }
            public Bpi bpi { get; set; }
        }
    
}
