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
using OfficeCulture.GiphyFunction.Manager;
using OfficeCulture.Sounds.Manager;
using System.Collections.Generic;

namespace TextFunction
{
    public static class HackMcr
    {
        private static string _slackMessageWebHook = "https://hooks.slack.com/services/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";

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
            SendSlackMessage(client, content, searchKeywords);
            SendSlackFile(content, searchKeywords);

            return req.CreateResponse(HttpStatusCode.OK, content);
        }


        public static void SendTextMessage(HttpClient client, string from, string searchKeywords)
        {
            if (!string.IsNullOrWhiteSpace(from) && !string.IsNullOrWhiteSpace(searchKeywords))
                client.GetAsync("https://api.clockworksms.com/http/send.aspx?key=a15795bf55cf6acaf6061be7af26bbb86bc22c52&to={from}&content=You%27re giphin about {searchKeywords}");
        }

        public static async Task SendSlackMessage(HttpClient client, string message, string searchKeywords)
        {
            var giphyManager = new GiphyManager();
            var giphy = await giphyManager.RunAsync(message);
            if (giphy.data.Any())
            {
                var datum = giphy.data.FirstOrDefault();
                var imageUrl = datum.images.original.url;

                var giphySlackMessage = new SlackMessage
                {
                    text = message,
                    attachments = new List<Attachment> { new Attachment
                        {
                            Text = message,                            
                            ImageUrl = new Uri(imageUrl)
                        }
                    }
                };

                await client.PostAsJsonAsync(_slackMessageWebHook, giphySlackMessage);
            }
        }

        public static async void SendSlackFile(string message, string searchKeywords)
        {
            //get sound
            var soundManager = new SoundManager();
            var sound = soundManager.RunAsync(message).Result;
            
            //turn into slack file upload
            //var soundSlackMessage = new SlackFileUpload
            //{
            //    token = "xoxp-465245447568-465981698178-465985666258-c2ff53ae821cdca8820458d7982e2b37",
            //    channels = "CDP77D8JC",
            //    title = message,
            //    filetype = "mp3",
            //    contentType = "multipart/form-data",
            //    file = new File
            //    {
            //        Mimetype = "audio/mpeg",
            //        Title = $"Click here to listen to {searchKeywords}",
            //        UrlPrivate = $"https:{sound.Url}",
            //        UrlPrivateDownload =$"https:{sound.Url}"
            //    }
            //};

            var soundSlackMessage = new SlackFileUpload
            {
                token = "xoxp-465245447568-465981698178-465907776675-f387ae03163e674dd443902cae1b0c93",
                channels = "CDP77D8JC",
                filename = "test.mp3",
                filetype = "mp3"
            };

            // upload to slack via api
            var slackFileManager = new SlackFileManager();
            await slackFileManager.RunAsync(soundSlackMessage);
        }
    }
}

