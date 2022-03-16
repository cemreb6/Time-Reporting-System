using Newtonsoft.Json;

namespace TRS2.Models{

    public class ActivitiesModel
    {
        [JsonProperty("code ")]
        public string code { get; set; }

        [JsonProperty("manager ")]
        public string manager { get; set; }

        [JsonProperty("name ")]
        public string name { get; set; }

        [JsonProperty("budget ")]
        public int budget { get; set; }
    }

    public class Activities
    {
        [JsonProperty("activities ")]
        public List<ActivitiesModel> ActivitiesList { get; set; }
    }
}