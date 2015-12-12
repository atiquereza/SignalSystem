using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalSystemApp.Models.TelephoneNumberAndUser;
using SignalSystemApp.Models.TelephoneUser;

namespace SignalSystemApp.Controllers
{
    public class TelephoneRequestController : Controller
    {   
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PendingRequest()
        {
            return View();
        }
        public ActionResult ResolveRequest()
        {
            return View();
        }

        public ActionResult SearchRequestNewForm()
        {
            string baNumber = Request["BANumberSearch"].Trim().ToString();
            string reqType = Request["reqType"].Trim().ToString();
            if (baNumber.Length == 0 || reqType.Length == 0 || reqType == "--Request Type--")
            {
                ViewData["Message"] = "Error! Complete Search Fields";
                return View("Index");
            }


            TelephoneNumber aTelephoneNumber = new TelephoneNumber();
            TelephoneUser aUser = new TelephoneUser();
            TelephoneUserInfo aInfo = aUser.GetTelephoneUserInfo(baNumber);
            if (reqType == "1")
            { 
                
                ViewData["AvailableNumbers"] = aTelephoneNumber.GetAvailableTelephoneNumberForNewConnection();
                return View("NewConnecton");
            }            
            if (reqType == "2")
            {
                return View("NewConnecton");
            }
            if (reqType == "3")
            {
                return View("NewConnecton");
            }

            return View("Index");
        }
    }
}