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
using OfficeCulture.Chuck;
using OfficeCulture.Chuck.Coin;
using Action = OfficeCulture.Data.Models.Action;
using SlackAttachment = OfficeCulture.Data.Models.SlackAttachment;
using OfficeCulture.Translate;

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

            // parse query parameter
            var message = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "content", true) == 0)
                .Value;

            var translationClient = new TranslateClient();
            var translationTask = translationClient.GetTranslation(message);

            var from = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "from", true) == 0)
                .Value;

            var luisClient = new LuisClient();
            var luisData = await luisClient.AnalyseText(message);

            // bold out entity pieces in original message
            var content = message;
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
            SendGiphinMessage(client, from, searchKeywords);
            SentimentMessage(client, from , luisData, searchKeywords);
           
            var translations = await translationTask;   
            foreach(var translation in translations)
            {
                SendTextMessage(client, from, $"It seemed like time to learn {translation.Key}, you said {translation.Value}");
            }

            var random = new Random();

            var imageUrl = await SendSlackMessage(client, content, searchKeywords);
            var soundUrl = await SendSlackSoundMessage(client, content, searchKeywords);

            const string EndpointUrl = "https://hackmcr.documents.azure.com:443/";
            const string PrimaryKey = "TrMpg5jbBZN1MWJnZ68SqIbv2sgkWm1G23xrEhBdpWFFa5KYMQl6XpCVlzxN1xauA45w0sDx5iHEgC4NKqSn3w==";
            DocumentClient docClient = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            var response = await docClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("ToDoList", "Messages"),
                new
                {
                    Message = message,
                    SearchKeywords = searchKeywords,
                    Sound = $"https:{soundUrl}",
                    Gif = imageUrl,
                    Sentiment = luisData.SentimentAnalysis.Label,
                    Timestamp = DateTime.Now
                });

            return req.CreateResponse(HttpStatusCode.OK, content);
        }

        private static async Task SentimentMessage(HttpClient client, string from, LuisData luisData, string searchKeywords)
        {
            string sentimentMessage;
            switch (luisData.SentimentAnalysis.Label)
            {
                case "positive":
                    sentimentMessage = $"You seemed happy you should probable calm down and remember a bitcoin is worth £";
                    break;
                case "negative":
                    sentimentMessage = $"You seemed sad, how bout a chuck norris fact to cheer you up: ";
                    break;
                default:
                    sentimentMessage = $"We couldn't figure out how you were feeling... here's a quote to inspire you: ";
                    break;
            }

            switch (luisData.SentimentAnalysis.Label)
            {
                case "positive":
                    var coinClient = new BitcoinClient();
                    var coinValue = await coinClient.GetPrice();
                    sentimentMessage = sentimentMessage + coinValue;
                    break;
                case "negative":
                    var chuckClient = new ChuckClient();
                    var joke = await chuckClient.GetJoke();
                    sentimentMessage = sentimentMessage + ": " + joke.value;
                    break;
                default:
                    var quoteClient = new QuoteClient();
                    var quote = await quoteClient.GetQuote();
                    sentimentMessage = sentimentMessage + " " + quote;
                    break;
            }

            SendTextMessage(client, from, sentimentMessage);
        }

        private static async Task<string> GetChuckJoke()
        {
            var chuckClient = new ChuckClient();

            var data = await chuckClient.GetJoke();

            return data.value;
        }

        public static void SendGiphinMessage(HttpClient client, string from, string searchKeywords)
        {
            if (string.IsNullOrWhiteSpace(searchKeywords))
            {
                searchKeywords = "something we can't understand yet";
            }

            SendTextMessage(client, from, $"You%27re giphin on about {searchKeywords}");
        }

        public static void SendTextMessage(HttpClient client, string from, string content)
        {
            if (!string.IsNullOrWhiteSpace(from))
            {
                var url = $"https://api.clockworksms.com/http/send.aspx?key=a15795bf55cf6acaf6061be7af26bbb86bc22c52&to={from}&content={content}";
                client.GetAsync(url);
            }
        }

        public static async Task<string> SendSlackMessage(HttpClient client, string message, string searchKeywords)
        {
            searchKeywords = string.IsNullOrWhiteSpace(searchKeywords) ? message : searchKeywords;
            var giphyManager = new GiphyManager();
            var giphy = await giphyManager.RunAsync(searchKeywords);
            if (!giphy.data.Any())
                return null;

            var datum = giphy.data.FirstOrDefault();
            var imageUrl = datum.images.original.url;

            var giphySlackMessage = new SlackMessage
            {
                text = message,
                attachments = new List<Attachment> { new Attachment
                        {
                            Text = searchKeywords,
                            ImageUrl = new Uri(imageUrl)
                        }
                    }
            };

            client.PostAsJsonAsync(_slackMessageWebHook, giphySlackMessage);

            return imageUrl;
        }

        public static async Task<string> SendSlackSoundMessage(HttpClient client, string message, string searchKeywords)
        {
            var soundManager = new SoundManager();
            var sound = await soundManager.RunAsync(string.IsNullOrWhiteSpace(searchKeywords) ? message : searchKeywords);

            var soundUrl = sound.Url;

            var soundSlackMessageWithAction = new SlackMessageWithAction()
            {
                Text = "",
                Channel = "CDP77D8JC",
                Attachments = new List<SlackAttachment> { new SlackAttachment
                    {
                        Fallback = "Who needs Spotify?!",
                        Title = "Who needs Spotify?!",
                        Color = "#3AA3E3",
                        Actions = new List<Action> { new Action
                            {
                                Type = "button",
                                Name = "playSound",
                                Value = "playSound",
                                Text = $"🔊 Play the {sound.Name} sound",
                                Url = new Uri($"https:{soundUrl}"),
                                Style = "primary"
                            }
                        }

                    }

                }
            };

        client.PostAsJsonAsync(_slackMessageWebHook, soundSlackMessageWithAction);

            return soundUrl;
        }


    //public static async Task<string> SendSlackFile(string message, string searchKeywords)
    //{
    //    //get sound
    //    var soundManager = new SoundManager();
    //    var sound = await soundManager.RunAsync(string.IsNullOrWhiteSpace(searchKeywords) ? message : searchKeywords);
    //    var soundUrl = sound.Url;
    //    //turn into slack file upload
    //    var soundSlackMessage = new SlackFileUpload
    //    {
    //        token = "xoxp-465245447568-465981698178-465985666258-c2ff53ae821cdca8820458d7982e2b37",
    //        channels = "CDP77D8JC",
    //        title = message,
    //        filetype = "mp3",
    //        contentType = "multipart/form-data",
    //        file = new File
    //        {
    //            Mimetype = "audio/mpeg",
    //            Title = $"Click here to listen to {searchKeywords}",
    //            UrlPrivate = $"https:{sound.Url}",
    //            UrlPrivateDownload = $"https:{sound.Url}"
    //        }
    //    };


    //    // upload to slack via api
    //    var slackFileManager = new SlackFileManager();
    //    slackFileManager.RunAsync(soundSlackMessage);

    //    return soundUrl;
    //}
}
}

