using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OfficeCulture.Luis
{
    public class LuisClient
    {
        public async Task<LuisData> AnalyseText(string text)
        {
            var client = new HttpClient();

            var response = await client.GetAsync($"https://westeurope.api.cognitive.microsoft.com/luis/v2.0/apps/f2acdce8-430a-426d-bdde-aa0d47092f5c?subscription-key=12b47e8ab1354c41b50e155ebace45a9&timezoneOffset=60&q={text}");

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<LuisData>(json);
        }
    }
}
