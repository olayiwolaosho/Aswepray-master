using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GetAllBibleResponse
{
    //For the response you need thr auth header - api - key 
    // the endpoint
    // and language eng
    class AllBibleResponse
    {
        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("dblId")]
        public string DblId { get; set; }

        [JsonProperty("relatedDbl")]
        public object RelatedDbl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nameLocal")]
        public string NameLocal { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("abbreviationLocal")]
        public string AbbreviationLocal { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("descriptionLocal")]
        public string DescriptionLocal { get; set; }

        [JsonProperty("language")]
        public Language Language { get; set; }

        [JsonProperty("countries")]
        public AudioBible[] Countries { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("audioBibles")]
        public AudioBible[] AudioBibles { get; set; }
    }

    public partial class AudioBible
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nameLocal")]
        public string NameLocal { get; set; }

        [JsonProperty("dblId", NullValueHandling = NullValueHandling.Ignore)]
        public string DblId { get; set; }
    }

    public partial class Language
    {
        [JsonProperty("id")]
        public Id Id { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("nameLocal")]
        public Name NameLocal { get; set; }

        [JsonProperty("script")]
        public Script Script { get; set; }

        [JsonProperty("scriptDirection")]
        public ScriptDirection ScriptDirection { get; set; }
    }

    public enum Id { Eng };

    public enum Name { English };

    public enum Script { Latin };

    public enum ScriptDirection { Ltr };

    public enum TypeEnum { Text };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                IdConverter.Singleton,
                NameConverter.Singleton,
                ScriptConverter.Singleton,
                ScriptDirectionConverter.Singleton,
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class IdConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Id) || t == typeof(Id?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "eng")
            {
                return Id.Eng;
            }
            throw new Exception("Cannot unmarshal type Id");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Id)untypedValue;
            if (value == Id.Eng)
            {
                serializer.Serialize(writer, "eng");
                return;
            }
            throw new Exception("Cannot marshal type Id");
        }

        public static readonly IdConverter Singleton = new IdConverter();
    }

    internal class NameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Name) || t == typeof(Name?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "English")
            {
                return Name.English;
            }
            throw new Exception("Cannot unmarshal type Name");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Name)untypedValue;
            if (value == Name.English)
            {
                serializer.Serialize(writer, "English");
                return;
            }
            throw new Exception("Cannot marshal type Name");
        }

        public static readonly NameConverter Singleton = new NameConverter();
    }

    internal class ScriptConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Script) || t == typeof(Script?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Latin")
            {
                return Script.Latin;
            }
            throw new Exception("Cannot unmarshal type Script");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Script)untypedValue;
            if (value == Script.Latin)
            {
                serializer.Serialize(writer, "Latin");
                return;
            }
            throw new Exception("Cannot marshal type Script");
        }

        public static readonly ScriptConverter Singleton = new ScriptConverter();
    }

    internal class ScriptDirectionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ScriptDirection) || t == typeof(ScriptDirection?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "LTR")
            {
                return ScriptDirection.Ltr;
            }
            throw new Exception("Cannot unmarshal type ScriptDirection");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ScriptDirection)untypedValue;
            if (value == ScriptDirection.Ltr)
            {
                serializer.Serialize(writer, "LTR");
                return;
            }
            throw new Exception("Cannot marshal type ScriptDirection");
        }

        public static readonly ScriptDirectionConverter Singleton = new ScriptDirectionConverter();
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "text")
            {
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
            if (value == TypeEnum.Text)
            {
                serializer.Serialize(writer, "text");
                return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
