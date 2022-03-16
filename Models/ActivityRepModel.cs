using Newtonsoft.Json;

namespace TRS2.Models{
    public class Entry
    {
        [JsonProperty("date ")]
        public string date { get; set; }

        [JsonProperty("code ")]
        public string code { get; set; }

        [JsonProperty("time ")]
        public int time { get; set; }

        [JsonProperty("description ")]
        public string description { get; set; }
    }


    public class ActivityRepModel{

        [JsonProperty("entries")]
        public List<Entry> Entries { get; set; }

    }
}