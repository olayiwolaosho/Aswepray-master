using Newtonsoft.Json;
using System;

namespace GetaParticularBibleRequest
{
    class ParticularBibleRequest
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
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
        public Country[] Countries { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }

        [JsonProperty("audioBibles")]
        public object[] AudioBibles { get; set; }
    }

    public partial class Country
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nameLocal")]
        public string NameLocal { get; set; }
    }

    public partial class Language
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nameLocal")]
        public string NameLocal { get; set; }

        [JsonProperty("script")]
        public string Script { get; set; }

        [JsonProperty("scriptDirection")]
        public string ScriptDirection { get; set; }
    }
}