using System;
using System.Collections.Generic;
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

                if (mailId.Trim().Length == 0 || mailDescription.Trim().Length == 0)
                {
                    ViewData["message"] = "Error! Fields shold not be empty.";
                    return View("Index");
                }

                Mail aMail = new Mail();
                string message = aMail.AddNewMail(mailId.Trim(), mailDescription.Trim());
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
                    aMailData.MailDate.ToString("dd-MM-yyyy").ToString()

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