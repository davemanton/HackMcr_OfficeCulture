using Newtonsoft.Json;

namespace OfficeCulture.Data.Models
{
    public partial class SoundSlackMessage
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("file")]
        public File File { get; set; }
    }
}