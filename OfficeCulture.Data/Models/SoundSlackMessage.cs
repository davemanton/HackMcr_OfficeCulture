using System.Net.Mail;
using Newtonsoft.Json;

public partial class SoundSlackMessage
{
    [JsonProperty("attachments")]
    public Attachment[] Attachments { get; set; }
}