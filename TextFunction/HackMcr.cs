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
using Microsoft.Azure.Documents.Client;

namespace TextFunction
{
    public class HackMcr
    {
        private static string _slackMessageWebHook = "https://hooks.slack.com/services/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";

        [FunctionName("hackmcr")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log)
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

            SendTextMessage(client, from, searchKeywords);
            var imageUrl = await SendSlackMessage(client, content, searchKeywords);
            var soundUrl = await SendSlackFile(content, searchKeywords);

            const string EndpointUrl = "https://hackmcr.documents.azure.com:443/";
            const string PrimaryKey = "TrMpg5jbBZN1MWJnZ68SqIbv2sgkWm1G23xrEhBdpWFFa5KYMQl6XpCVlzxN1xauA45w0sDx5iHEgC4NKqSn3w==";
            DocumentClient docClient = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            var response = await docClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("ToDoList", "Messages"),
                new
                {
                    Message = content,
                    SearchKeywords = searchKeywords,
                    Sound = soundUrl,
                    Gif = imageUrl,
                    Timestamp = DateTime.Now
                });

            return req.CreateResponse(HttpStatusCode.OK, content);
        }


        public static void SendTextMessage(HttpClient client, string from, string searchKeywords)
        {
            if (string.IsNullOrWhiteSpace(searchKeywords))
            {
                searchKeywords = "Luis is available right now, but if you leave a message, he will get back to you";
            }

            if (!string.IsNullOrWhiteSpace(from))
            {
                var url = $"https://api.clockworksms.com/http/send.aspx?key=a15795bf55cf6acaf6061be7af26bbb86bc22c52&to={from}&content=You%27re+giphin+on+about+{searchKeywords}";
                client.GetAsync(url);
            }
        }

        public static async Task<string> SendSlackMessage(HttpClient client, string message, string searchKeywords)
        {
            var giphyManager = new GiphyManager();
            var giphy = await giphyManager.RunAsync(message);
            if (!giphy.data.Any())
                return null;

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

            client.PostAsJsonAsync(_slackMessageWebHook, giphySlackMessage);

            return imageUrl;
        }

        public static async Task<string> SendSlackFile(string message, string searchKeywords)
        {
            //get sound
            var soundManager = new SoundManager();
            var sound = await soundManager.RunAsync(message);
            var soundUrl = sound.Url;
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
                token = "xoxp-465245447568-465812080884-465912753411-37f8b9cb7372ef62d8a3990062e9b3da",
                channels = "CDP77D8JC",
                filename = "test.mp3",
                filetype = "mp3"
            };

            // upload to slack via api
            var slackFileManager = new SlackFileManager();
            slackFileManager.RunAsync(soundSlackMessage);

            return soundUrl;
        }
    }
}

