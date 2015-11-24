using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using SignalSystemApp.Models.Application;

namespace SignalSystemApp.Controllers
{
    public class ApplicationController : Controller
    {
        // GET: Application
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewComplainType()
        {
            try
            {
                string complain = Request["complainType"].ToString();

                if (complain.Trim().Length == 0)
                {
                    ViewData["message"] = "Error! Enter Valid Complain Type.";
                    return View("Index");
                }

                ApplicationConfiguration applicationConfiguration = new ApplicationConfiguration();
                string messasge = applicationConfiguration.AddNewComplainType(complain);



                ViewData["message"] = messasge;
                return View("Index");
            }
            catch (Exception exception)
            {
                ViewData["message"] = "Error! Exception Found: " + exception.Message;
                return View("Index");
            }
        }
    }
}