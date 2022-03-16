using TRS2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TRS2.Services{

    public class Write{

        public void WriteReport(Entry entry1){
            ReadReport read=new ReadReport();
            read.activityList.Entries.Add(entry1);
            
            JObject newReport =new JObject();

            JArray entriesArray=new JArray();
            foreach (Entry item in read.activityList.Entries)
            {
                entriesArray.Add(JToken.FromObject(item));
            }

            JProperty property=new JProperty("entries ",entriesArray);
            newReport.Add(property);
            
            string json=JsonConvert.SerializeObject(newReport, Formatting.Indented);
            System.IO.File.WriteAllText(@"kowalski-2021-11",json);
        }

        public void WriteOtherReport(string fileName,ActivityRepModel model){
            
            JObject newReport =new JObject();
            JArray entriesArray=new JArray();
            foreach (Entry item in model.Entries)
            {
                entriesArray.Add(JToken.FromObject(item));
            }
            JProperty property=new JProperty("entries ",entriesArray);
            newReport.Add(property);
            string json=JsonConvert.SerializeObject(newReport, Formatting.Indented);
            System.IO.File.WriteAllText(@fileName,json);
        }

        public void WriteActivity(JArray actArray){
            string fileName = "activity.json"; 
            JObject newActivities=new JObject();
            JProperty property=new JProperty("activities ",actArray);
            newActivities.Add(property);
            string json=JsonConvert.SerializeObject(newActivities, Formatting.Indented);
            System.IO.File.WriteAllText(@fileName,json);
        }
    }

}