using Newtonsoft.Json;

namespace OfficeCulture.Data.Models
{
    public partial class GiphySlackMessage
    {
        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; set; }
    }
}