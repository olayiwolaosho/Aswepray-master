using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WePray.AllConstants;
using WePrayFeaturedImageUrl;
using Xamarin.Essentials;

namespace WePrayWPResponseObject
{
    public partial class WPResponseObject
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("date_gmt")]
        public DateTimeOffset DateGmt { get; set; }

        [JsonProperty("guid")]
        public GuidClass Guid { get; set; }

        [JsonProperty("modified")]
        public DateTimeOffset Modified { get; set; }

        [JsonProperty("modified_gmt")]
        public DateTimeOffset ModifiedGmt { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("link")]
        public Uri Link { get; set; }

        [JsonProperty("title")]
        public GuidClass Title { get; set; }

        [JsonProperty("content")]
        public Content Content { get; set; }

        [JsonProperty("excerpt")]
        public Content Excerpt { get; set; }

        [JsonProperty("author")]
        public long Author { get; set; }

        [JsonProperty("featured_media")]
        public long FeaturedMedia { get; set; }

        [JsonProperty("comment_status")]
        public string CommentStatus { get; set; }

        [JsonProperty("ping_status")]
        public string PingStatus { get; set; }

        [JsonProperty("sticky")]
        public bool Sticky { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("meta")]
        public List<object> Meta { get; set; }

        [JsonProperty("categories")]
        public List<long> Categories { get; set; }

        [JsonProperty("tags")]
        public List<long> Tags { get; set; }

        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }

    public partial class Content
    {
        [JsonProperty("rendered")]
        public string Rendered { get; set; }

        [JsonProperty("protected")]
        public bool Protected { get; set; }
    }

    public partial class GuidClass
    {
        [JsonProperty("rendered")]
        public string Rendered { get; set; }
    }

    public partial class Links
    {
        [JsonProperty("self")]
        public List<About> Self { get; set; }

        [JsonProperty("collection")]
        public List<About> Collection { get; set; }

        [JsonProperty("about")]
        public List<About> About { get; set; }

        [JsonProperty("author")]
        public List<Author> Author { get; set; }

        [JsonProperty("replies")]
        public List<Author> Replies { get; set; }

        [JsonProperty("version-history")]
        public List<VersionHistory> VersionHistory { get; set; }

        [JsonProperty("predecessor-version")]
        public List<PredecessorVersion> PredecessorVersion { get; set; }

        [JsonProperty("wp:featuredmedia")]
        public List<Author> WpFeaturedmedia { get; set; }

        [JsonProperty("wp:attachment")]
        public List<About> WpAttachment { get; set; }

        [JsonProperty("wp:term")]
        public List<WpTerm> WpTerm { get; set; }

        [JsonProperty("curies")]
        public List<Cury> Curies { get; set; }
    }

    public partial class About
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }
    }

    public partial class Author
    {
        [JsonProperty("embeddable")]
        public bool Embeddable { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }
    }

    public partial class Cury
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("templated")]
        public bool Templated { get; set; }
    }

    public partial class PredecessorVersion
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }
    }

    public partial class VersionHistory
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }
    }

    public partial class WpTerm
    {
        [JsonProperty("taxonomy")]
        public string Taxonomy { get; set; }

        [JsonProperty("embeddable")]
        public bool Embeddable { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }
    }

    public partial class WPResponseObject
    {
        public static List<WPResponseObject> FromJson(string json) => JsonConvert.DeserializeObject<List<WPResponseObject>>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this List<WPResponseObject> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }  
    
    internal static class GetFeaturedImage
    {

        public async static Task<string> featuredimage(string uri)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            HttpResponseMessage response = await httpClient.GetAsync(uri);

            string serialized = await response.Content.ReadAsStringAsync();

            var result = FeaturedImageUrl.FromJson(serialized);

            return result.SourceUrl.AbsoluteUri;
        }

    }

}

