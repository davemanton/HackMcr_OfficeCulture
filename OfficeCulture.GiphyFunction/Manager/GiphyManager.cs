using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using OfficeCulture.GiphyFunction.Extensions;
using OfficeCulture.GiphyFunction.Model;

namespace OfficeCulture.GiphyFunction.Manager
{
    public class GiphyManager
    {
        private readonly HttpClient _client = new HttpClient();
        private string _giphyKey = "2dpYagXIKeSFSCNP3uIBoLlVAdHsseJ1";
        private string _giphyUrl = "https://api.giphy.com/v1/giphy/";
        private string _giphyEndpoint = "search";
        private string _giphyLimit = "1";
        private string _giphyRating = "pg";

        public async Task<Giphy> GetGiphyTaskAsync(string query)
        {
            Giphy giphy = null;
            HttpResponseMessage response = await _client.GetAsync(
                    $"{_giphyEndpoint}?q={query}&limit={_giphyLimit}&rating={_giphyRating}&api_key={_giphyKey}");
            if (response.IsSuccessStatusCode)
            {
                giphy = await response.Content.ReadAsJsonAsync<Giphy>();
            }
            return giphy;
        }

        public async Task<Giphy> RunAsync(string query)
        {
            _client.BaseAddress = new Uri(_giphyUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
              
                var giphy = await GetGiphyTaskAsync(query);
                if (giphy != null)
                {
                    return giphy;
                }
            }
            catch (Exception e)
            {
                // Error
            }
            return null;
        }
    }
}
