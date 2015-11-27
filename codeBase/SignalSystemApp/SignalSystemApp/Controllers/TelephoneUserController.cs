using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalSystemApp.Controllers
{
    public class TelephoneUserController: Controller
    {
        public ActionResult Index()
        {
            //This function will add A new Person Info
            return View();
        }

        public ActionResult AddTelephoneUser()
        {   
            //dublicate mehtod like Index, this will remove later
            return View("Index");
        }

        public ActionResult AddNewTelephoneNumber()
        {
            return View();
        }

        public ActionResult ViewActiveTelephoneUsers()
        {
            return View();
        }
        public ActionResult ViewTerminatedTelephoneHistory()
        {
            return View();
        }

       
    }
}