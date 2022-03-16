using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TRS2.Models;
using TRS2.Services;
using Newtonsoft.Json.Linq;
using System.IO;


namespace TRS2.Controllers;

public class HomeController : Controller
{
    public string usrname="";
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet][HttpPost]
    public IActionResult Index(DateTime reportDate)
    {
        ActivityRepModel rep=new ActivityRepModel();
        
        string date="";
        if(reportDate !=null){
            ReadReport read=new ReadReport();
            read.ReadFile();
            rep.Entries=new List<Entry>();
            foreach (Entry item in read.activityList.Entries)
            {  
                if(item.date.Contains((reportDate.ToString("yyyy-MM-dd")))){
                    (rep.Entries).Add(item);
                    date=reportDate.ToString("yyyy-MM-dd");
                }
            }
        }
        TempData["date"]=date;

        ReadActivities read1=new ReadActivities();

        //ViewBag.activitiesAmount=rep.Entries.Count;
        ViewBag.activitiesDetails=read1.activityList;
        return View(rep);   
    }

    [HttpGet]
    public IActionResult Menu(string selectDate)
    {
        ActivityRepModel rep=new ActivityRepModel();
        if(selectDate=="2021-11"){
            ReadReport read=new ReadReport();
            read.ReadFile();
            rep=read.activityList;
        }
        return View(rep);
    }

    public IActionResult Delete()
    {
        ReadActivities read=new ReadActivities();
        CurrentDayActivityModel model=new CurrentDayActivityModel(read.activityList);
        return View(model);
    }

    [HttpGet][HttpPost]
    public IActionResult EntryDialog()
    {    
        ReadActivities read=new ReadActivities();
        CurrentDayActivityModel model=new CurrentDayActivityModel(read.activityList);
        return View(model);
    }

    [HttpGet]
    public ActionResult DeleteAction(string code){
        
        string deletedcode=code;
        ReadActivities read=new ReadActivities();

        CurrentDayActivityModel model=new CurrentDayActivityModel(read.activityList);
        List<ActivitiesModel> newModel=new List<ActivitiesModel>();
        bool isValidCode=false;
        foreach (ActivitiesModel item in model.activityList)
        {
            if(item.code!=deletedcode && item.code !=deletedcode+" "){
                newModel.Add(item);
            }
            else{
                isValidCode=true;
            }
        }

        string message="";
        if(!isValidCode){
            message="Entered code is not valid.Please enter a valid code.";
        }

        else{
            JArray actArray=new JArray();
            foreach (ActivitiesModel item in newModel)
            {
                actArray.Add(JToken.FromObject(item));
            }
            Write delete=new Write();
            delete.WriteActivity(actArray);
            message="Entered code is valid.The activity has been successfully deleted.";
        }
           
        TempData["msg"]=message;
        return RedirectToAction("Delete");
    }

    [HttpGet]
    public ActionResult AddActivity(string code,string manager,string name,int budget){

        ActivitiesModel activitiesModel=new ActivitiesModel();
        activitiesModel.code=code+" ";
        activitiesModel.manager=manager+" ";
        activitiesModel.name=name+" ";
        activitiesModel.budget=budget;
        
        bool isCodeUnique=true;
        ReadActivities read=new ReadActivities();
        foreach(ActivitiesModel act in read.activityList){
            if(act.code==activitiesModel.code ||act.code==activitiesModel.code+" "){
                isCodeUnique=false;
            }
        }

        string message="";
        if(isCodeUnique){
            read.activityList.Add(activitiesModel);

            JArray actArray=new JArray();
            foreach (ActivitiesModel item in read.activityList)
            {
                actArray.Add(JToken.FromObject(item));
            }
            Write addAct=new Write();
            addAct.WriteActivity(actArray);
            message="The activity is added successfully.";
        }
        else{
            message="The code of the activity must be unique.Please enter different code.";
        }
        
        TempData["addmsg"]=message;
        return RedirectToAction("EntryDialog");
    }

    [HttpGet]
    public ActionResult AddReport(DateTime selectDate,int time,string description,string projectCode,string username){
        string newFileName=username+"-"+selectDate.Year+"-"+selectDate.Month+".json";

        ActivityRepModel rep=new ActivityRepModel();
        rep.Entries=new List<Entry>();

        if(System.IO.File.Exists(@newFileName)){
            if(newFileName=="kowalski-2021-11.json"){
                ReadReport read1=new ReadReport();
                read1.ReadFile();
                rep=read1.activityList;
            }
            else{
                ReadReport read=new ReadReport();
                read.ReadOtherFiles(newFileName);
                rep=read.activityList;
            }
            
        }
        else{
           FileStream fs = System.IO.File.Create(@newFileName);
           fs.Close();
        }
        Entry entry1=new Entry();
        entry1.date=selectDate.ToString("yyyy-MM-dd")+" ";
        entry1.time=time;
        entry1.code=projectCode+" ";
        entry1.description=description+" ";
        rep.Entries.Add(entry1);

        Write write=new Write();
        write.WriteOtherReport(newFileName,rep);

        return RedirectToAction("EntryDialog");
    }

    private void EditManager(string edittedActivity, string editmanager){
        ReadActivities read=new ReadActivities();
        foreach (ActivitiesModel model in read.activityList)
        {
            if(model.code.Contains(edittedActivity)){
                model.manager=editmanager+" ";
            }
        }
        JArray array=new JArray();
        foreach (ActivitiesModel model in read.activityList)
        {
            array.Add(JToken.FromObject(model));
        }
        Write write=new Write();
        write.WriteActivity(array);
    }
    private void EditName(string edittedActivity,string editname){
        ReadActivities read=new ReadActivities();
        foreach (ActivitiesModel model in read.activityList)
        {
            if(model.code.Contains(edittedActivity)){
                model.name=editname+" ";
            }
        }
        JArray array=new JArray();
        foreach (ActivitiesModel model in read.activityList)
        {
            array.Add(JToken.FromObject(model));
        }
        Write write=new Write();
        write.WriteActivity(array);
    }
    private void EditBudget(string edittedActivity,int editbudget){
        ReadActivities read=new ReadActivities();
        foreach (ActivitiesModel model in read.activityList)
        {
            if(model.code.Contains(edittedActivity)){
                model.budget=editbudget;
            }
        }
        JArray array=new JArray();
        foreach (ActivitiesModel model in read.activityList)
        {
            array.Add(JToken.FromObject(model));
        }
        Write write=new Write();
        write.WriteActivity(array);
    }

    [HttpGet]
    public ActionResult EditActivity(string edittedActivity, string editmanager,string editname,int editbudget){
        
        if(editmanager!=null){
            EditManager(edittedActivity,editmanager);
        }
        else if(editname!=null){
            EditName(edittedActivity,editname);
        }
        else if(editbudget!=0){
            EditBudget(edittedActivity,editbudget);
        }
        return RedirectToAction("EntryDialog");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
