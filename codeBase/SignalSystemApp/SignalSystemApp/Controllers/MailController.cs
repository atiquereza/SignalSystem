using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalSystemApp.Models;
using SignalSystemApp.Models.Mail;

namespace SignalSystemApp.Controllers
{
    public class MailController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListMails()
        {
            return View();
        }

        public ActionResult AddNewMail()
        {
            try
            {
                string mailId = Request["mailId"].ToString();
                string mailDescription = Request["mailDescription"].ToString();
                string dateArrival = Request["dateArrival"].ToString();
                string dateDepurture = Request["dateDepurture"].ToString();
                string mailFrom = Request["mailFrom"].ToString();
                string mailTo = Request["mailTo"].ToString();

                if (mailId.Trim().Length == 0 || mailDescription.Trim().Length == 0 || dateDepurture.Length == 0 || dateArrival.Length == 0 || mailFrom=="-1" || mailTo=="-1")
                {
                    ViewData["message"] = "Error! Fields should not be empty.";
                    return View("Index");
                }
                

                MailData aMailData = new MailData();
                Mail aMail = new Mail();
                if (!aMail.CheckMailID(mailId))
                {
                    ViewData["message"] = "Error! Mail ID should Be Unique";
                    return View("Index");
                }
                aMailData.MailID = mailId;
                aMailData.MailDescription = mailDescription;
                aMailData.MailFrom = mailFrom;
                aMailData.MailTo = mailTo;
              //  DateTime dateValue = DateTime.Parse(dateArrival);
                //DateTime dateValue = Convert.ToDateTime(dateArrival);
                DateTime dt = DateTime.ParseExact(dateArrival, "dd/MM/yyyy", CultureInfo.InvariantCulture);
               
                aMailData.MailArrival = dt;
                dt = DateTime.ParseExact(dateDepurture, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //dateValue = Convert.ToDateTime(dateDepurture);
                aMailData.MailDeparture = dt;
                string message = aMail.AddNewMail(aMailData);
                ViewData["message"] = message;
                return View("Index");
            }
            catch (Exception exception)
            {
                ViewData["message"] = "Error! " + exception.Message;
                return View("Index");
            }
        }

        public JsonResult DataProviderAction(JQueryDataTableParamModel aModel)
        {

            Mail aMail = new Mail();

            List<MailData> mailData = aMail.GetMailData();
            var idFilter = Convert.ToString(Request["sSearch_0"]);
            var mailIdFilter = Convert.ToString(Request["sSearch_1"]);
            var mailDescriptionFilter = Convert.ToString(Request["sSearch_2"]);
            var dateFilter = Convert.ToString(Request["sSearch_3"]);


            

            var result = from aMailData in mailData
                select new[]
                {

                    aMailData.ID,
                    aMailData.MailID,
                    aMailData.MailDescription,
                    aMailData.MailArrival.ToString("dd-MM-yyyy").ToString()

                };


            List<string[]> resultList = result.ToList();
            List<string> aRankList = new List<string>();
            List<string> banumberList = new List<string>();
            List<string> complainTypeList = new List<string>();
            return Json(new
            {
                sEcho = aModel.sEcho,

                iTotalRecords = 25,//resultList.Count,
                iTotalDisplayRecords = 12,//filteredComplaneList.Count(),
                aaData = resultList
            },
               JsonRequestBehavior.AllowGet);
        }
    }
}