using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
                DateTime dt = DateTime.ParseExact(dateArrival, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                aMailData.MailArrivalDate = dt;
                dt = DateTime.ParseExact(dateDepurture, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                aMailData.MailDepartureDate = dt;
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

            var mailId = Convert.ToString(Request["sSearch_0"]);
            var mailDescription = Convert.ToString(Request["sSearch_1"]);
            var mailFrom = Convert.ToString(Request["sSearch_2"]);
            var mailTo = Convert.ToString(Request["sSearch_3"]);
            var dateArrivalRange = Convert.ToString(Request["sSearch_4"]);
            string dateArrivalFrom = string.Empty;
            string dateArrivalTo = string.Empty;

            string[] dates = Regex.Split(dateArrivalRange, "-yadcf_delim-");
            if (dates.Count() == 2)
            {
                dateArrivalFrom = dates[0];
                dateArrivalTo = dates[1];
            }
            else if (dates.Count() == 1)
            {
                dateArrivalFrom = dates[0];
            }


            Mail aMail = new Mail();
            int totalRecords = 0;
            List<MailData> mailData = aMail.GetMailData(mailId,mailDescription,mailFrom,mailTo,
                dateArrivalFrom,dateArrivalTo,aModel.iDisplayStart,aModel.iDisplayLength,out totalRecords);
           

            var result = from aMailData in mailData
                select new[]
                {

                    aMailData.MailDBID,
                    aMailData.MailID,
                    aMailData.MailDescription,
                    aMailData.MailArrivalDate.ToString("yyyy-MM-dd").ToString() ,
                    aMailData.MailDepartureDate.ToString("yyyy-MM-dd").ToString(),
                    aMailData.MailFrom,
                    aMailData.MailTo,
                    ""

                };
            List<string[]> resultList = result.ToList();
          
            return Json(new
            {
                sEcho = aModel.sEcho,

                iTotalRecords = resultList.Count,
                iTotalDisplayRecords = totalRecords,
                aaData = resultList
            },
               JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMailInfo(int id)
        {
            Mail aMail = new Mail();
            List<MailData> mailData = new List<MailData>();

            mailData = aMail.SingleMailInfo(id);
            return Json(mailData);
        }

        public ActionResult EditEntry(MailData aMailData)
        {
            Mail aMail = new Mail();
            string message = aMail.EditMail(aMailData);
            return View("ListMails");
        }
    }
}