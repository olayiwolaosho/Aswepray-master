using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WePrayFeaturedImageUrl
{
    public partial class FeaturedImageUrl
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("date_gmt")]
        public DateTimeOffset DateGmt { get; set; }

        [JsonProperty("guid")]
        public Caption Guid { get; set; }

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
        public Caption Title { get; set; }

        [JsonProperty("author")]
        public long Author { get; set; }

        [JsonProperty("comment_status")]
        public string CommentStatus { get; set; }

        [JsonProperty("ping_status")]
        public string PingStatus { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("meta")]
        public List<object> Meta { get; set; }

        [JsonProperty("description")]
        public Caption Description { get; set; }

        [JsonProperty("caption")]
        public Caption Caption { get; set; }

        [JsonProperty("alt_text")]
        public string AltText { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("media_details")]
        public MediaDetails MediaDetails { get; set; }

        [JsonProperty("post")]
        public string Post { get; set; }

        [JsonProperty("source_url")]
        public Uri SourceUrl { get; set; }

        [JsonProperty("_links")]
        public Links Links { get; set; }
    }

    public partial class Caption
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

    public partial class MediaDetails
    {
        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("sizes")]
        public Sizes Sizes { get; set; }

        [JsonProperty("image_meta")]
        public ImageMeta ImageMeta { get; set; }
    }

    public partial class ImageMeta
    {
        [JsonProperty("aperture")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Aperture { get; set; }

        [JsonProperty("credit")]
        public string Credit { get; set; }

        [JsonProperty("camera")]
        public string Camera { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("created_timestamp")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long CreatedTimestamp { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("focal_length")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long FocalLength { get; set; }

        [JsonProperty("iso")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Iso { get; set; }

        [JsonProperty("shutter_speed")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ShutterSpeed { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("orientation")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Orientation { get; set; }

        [JsonProperty("keywords")]
        public List<object> Keywords { get; set; }
    }

    public partial class Sizes
    {
        [JsonProperty("medium")]
        public Full Medium { get; set; }

        [JsonProperty("thumbnail")]
        public Full Thumbnail { get; set; }

        [JsonProperty("mh-magazine-lite-slider")]
        public Full MhMagazineLiteSlider { get; set; }

        [JsonProperty("mh-magazine-lite-content")]
        public Full MhMagazineLiteContent { get; set; }

        [JsonProperty("mh-magazine-lite-large")]
        public Full MhMagazineLiteLarge { get; set; }

        [JsonProperty("mh-magazine-lite-medium")]
        public Full MhMagazineLiteMedium { get; set; }

        [JsonProperty("mh-magazine-lite-small")]
        public Full MhMagazineLiteSmall { get; set; }

        [JsonProperty("full")]
        public Full Full { get; set; }
    }

    public partial class Full
    {
        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("source_url")]
        public Uri SourceUrl { get; set; }
    }

    public partial class FeaturedImageUrl
    {
        public static FeaturedImageUrl FromJson(string json) => JsonConvert.DeserializeObject<FeaturedImageUrl>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this FeaturedImageUrl self) => JsonConvert.SerializeObject(self, Converter.Settings);
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

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
