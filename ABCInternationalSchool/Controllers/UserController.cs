using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABCInternationalSchool.Models;

namespace ABCInternationalSchool.Controllers
{
    public class UserController : Controller
    {
        OAF_DBEntities oaf = new OAF_DBEntities();
        // GET: User
        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert(Application app, Processed_applications pa)
        {
            Application ap = new Application();

            ap.Branch_id = int.Parse(Request.Form["ddlbranch"].ToString());

            ap.Class_id = int.Parse(Request.Form["ddlclass"].ToString());

            ap.Name = Request.Form["txtname"].ToString();

            ap.Age = int.Parse(Request.Form["txtage"].ToString());

            ap.DOB = DateTime.Parse(Request.Form["dtdate"].ToString());

            ap.Address = Request.Form["txtadd"].ToString();

            pa.Comments = "Not yet Processed,check later";
            oaf.Processed_applications.Add(pa);
            oaf.Applications.Add(ap);
            var res = oaf.SaveChanges();

            if (res > 0)
              
                Response.Write("Your Application has been submitted, Application Id :" + ap.Id);
            return View();
        }
        [HttpGet]
        public ActionResult Status()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Status(Processed_applications pa)
        {
            int id = int.Parse(Request.Form["txtid"].ToString());
            var status = oaf.Processed_applications.Where(x => x.App_id == id).SingleOrDefault();
            if (status != null)
            {
                if (status.Comments == "Processed")
                {
                    Response.Write("Your application is processed");
                }
                else
                {
                    Response.Write("Your application is yet to be resolved");
                }
            }
            else
                Response.Write("Invalid ID");
            
            return View();
        }
        [HttpGet]
        public ActionResult Process()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Process(string Command)
        {
            int id = int.Parse(Request.Form["txtid"].ToString());
            if (Command == "Status")
            {
                var status = oaf.Processed_applications.Where(x => x.App_id == id).SingleOrDefault();
                ViewData["id"] = status;
            }
            if (Command == "SUBMIT")
            {
               
                var status = oaf.Processed_applications.Where(x => x.App_id == id).SingleOrDefault();

                status.Comments = Request.Form["txtcomm"].ToString();

                status.Date_of_Resolve = DateTime.Now;
                var prstatus = (from t in oaf.Applications
                                join b in oaf.Branches on t.Branch_id equals b.Branch_id
                                where t.Id == id
                                select b.Contact_person).SingleOrDefault();
               
                status.ResolvedBy = prstatus;

                var res = oaf.SaveChanges();
                if (res > 0)
                    Response.Write("Application resolved");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Report()
        {
            var data = from t in oaf.Applications
                       join p in oaf.Processed_applications on t.Id equals p.App_id
                       orderby t.Id ascending
                       select t;
            List<POCO> lst = new List<POCO>();

            foreach (var j in data)
            {
                POCO pm = new POCO();
                pm.Id = j.Id;
                pm.Name = j.Name;
                pm.Age = j.Age;
                pm.DOB = j.DOB;
                pm.Address = j.Address;
                pm.Branch_id = j.Branch_id;
                pm.Class_id = j.Class_id;

                lst.Add(pm);
            }
         
            return View(lst);

         
        }
        [HttpPost]
        public ActionResult Report(POCO poo)
        {
            var data2 = (from t in oaf.Processed_applications
                         where t.Comments == "Processed"
                         select t).ToList();
            Session["count"] = data2.Count();

            var dd = oaf.Applications.Count();
            Session["dd"] = dd;

            if (Request.Form["ddlpr"].ToString() == "Processed")
            {

                var data = from t in oaf.Applications
                           join p in oaf.Processed_applications on t.Id equals p.App_id
                           where p.Comments == "Processed"
                           orderby t.Id ascending
                           select t;
                List<POCO> lst = new List<POCO>();

                foreach (var j in data)
                {
                    POCO pm = new POCO();
                    pm.Id = j.Id;
                    pm.Name = j.Name;
                    pm.Age = j.Age;
                    pm.DOB = j.DOB;
                    pm.Address = j.Address;
                    pm.Branch_id = j.Branch_id;
                    pm.Class_id = j.Class_id;

                    lst.Add(pm);
                }
              
                return View(lst);
            }
            else
            {
                var data1 = (from t in oaf.Applications
                             join p in oaf.Processed_applications on t.Id equals p.App_id
                             where p.Comments != "Processed"
                             select t).ToList();

                Session["datta1"] = data1;
                List<POCO> lst = new List<POCO>();

                foreach (var j in data1)
                {
                    POCO pm = new POCO();
                    pm.Id = j.Id;
                    pm.Name = j.Name;
                    pm.Age = j.Age;
                    pm.DOB = j.DOB;
                    pm.Address = j.Address;
                    pm.Branch_id = j.Branch_id;
                    pm.Class_id = j.Class_id;

                    lst.Add(pm);
                }
                
                return View(lst);
            }
        }
    }
}
