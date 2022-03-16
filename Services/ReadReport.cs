using TRS2.Models;
using Newtonsoft.Json.Linq;

namespace TRS2.Services{

    public class ReadReport{
        public ActivityRepModel activityList {get;set;}

        public ReadReport(){
            activityList=new ActivityRepModel();
           // ReadFile();
        }   

        public void ReadFile(){
            string fileName="kowalski-2021-11.json";
            StreamReader reader=new StreamReader(fileName);
            string json=reader.ReadToEnd();
            reader.Close();

            JObject report=JObject.Parse(json);
            List<Entry> entryList=new List<Entry>();
            foreach (JProperty item in report.Properties())
            {

                if(item.Name=="entries "){
                    var entry=(JArray)item.Value;
                    foreach (JObject ent1 in entry)
                    {
                        Entry ent=new Entry();
                        ent.code=ent1.GetValue("code ").ToString();
                        ent.date=ent1.GetValue("date ").ToString();
                        ent.time=(int)ent1.GetValue("time ");
                        ent.description=ent1.GetValue("description ").ToString();
                        entryList.Add(ent);        
                    }
                }
                
            }
           activityList.Entries=entryList;   
        }

        public void ReadOtherFiles(string file){
            string fileName=file;
            StreamReader reader=new StreamReader(fileName);
            string json=reader.ReadToEnd();
            reader.Close();

            JObject report=JObject.Parse(json);
            List<Entry> entryList=new List<Entry>();
            foreach (JProperty item in report.Properties())
            {
                
                if(item.Name=="entries "){
                    var entry=(JArray)item.Value;
                    foreach (JObject ent1 in entry)
                    {
                        Entry ent=new Entry();
                        ent.code=ent1.GetValue("code ").ToString();
                        ent.date=ent1.GetValue("date ").ToString();
                        ent.time=(int)ent1.GetValue("time ");
                        ent.description=ent1.GetValue("description ").ToString();
                        entryList.Add(ent);        
                    }
                }
                
            }
           activityList.Entries=entryList;    
        }
    }
}