using System;
using System.Collections.Generic;

namespace TRS2.Models{

    public class CurrentDayActivityModel{

        public List<ActivitiesModel> activityList{get; set;}

        public CurrentDayActivityModel(List<ActivitiesModel> activityList){
            this.activityList=activityList;
        }
    }
}