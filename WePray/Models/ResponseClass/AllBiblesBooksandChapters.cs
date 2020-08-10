using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WePrayAllBiblesBooksandChapters
{
    class AllBiblesBooksandChapters
    {
        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("bibleId")]
        public string BibleId { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nameLong")]
        public string NameLong { get; set; }

        [JsonProperty("chapters")]
        public Chapter[] Chapters { get; set; }
    }

    public partial class Chapter
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("bibleId")]
        public string BibleId { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("bookId")]
        public string BookId { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}
