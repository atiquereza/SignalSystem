using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalSystemApp.Models.Telephone;

namespace SignalSystemApp.Controllers
{
    public class TelephoneController : Controller
    {
        // GET: Telephone


        public ActionResult NewComplain()
        {
            return View();
        }

        public ActionResult SearchActiveTelephoneInfo()
        {

            string telephoneNumber = Request["search"].ToString();

            Telephone telephone = new Telephone();
            Dictionary<string,string> userInfo = telephone.GetUserInfo(telephoneNumber);

            if (userInfo.Count == 0)
            {
                ViewData["Message"] = "Error! Telephone Number Not in use....";
                return View("NewComplain");
            }
            ViewData["ID"] = userInfo["ID"];
            ViewData["Name"] = userInfo["Name"];
            ViewData["BANumber"] = userInfo["BANumber"];
            ViewData["TelephoneNumber"] = userInfo["NewPhoneNumber"];
            ViewData["Address"] = userInfo["Address"];
            ViewData["Rank"] = userInfo["Rank"];


            ViewData["Message"] = "Searched Result";
            return View("NewComplain");
        }


        public ActionResult SubmitComplains()
        {

            string userId = Request["userId"].ToString();

            string complainTypeId = Request["type"].ToString();
            string description = Request["description"].ToString();

            Telephone aTelephone = new Telephone();
            bool success = aTelephone.SubmitNewComplain(userId,description,complainTypeId);


            ViewData["Message"] = "Submitted Complains";
            return View("NewComplain");
        }
        public ActionResult AssignTelephone()
        {
            string baNumber = Request["baNumber"].ToString();
            string name = Request["name"].ToString();
            string rank = Request["rank"].ToString();
            string telephoneNumber = Request["telephoneNumber"].ToString();
            string address = Request["address"].ToString();
            string sex = string.Empty;
            if (Request["sex"] != null)
            {
                sex = Request["sex"].ToString();
            }

            if (baNumber.Trim().Length == 0 || name.Trim().Length == 0 || rank.Trim().Length == 0 || telephoneNumber.Trim().Length == 0
               || address.Trim().Length == 0 || sex.Trim().Length == 0)
            {
                ViewData["Message"] = "Error! Fields can not left blank";
                return View("AddTelephone");
            }

            string outputMessage = string.Empty;
            Telephone aTelephone = new Telephone();
            bool success = aTelephone.AssignNewTelephone(baNumber, name, rank, telephoneNumber, address, sex, out outputMessage);

            if (!success)
            {
                ViewData["Message"] = outputMessage;

                return View("AddTelephone");
            }


            ViewData["Message"] = "Telephone User Successfully Added";

            return View("AddTelephone");
        }

        public ActionResult AddTelephone()
        {
            return View();
        }

        public ActionResult PendingComplains()
        {
            Telephone telephone = new Telephone();
            
            List<Dictionary<string,string>> pendingComplains = new List<Dictionary<string, string>>();
            pendingComplains = telephone.GetPendingComplains();
            ViewData["PendingComplains"] = pendingComplains;
            return View();
        }

        public ActionResult ResolvedComplains()
        {
            Telephone telephone = new Telephone();
            List<Dictionary<string, string>> resolved = new List<Dictionary<string, string>>();
            resolved = telephone.GetResolvedComplains();
            ViewData["Resolved"] = resolved;

            return View();
        }
       
        public ActionResult SearchResolvedComplains()
        {
            string phoneNumber = Request["search"].ToString();
            Telephone telephone = new Telephone();
            List<Dictionary<string, string>> resolved = new List<Dictionary<string, string>>();
            resolved = telephone.GetResolvedComplainsByPhoneNumber(phoneNumber);
            ViewData["Resolved"] = resolved;

            return View("ResolvedComplains");
        }
    }
}