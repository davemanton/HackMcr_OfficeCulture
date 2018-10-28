using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OfficeCulture.Chuck
{
    public class QuoteClient
    {
        public async Task<string> GetQuote()
        {
            var client = new HttpClient();

            var response = await client.GetAsync("https://api.chucknorris.io/jokes/random?category=dev");

            var responseJson = await response.Content.ReadAsStringAsync();

            var quoteObject = JsonConvert.DeserializeObject<RootObject>(responseJson);

            return quoteObject.contents.quotes[0].quote;
        }
    }

    public class Success
    {
        public int total { get; set; }
    }

    public class Quote
    {
        public string quote { get; set; }
        public string length { get; set; }
        public string author { get; set; }
        public List<string> tags { get; set; }
        public string category { get; set; }
        public string date { get; set; }
        public string permalink { get; set; }
        public string title { get; set; }
        public string background { get; set; }
        public string id { get; set; }
    }

    public class Contents
    {
        public List<Quote> quotes { get; set; }
        public string copyright { get; set; }
    }

    public class RootObject
    {
        public Success success { get; set; }
        public Contents contents { get; set; }
    }
}
