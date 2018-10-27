using Newtonsoft.Json;

namespace OfficeCulture.Giphy.Models
{
    public partial class Giphy
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("slug")]
        public string Slug { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("images")]
        public Images Image { get; set; }
    }

    public class Images
    {
        [JsonProperty("original")]
        public ImageProperties ImageProperty { get; set; }
    }

    public class ImageProperties
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("width")]
        public string Width { get; set; }
        [JsonProperty("height")]
        public string Height { get; set; }
        [JsonProperty("size")]
        public string Size { get; set; }
    }
}
