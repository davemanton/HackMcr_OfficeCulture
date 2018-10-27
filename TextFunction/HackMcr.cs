using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using OfficeCulture.Data.Models;
using OfficeCulture.Sounds.Manager;

namespace TextFunction
{
    public static class HackMcr
    {
        private static string _slackWebHook = "https://hooks.slack.com/services/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";

        [FunctionName("hackmcr")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            
            // parse query parameter
            string content = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "content", true) == 0)
                .Value;

            SendTextMessage(content);
            SendSoundTextMessage(content);

            //client.GetAsync("https://api.clockworksms.com/http/send.aspx?key=a15795bf55cf6acaf6061be7af26bbb86bc22c52&to=447950580480&content=Hello+World");

            return req.CreateResponse(HttpStatusCode.OK, content);
        }

        public static void SendTextMessage(string query)
        {
            var slackMessage = new SlackMessage
            {
                text = query
            };
            HttpClient client = new HttpClient();

            client.PostAsJsonAsync(_slackWebHook, slackMessage);
        }

        public static void SendSoundTextMessage(string query)
        {
            var soundManager = new SoundManager();
            var sound = soundManager.RunAsync(query).Result;
            
            var soundSlackMessage = new SoundSlackMessage
            {
                File = new File
                {
                    Filetype = "mp3",
                    Name = sound.Name,
                    Title = sound.Name,
                    UrlPrivate = new Uri(sound.Url),
                    Id = sound.Id.ToString(),
                    UrlPrivateDownload = new Uri(sound.Url),
                    IsExternal = true
                }
            };
            HttpClient client = new HttpClient();
            client.PostAsJsonAsync(_slackWebHook, soundSlackMessage);
        }
    }

    // DM - move into data layer
    public class SlackMessage
    {
        public string text { get; set; }
    }
        
}

