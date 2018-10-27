using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OfficeCulture.Sounds.Extensions;
using OfficeCulture.Sounds.Models;

namespace OfficeCulture.Sounds.Manager
{
    public class SoundManager
    {
        private static readonly HttpClient Client = new HttpClient();

        static void Main()
        {
            RunAsync("dog").GetAwaiter().GetResult();
        }

        static void ShowSound(Sound sound)
        {
            EventLog.WriteEntry("SoundLog | ", "The sound name is " + sound.Name);
        }

        static async Task<List<Sound>> GetSoundsAsync(string query)
        {
            List<Sound> sounds = null;
            HttpResponseMessage response = await Client.GetAsync($"/api/sounds?q={query}");
            if (response.IsSuccessStatusCode)
            {
                sounds = await response.Content.ReadAsJsonAsync<List<Sound>>();
            }
            return sounds;
        }

        public static async Task RunAsync(string query)
        {
            Client.BaseAddress = new Uri("https://soundy.top");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Get the sounds
                var sounds = await GetSoundsAsync(query);
                if (sounds != null)
                {
                    // Return the first sound in listing
                    ShowSound(sounds.FirstOrDefault());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}