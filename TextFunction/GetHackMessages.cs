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
    public static class GetHackMessages
    {
        private static string _slackMessageWebHook = "https://hooks.slack.com/services/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";

        [FunctionName("gethackmessages")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log)
        {
            // parse query parameter
            string timestampString = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "timestamp", true) == 0)
                .Value;

            var timestamp = DateTime.ParseExact(timestampString, "yyyyMMdd HH:mm", null);

            const string EndpointUrl = "https://hackmcr.documents.azure.com:443/";
            const string PrimaryKey = "TrMpg5jbBZN1MWJnZ68SqIbv2sgkWm1G23xrEhBdpWFFa5KYMQl6XpCVlzxN1xauA45w0sDx5iHEgC4NKqSn3w==";
            DocumentClient docClient = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            List<HackMessage> messages = docClient.CreateDocumentQuery<HackMessage>(
                UriFactory.CreateDocumentCollectionUri("ToDoList", "Messages"))
                .Where(x => x.Timestamp > timestamp).ToList();


            return req.CreateResponse(HttpStatusCode.OK, messages);
        }        
    }

    public class HackMessage
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string SearchKeywords { get; set; }
        public string Sound { get; set; }
        public string Gif { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

