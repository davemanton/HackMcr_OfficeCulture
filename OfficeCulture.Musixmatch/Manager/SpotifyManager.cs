using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using OfficeCulture.Spotify.Extensions;
using OfficeCulture.Spotify.Model;

namespace OfficeCulture.Spotify.Manager
{
    public class SpotifyManager
    {
        private readonly HttpClient _client = new HttpClient();
        private string _spotifyToken = "29d881177c20b8a154569e98460450fb";
        private string _spotifyUrl = "https://api.spotify.com/v1/";
        private string _spotifyEndpoint = "search";
        private string _spotifyType = "artist";
       
        public async Task<Model.Spotify> GetSpotifyTaskAsync(string query)
        {

            Model.Spotify spotify = null;

           

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "BQBWRpXJKghAfPXOBAoQNjosaj-eiYJLKrk2FUkKztReNJ6cqE4x6D0xuWyY4tw8d1Y3q3XbIIgQOH6uVrlHapTt2DkBF_-U01QoCctIWMvNo6fZNaLTJjOKUTeoNCpSL0UJ1SnFoHU-vKuQBcb73G4TNS3uyw1_32Xpj-wYESBeWSlEb29JHvXHpll9d4i4V2mXd3mcz0cSzKeP5ymZKmEF7Pd2vfsb6NfLLVx1Kp_172NsW9fFlMN6Q_CR5bunqe43tkptWC-ECxXl3hiQ");
            HttpResponseMessage response = await _client.GetAsync(
                $"{_spotifyEndpoint}?q={query}&type={_spotifyType}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpResponseMessage authenticateResponse = await _client.GetAsync(
                    $"{_spotifyEndpoint}?q={query}&type={_spotifyType}");
            }

            if (response.IsSuccessStatusCode)
            {
                spotify = await response.Content.ReadAsJsonAsync<Model.Spotify>();
            }
            return spotify;
        }

        public async Task<Model.Spotify> RunAsync(string query)
        {
            _client.BaseAddress = new Uri(_spotifyUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {

                var spotify = await GetSpotifyTaskAsync(query);
                if (spotify != null)
                {
                    return spotify;
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
