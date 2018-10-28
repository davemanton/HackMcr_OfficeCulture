
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OfficeCulture.Chuck
{
    public class ChuckClient
    {
        public async Task<ChuckJoke> GetJoke()
        {
            var client = new HttpClient();

            var response = await client.GetAsync("https://api.chucknorris.io/jokes/random?category=dev");

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ChuckJoke>(responseJson);
        }
    }

    public class ChuckJoke
    {
        public string[] category { get; set; }
        public string icon_url { get; set; }
        public string id { get; set; }
        public string url { get; set; }
        public string value { get; set; }
    }
}
