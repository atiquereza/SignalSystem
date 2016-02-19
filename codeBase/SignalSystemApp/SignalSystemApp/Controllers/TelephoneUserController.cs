using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SignalSystem.Libs;
using SignalSystemApp.Models;
using SignalSystemApp.Models.TelephoneNumberAndUser;
using SignalSystemApp.Models.TelephoneUser;

namespace SignalSystemApp.Controllers
{
    public class TelephoneUserController : Controller
    {
        [Authenticate]
        public ActionResult Index()
        {
            //This function will add A new Person Info
            return View();
        }
        [Authenticate]
        public ActionResult AddTelephoneUser()
        {
            //dublicate mehtod like Index, this will remove later
            return View("Index");
        }
        [Authenticate]
        [HttpPost]
        public ActionResult AddNewTelephoneUser(TelephoneUserInfo aTelephoneUserInfo)
        {
            if (aTelephoneUserInfo.BANumber.Trim().Length == 0
                || aTelephoneUserInfo.Appointment.Trim().Length == 0
                || aTelephoneUserInfo.EmailAddress.Trim().Length == 0
                || aTelephoneUserInfo.FullName.Trim().Length == 0
                || aTelephoneUserInfo.JoiningDate.ToString("yyyy-MM-dd") == "0001-01-01"
                || aTelephoneUserInfo.MaritalStatus.Trim().Length == 0
                || aTelephoneUserInfo.OfficeAddress.Trim().Length == 0
                || aTelephoneUserInfo.PermanentAddress.Trim().Length == 0
                || aTelephoneUserInfo.PlateName.Trim().Length == 0
                || aTelephoneUserInfo.PresentAddress.Trim().Length == 0
                || aTelephoneUserInfo.RankID.Trim().Length == 0
                || aTelephoneUserInfo.ServiceStatus.Trim().Length == 0
                || aTelephoneUserInfo.Sex.Trim().Length == 0
                || aTelephoneUserInfo.Unit.Trim().Length == 0)


            {
                ViewData["Message"] = "Error! Please Set All Fields";
                return View("Index");
            }

            if (aTelephoneUserInfo.ServiceStatus.Trim().ToLower() == "LPR"
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
        [Authenticate]
        public ActionResult AddNewTelephoneNumber()
        {
            return View();
        }
        [Authenticate]
        [HttpPost]
        public ActionResult SubmitNewTelephoneNumber(TelephoneNumberInfo aTelephoneNumberInfo)
        {
            if (aTelephoneNumberInfo.ConnectionTypeID.Trim().Length == 0
                || aTelephoneNumberInfo.PhoneNumber.Trim().Length == 0)
            {
                ViewData["Message"] = "Error! Fill All Fields";
                return View("AddNewTelephoneNumber");
            }

            TelephoneNumber aNumber = new TelephoneNumber();
            string message = aNumber.AddNewTelephoneNumber(aTelephoneNumberInfo);
            ViewData["Message"] = message;
            return View("AddNewTelephoneNumber");
        }
        [Authenticate]
        public ActionResult ViewActiveTelephoneUsers()
        {
            return View();
        }
        [Authenticate]
        public ActionResult ViewTerminatedTelephoneHistory()
        {
            return View();
        }
        [Authenticate]
        public ActionResult GetActivePhoneDataProviderAction(JQueryDataTableParamModel aModel)
        {
            string baNumber = Convert.ToString(Request["sSearch_0"]);
            string name = Convert.ToString(Request["sSearch_1"]);
            string palteName = Convert.ToString(Request["sSearch_2"]);
            string requestDate = Convert.ToString(Request["sSearch_3"]);
            string phoneNumber = Convert.ToString(Request["sSearch_4"]);
            string rank = Convert.ToString(Request["sSearch_5"]);


            string[] dates = Regex.Split(requestDate, "-yadcf_delim-");
            string fromDate = string.Empty;
            string toDate = string.Empty;
            if (dates.Count() == 2)
            {
                fromDate = dates[0];
                toDate = dates[1];
            }
            if (dates.Count() == 1)
            {
                fromDate = dates[0];
            }
            TelephoneNumber aRequest = new TelephoneNumber();
            int totalRecords = 0;
            int filteredRecord = 0;
            List<string[]> terminatedHistory = aRequest.GetActivePhoneNumberDetails(out totalRecords, out filteredRecord,
                baNumber, name, palteName, fromDate, toDate, phoneNumber, rank, aModel.iDisplayStart,
                aModel.iDisplayLength);

            return Json(new
            {
                sEcho = aModel.sEcho,
                iTotalRecords = filteredRecord,
                iTotalDisplayRecords = totalRecords,
                aaData = terminatedHistory
            }, JsonRequestBehavior.AllowGet);
        }
    }

}
