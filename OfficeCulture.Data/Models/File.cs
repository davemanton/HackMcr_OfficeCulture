using System;
using Newtonsoft.Json;

namespace OfficeCulture.Data.Models
{
    public partial class File
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("mimetype")]
        public string Mimetype { get; set; }

        [JsonProperty("filetype")]
        public string Filetype { get; set; }

        [JsonProperty("pretty_type")]
        public string PrettyType { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("editable")]
        public bool Editable { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("is_external")]
        public bool IsExternal { get; set; }

        [JsonProperty("external_type")]
        public string ExternalType { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("public_url_shared")]
        public bool PublicUrlShared { get; set; }

        [JsonProperty("display_as_bot")]
        public bool DisplayAsBot { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("url_private")]
        public Uri UrlPrivate { get; set; }

        [JsonProperty("url_private_download")]
        public Uri UrlPrivateDownload { get; set; }

        [JsonProperty("thumb_64")]
        public Uri Thumb64 { get; set; }

        [JsonProperty("thumb_80")]
        public Uri Thumb80 { get; set; }

        [JsonProperty("thumb_360")]
        public Uri Thumb360 { get; set; }

        [JsonProperty("thumb_360_w")]
        public long Thumb360_W { get; set; }

        [JsonProperty("thumb_360_h")]
        public long Thumb360_H { get; set; }

        [JsonProperty("thumb_480")]
        public Uri Thumb480 { get; set; }

        [JsonProperty("thumb_480_w")]
        public long Thumb480_W { get; set; }

        [JsonProperty("thumb_480_h")]
        public long Thumb480_H { get; set; }

        [JsonProperty("thumb_160")]
        public Uri Thumb160 { get; set; }

        [JsonProperty("image_exif_rotation")]
        public long ImageExifRotation { get; set; }

        [JsonProperty("original_w")]
        public long OriginalW { get; set; }

        [JsonProperty("original_h")]
        public long OriginalH { get; set; }

        [JsonProperty("permalink")]
        public Uri Permalink { get; set; }

        [JsonProperty("permalink_public")]
        public Uri PermalinkPublic { get; set; }

        [JsonProperty("comments_count")]
        public long CommentsCount { get; set; }

        [JsonProperty("is_starred")]
        public bool IsStarred { get; set; }

        [JsonProperty("channels")]
        public object[] Channels { get; set; }

        [JsonProperty("groups")]
        public object[] Groups { get; set; }

        [JsonProperty("ims")]
        public string[] Ims { get; set; }

        [JsonProperty("has_rich_preview")]
        public bool HasRichPreview { get; set; }
    }
}