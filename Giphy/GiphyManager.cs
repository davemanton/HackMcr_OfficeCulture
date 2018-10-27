using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Giphy
{
    public class GiphyManager
    {
        public GiphyManager()
        {
            _requestTimeout = 300000;
            _waitTimeout = 30000;
        }
        
        // Time the application wait before it re-retreive status of the request.
        private int _waitTimeout;
        public int WaitTimeout
        {
            get { return _waitTimeout; }
            set { _waitTimeout = value; }
        }

      private int _requestTimeout;

        public int RequestTimeout
        {
            get { return _requestTimeout; }
            set { _requestTimeout = value; }
        }

   
        public async Task<bool> SendGiphyRequest()
        {
            var rawGiphyUrl = new Uri("https://api.giphy.com/v1/stickers/search?q=cats&limit=1&rating=pg&offset=0&api_key=dc6zaTOxFJmzC");
            var handler = new HttpClientHandler() { AllowAutoRedirect = false, PreAuthenticate = false };

            using (HttpClient client = new HttpClient(handler))
            {
                // ***Step1 Send RawExtraction Request***
                Console.WriteLine("Sending Giphy Request");
                Console.WriteLine("Waiting for response from server...");

                // Create Http Request and set header and request content Set HttpMethod to Post request.
                var giphyRequest = new HttpRequestMessage(HttpMethod.Get, rawGiphyUrl);

                
                // Call SendAsync to get giphs
                var giphyResponse = await client.SendAsync(giphyRequest);

                Uri location = null;
                var statusResponseContent = String.Empty;
                if ((giphyResponse.StatusCode != System.Net.HttpStatusCode.OK) && (giphyResponse.StatusCode != System.Net.HttpStatusCode.Accepted))
                {
                    Console.WriteLine("Request Failed Status Code:{0} Reason:{1}", giphyResponse.StatusCode, giphyResponse.ReasonPhrase);
                    return false;
                }
                var overallRunTime = 0;
                if (giphyResponse.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    System.Console.WriteLine("Request Accepted");
                    location = giphyResponse.Headers.Location;
                    Console.WriteLine("Location: {0}", giphyResponse.Headers.Location);
                    Console.WriteLine("Polling Request status");
                    // *** Step2 Polling the status using the location provied with response from previous step.***

                }
                else //200
                {
                    statusResponseContent = await giphyResponse.Content.ReadAsStringAsync();
                }
                if (overallRunTime > RequestTimeout)
                {
                    // assume that it exit from the loop because Timeout mark this one as failed.
                    Console.WriteLine("Request Failed! the request take time to be completed and it reach Request Time out");
                    return false;
                }
                Console.WriteLine("Request completed");
               

                var objGiphy = JObject.Parse(statusResponseContent);
                
                Console.WriteLine("========================================");
                

                Console.WriteLine("========================================");
            }

            return true;
        }
    }
}
