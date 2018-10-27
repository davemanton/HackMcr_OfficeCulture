using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Giphy
{
    public class Program
    {
       
        public static void Main(string[] args)
        {
            try
            {
                GiphyManager mgr = new GiphyManager();
                

               
                var giphyRequestContent = "";
               
              
                System.Console.WriteLine(giphyRequestContent);
                if (mgr.SendGiphyRequest().GetAwaiter().GetResult())
                    System.Console.WriteLine("Request Completed Successful");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

    }
}
