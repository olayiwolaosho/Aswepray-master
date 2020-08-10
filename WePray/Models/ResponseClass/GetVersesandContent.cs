using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WePrayGetVersesandContent
{
    public class GetVersesandContent
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("bibleId")]
        public string BibleId { get; set; }

        [JsonProperty("number")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Number { get; set; }

        [JsonProperty("bookId")]
        public string BookId { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("content")]
        public Content[] Content { get; set; }

        [JsonProperty("next")]
        public Next Next { get; set; }

        [JsonProperty("previous")]
        public Next Previous { get; set; }
    }

    public partial class Content
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("attrs")]
        public ContentAttrs Attrs { get; set; }

        [JsonProperty("items")]
        public ContentItem[] Items { get; set; }
    }

    public partial class ContentAttrs
    {
        [JsonProperty("style")]
        public string Style { get; set; }
    }

    public partial class ContentItem
    {
        //[JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("attrs")]
        public PurpleAttrs Attrs { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public ItemItem[] Items { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }

    public partial class PurpleAttrs
    {
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Number { get; set; }

        //[JsonProperty("style", NullValueHandling = NullValueHandling.Ignore)]
        public string Style { get; set; }

        [JsonProperty("verseId", NullValueHandling = NullValueHandling.Ignore)]
        public string VerseId { get; set; }

        [JsonProperty("verseOrgIds", NullValueHandling = NullValueHandling.Ignore)]
        public string[] VerseOrgIds { get; set; }
    }

    public partial class ItemItem
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("attrs", NullValueHandling = NullValueHandling.Ignore)]
        public FluffyAttrs Attrs { get; set; }
    }

    public partial class FluffyAttrs
    {
        [JsonProperty("verseId")]
        public string VerseId { get; set; }

        [JsonProperty("verseOrgIds")]
        public string[] VerseOrgIds { get; set; }
    }

    public partial class Next
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("bookId")]
        public string BookId { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("fums")]
        public string Fums { get; set; }

        [JsonProperty("fumsId")]
        public Guid FumsId { get; set; }

        [JsonProperty("fumsJsInclude")]
        public string FumsJsInclude { get; set; }

        [JsonProperty("fumsJs")]
        public string FumsJs { get; set; }

        [JsonProperty("fumsNoScript")]
        public string FumsNoScript { get; set; }
    }



    public enum TypeEnum { Tag, Text };

 

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

  
    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "tag":
                    return TypeEnum.Tag;
                case "text":
                    return TypeEnum.Text;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.Tag:
                    serializer.Serialize(writer, "tag");
                    return;
                case TypeEnum.Text:
                    serializer.Serialize(writer, "text");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }

   
}
