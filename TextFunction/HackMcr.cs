using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace TextFunction
{
    public static class HackMcr
    {
        [FunctionName("hackmcr")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string content = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "content", true) == 0)
                .Value;

            var client = new HttpClient();

            var slackMessage = new SlackMessage
            {
                text = content
            };

            client.PostAsJsonAsync("https://hooks.slack.com/services/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", slackMessage);

            client.GetAsync("https://api.clockworksms.com/http/send.aspx?key=a15795bf55cf6acaf6061be7af26bbb86bc22c52&to=447950580480&content=Hello+World");

            return req.CreateResponse(HttpStatusCode.OK, content);
        }


    }

    public class SlackMessage
    {
        public string text { get; set; }
    }
}
