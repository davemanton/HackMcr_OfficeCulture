using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OfficeCulture.Sounds.Models
{
    public partial class Sound
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("image")]
        public object Image { get; set; }
    }
}
