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
        private string _giphyKey = "dc6zaTOxFJmzC";
        private string _giphyUrl = "https://api.giphy.com/v1/stickers/";
        private string _giphyEndpoint = "search";
        private string _giphyLimit = "1";
        private string _giphyRating = "pg";

        public async Task<Giphy> GetGiphyTaskAsync(string query)
        {
            Giphy objGiphy = null;
            HttpResponseMessage response = await _client.GetAsync(
                    $"{_giphyEndpoint}?q={query}&limit={_giphyLimit}&rating={_giphyRating}&api_key={_giphyKey}");
            if (response.IsSuccessStatusCode)
            {
                objGiphy = await response.Content.ReadAsJsonAsync<Giphy>();
            }
            return objGiphy;
        }

        public async Task<Giphy> RunAsync(string query)
        {
            _client.BaseAddress = new Uri(_giphyUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Get the sounds
                var objGiphy = await GetGiphyTaskAsync(query);
                if (objGiphy != null)
                {
                    // Return the first sound in listing
                    return objGiphy;
                }
            }
            catch (Exception e)
            {
                // Error
            }
            return null;
        }
        //public async Task<Model.Giphy> SendGiphyRequest()
        //{
        //    var rawGiphyUrl = new Uri("https://api.giphy.com/v1/stickers/search?q=cats&limit=1&rating=pg&offset=0&api_key=dc6zaTOxFJmzC");
        //    var handler = new HttpClientHandler() { AllowAutoRedirect = false, PreAuthenticate = false };

        //    using (HttpClient client = new HttpClient(handler))
        //    {

        //        // Create Http Request and set header and request content Set HttpMethod to Post request.
        //        var giphyRequest = new HttpRequestMessage(HttpMethod.Get, rawGiphyUrl);


        //        // Call SendAsync to get giphs
        //        var giphyResponse = await client.SendAsync(giphyRequest);

        //        Uri location = null;
        //        var statusResponseContent = String.Empty;
        //        if ((giphyResponse.StatusCode != System.Net.HttpStatusCode.OK) && (giphyResponse.StatusCode != System.Net.HttpStatusCode.Accepted))
        //        {
        //            Console.WriteLine("Request Failed Status Code:{0} Reason:{1}", giphyResponse.StatusCode, giphyResponse.ReasonPhrase);
        //            return false;
        //        }
        //        var overallRunTime = 0;
        //        if (giphyResponse.StatusCode == System.Net.HttpStatusCode.Accepted)
        //        {
        //            System.Console.WriteLine("Request Accepted");
        //            location = giphyResponse.Headers.Location;
        //            Console.WriteLine("Location: {0}", giphyResponse.Headers.Location);
        //            Console.WriteLine("Polling Request status");
        //            // *** Step2 Polling the status using the location provied with response from previous step.***

        //        }
        //        else //200
        //        {
        //            statusResponseContent = await giphyResponse.Content.ReadAsStringAsync();
        //        }

        //        Console.WriteLine("Request completed");


        //        var objGiphy = JObject.Parse(statusResponseContent);

        //        Console.WriteLine("========================================");


        //        Console.WriteLine("========================================");
        //    }

        //    return true;
        //}
    }
}
