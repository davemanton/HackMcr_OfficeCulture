using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace GiphyFunction
{
  
    public class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowSticker(Sticker Sticker)
        {
            Console.WriteLine($"Name: {Sticker.Name}\tPrice: " +
                $"{Sticker.Price}\tCategory: {Sticker.Category}");
        }

        static async Task<Uri> CreateStickerAsync(Sticker Sticker)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/Stickers", Sticker);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<Sticker> GetStickerAsync(string path)
        {
            Sticker Sticker = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                Sticker = await response.Content.ReadAsAsync<Sticker>();
            }
            return Sticker;
        }

        static async Task<Sticker> UpdateStickerAsync(Sticker Sticker)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/Stickers/{Sticker.Id}", Sticker);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated Sticker from the response body.
            Sticker = await response.Content.ReadAsAsync<Sticker>();
            return Sticker;
        }

        static async Task<HttpStatusCode> DeleteStickerAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/Stickers/{id}");
            return response.StatusCode;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("//api.giphy.com/v1/stickers/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new Sticker
                //Sticker Sticker = new Sticker
                //{
                //    Name = "Gizmo",
                //    Price = 100,
                //    Category = "Widgets"
                //};

                //var url = await CreateStickerAsync(Sticker);
                //Console.WriteLine($"Created at {url}");

                //// Get the Sticker
                //Sticker = await GetStickerAsync(url.PathAndQuery);
                //ShowSticker(Sticker);

              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
