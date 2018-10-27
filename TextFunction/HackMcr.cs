using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using OfficeCulture.Sounds.Manager;

namespace TextFunction
{
    public static class HackMcr
    {
        private static readonly HttpClient _client = new HttpClient();
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

            _client.PostAsJsonAsync(_slackWebHook, slackMessage);
        }

        public static void SendSoundTextMessage(string query)
        {
            var soundManager = new SoundManager();
            var sound = soundManager.RunAsync(query).Result;
            
            var soundSlackMessage = new SoundSlackMessage
            {
                Attachments = new[] { new Attachment
                    {
                        AuthorName = "Authorname " + query,
                        TitleLink = new Uri("http://www.google.com?q=Thisisatitlelink"),
                        Text = sound.Url,
                        AuthorLink = new Uri(sound.Url),
                        ImageUrl = new Uri("https://media1.giphy.com/media/s2qXK8wAvkHTO/giphy.gif?cid=3640f6095bd498e6786c494e67100c67")
                    }
                }
            };

            _client.PostAsJsonAsync(_slackWebHook, soundSlackMessage);
        }
    }

    public class SlackMessage
    {
        public string text { get; set; }
    }
        
}

