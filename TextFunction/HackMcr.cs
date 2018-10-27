using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using OfficeCulture.Luis;
using OfficeCulture.Data.Models;
using OfficeCulture.Sounds.Manager;
using System.Collections.Generic;

namespace TextFunction
{
    public static class HackMcr
    {
        private static string _slackMessageWebHook = "https://hooks.slack.com/services/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";
        private static string _slackFileWebHook = "https://hooks.slack.com/services/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";

        [FunctionName("hackmcr")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string content = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "content", true) == 0)
                .Value;

            string from = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "from", true) == 0)
                .Value;

            var luisClient = new LuisClient();
            var luisData = await luisClient.AnalyseText(content);

            // bold out entity pieces in original message
            if (luisData.Entities != null)
            {
                var counter = 0;
                foreach (var entity in luisData.Entities)
                {
                    content = content.Insert(entity.StartIndex + counter++, "*");
                    content = content.Insert(entity.EndIndex + counter++ + 1, "*");
                }
            }

            var searchKeywords = string.Join(" ", luisData.Entities.Select(x => x.Entity));

            var client = new HttpClient();

            SendTextMessage(client, content, searchKeywords);
            SendSoundTextMessage(client, content, searchKeywords);

            return req.CreateResponse(HttpStatusCode.OK, content);
        }


        public static void SendTextMessage(HttpClient client, string from, string searchKeywords)
        {
            if (!string.IsNullOrWhiteSpace(from) && !string.IsNullOrWhiteSpace(searchKeywords))
                client.GetAsync("https://api.clockworksms.com/http/send.aspx?key=a15795bf55cf6acaf6061be7af26bbb86bc22c52&to={from}&content=You%27re giphin about {searchKeywords}");
        }

        public static void SendSoundTextMessage(HttpClient client, string message, string searchKeywords)
        {
            var soundManager = new SoundManager();
            var sound = soundManager.RunAsync(message).Result;

            var soundSlackMessage = new SlackMessage
            {
                text = message,
                attachments = new List<Attachment> {
                    new Attachment
                    {
                        Title = $"Click here to listen to {searchKeywords}",
                        TitleLink = $"https://{sound.Url}"
                    }
                }
            };

            client.PostAsJsonAsync(_slackMessageWebHook, soundSlackMessage);
        }
    }
}

