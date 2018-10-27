using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OfficeCulture.Sounds.Extensions;
using OfficeCulture.Sounds.Model;

namespace OfficeCulture.Sounds.Manager
{
    public class SoundManager
    {
        private readonly HttpClient _client = new HttpClient();
        
        public async Task<List<Sound>> GetSoundsAsync(string query)
        {
            List<Sound> sounds = null;
            HttpResponseMessage response = await _client.GetAsync($"/api/sounds?q={query}");
            if (response.IsSuccessStatusCode)
            {
                sounds = await response.Content.ReadAsJsonAsync<List<Sound>>();
            }
            return sounds;
        }

        public async Task<Sound> RunAsync(string query)
        {
            _client.BaseAddress = new Uri("https://soundy.top");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Get the sounds
                var sounds = await GetSoundsAsync(query);
                if (sounds != null)
                {
                    // Return the first sound in listing
                    return sounds.FirstOrDefault();
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