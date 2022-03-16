using TRS2.Models;
using Newtonsoft.Json;

namespace TRS2.Services{

    public class ReadActivities{

        public List<ActivitiesModel> activityList {get;set;}

        public ReadActivities(){
            ReadFile();
        }

        public void ReadFile(){
            string fileName="activity.json";
            Activities actModel = JsonConvert.DeserializeObject<Activities>(File.ReadAllText(fileName)); 
            activityList=actModel.ActivitiesList;
            }
        }
    }