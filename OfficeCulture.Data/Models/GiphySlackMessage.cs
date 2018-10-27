using Newtonsoft.Json;

namespace OfficeCulture.Data.Models
{
    public partial class GiphySlackMessage
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; set; }
    }
}