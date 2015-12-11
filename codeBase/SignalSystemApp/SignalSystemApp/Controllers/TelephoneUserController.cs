using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalSystemApp.Models.TelephoneUser;

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

        [HttpPost]
        public ActionResult AddNewTelephoneUser(TelephoneUserInfo aTelephoneUserInfo)
        {
            if (aTelephoneUserInfo.BANumber.Trim().Length == 0
                ||aTelephoneUserInfo.Appointment.Trim().Length == 0
                || aTelephoneUserInfo.EmailAddress.Trim().Length == 0
                || aTelephoneUserInfo.FullName.Trim().Length == 0
                || aTelephoneUserInfo.JoiningDate.ToString("yyyy-MM-dd") == "0001-01-01"
                || aTelephoneUserInfo.MaritalStatus.Trim().Length == 0
                || aTelephoneUserInfo.OfficeAddress.Trim().Length == 0
                || aTelephoneUserInfo.PermanentAddress.Trim().Length == 0
                || aTelephoneUserInfo.PlateName.Trim().Length== 0
                || aTelephoneUserInfo.PresentAddress.Trim().Length ==0
                || aTelephoneUserInfo.RankID.Trim().Length == 0
                || aTelephoneUserInfo.ServiceStatus.Trim().Length == 0
                || aTelephoneUserInfo.Sex.Trim().Length == 0
                || aTelephoneUserInfo.Unit.Trim().Length == 0)


            {
                ViewData["Message"] = "Error! Please Set All Fields";
                return View("Index");
            }

            if (aTelephoneUserInfo.ServiceStatus.Trim().ToLower()=="LPR"
                && aTelephoneUserInfo.LPRDate.ToString("yyyy-MM-dd") == "0001-01-01")
            {
                ViewData["Message"] = "Error! Please Set LPR Date Fields";
                return View("Index");
            }

            TelephoneUser aUser = new TelephoneUser();
            string message = aUser.AddTelephoneUser(aTelephoneUserInfo);

            ViewData["Message"] = message;
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