using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SignalSystem.Libs;
using SignalSystemApp.Models;
using SignalSystemApp.Models.TelephoneNumberAndUser;
using SignalSystemApp.Models.TelephoneRequest;
using SignalSystemApp.Models.TelephoneUser;

namespace SignalSystemApp.Controllers
{
    public class TelephoneRequestController : Controller
    {
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }
        [Authenticate]
        public ActionResult PendingRequest()
        {
            return View();
        }
        [Authenticate]
        public ActionResult ResolveRequest()
        {
            return View();
        }
        [Authenticate]
        public ActionResult SearchRequestNewForm()
        {
            string baNumber = Request["BANumberSearch"].Trim().ToString();
            string reqType = Request["reqType"].Trim().ToString();
            string addressType = Request["AddressType"].Trim().ToString();
            string connectionType = Request["ConnectionType"].Trim().ToString();
            if (baNumber.Length == 0 || reqType.Length == 0 || reqType == "--Request Type--")
            {
                ViewData["Message"] = "Error! Complete Search Fields";
                ViewData["baNumber"] = baNumber;
                ViewData["reqType"] = reqType;
                ViewData["addressType"] = addressType;
                ViewData["connectionType"] = connectionType;
               
                return View("Index");
            }


            TelephoneNumber aTelephoneNumber = new TelephoneNumber();
            TelephoneUser aUser = new TelephoneUser();
            TelephoneUserInfo aInfo = aUser.GetTelephoneUserInfo(baNumber);

            if (aInfo.BANumber == null)
            {
                string url = UtilityLibrary.GetBaseURL() + "TelephoneUser/AddTelephoneUser";
                ViewData["Message"] = "Error! Telephone User not found. To Add Telephone User Click <a href=\""+url+"\" style=\"color: blue;\">Here</a>";
                ViewData["baNumber"] = baNumber;
                ViewData["reqType"] = reqType;
                ViewData["addressType"] = addressType;
                ViewData["connectionType"] = connectionType;
                return View("Index");
            }


            ViewData["UserInfo"] = aInfo;
            ViewData["AddressType"] = addressType;
            ViewData["ConnectionType"] = connectionType;
            
            if (reqType == "1")
            {
                if (addressType == "Office")
                {
                    ViewData["Address"] = aInfo.OfficeAddress;
                }
                else
                {
                    ViewData["Address"] = aInfo.PresentAddress;
                }
                ViewData["RequestType"] = "New Connection";
                ViewData["AvailableNumbers"] = aTelephoneNumber.GetAvailableTelephoneNumberForNewConnection(connectionType);
                ViewData["baNumber"] = baNumber;
                ViewData["reqType"] = reqType;
                ViewData["addressType"] = addressType;
                ViewData["connectionType"] = connectionType;
                return View("Index");
            }
            if (reqType == "2")
            {
                if (addressType == "Office")
                {
                    ViewData["Address"] = aInfo.OfficeAddress;
                }
                else
                {
                    ViewData["Address"] = aInfo.PresentAddress;
                }
                ViewData["RequestType"] = "Shifting";
                ViewData["AvailableNumbers"] = aTelephoneNumber.GetAllotedNumbersForExistingUser(connectionType,addressType,baNumber);
                ViewData["baNumber"] = baNumber;
                ViewData["reqType"] = reqType;
                ViewData["addressType"] = addressType;
                ViewData["connectionType"] = connectionType;
                return View("Index");
            }
            if (reqType == "3")
            {
                if (addressType == "Office")
                {
                    ViewData["Address"] = aInfo.OfficeAddress;
                }
                else
                {
                    ViewData["Address"] = aInfo.PresentAddress;
                }
                ViewData["RequestType"] = "Termination";
                ViewData["AvailableNumbers"] = aTelephoneNumber.GetAllotedNumbersForExistingUser(connectionType, addressType, baNumber);
                ViewData["baNumber"] = baNumber;
                ViewData["reqType"] = reqType;
                ViewData["addressType"] = addressType;
                ViewData["connectionType"] = connectionType;
                return View("Index");
            }

            ViewData["baNumber"] = baNumber;
            ViewData["reqType"] = reqType;
            ViewData["addressType"] = addressType;
            ViewData["connectionType"] = connectionType;
            return View("Index");


        }
        [Authenticate]
        public ActionResult AddNewRequest()
        {
           



            string BANumberID = Request["BANumberID"].ToString().Trim();

            if (BANumberID.Trim().Length == 0)
            {
                ViewData["Message"] = "Error! Complete Search Fields";
                return View("Index");
            }
            string BANumber = Request["BANumber"].ToString().Trim();
            string ApplicationNumber = Request["ApplicationNumber"].ToString().Trim();
            string ConnectionType = Request["ConnectionType"].ToString().Trim();
            string AddressType = Request["AddressType"].ToString().Trim();
            string PhoneNumber = Request["PhoneNumber"].ToString().Trim();
            string Address = Request["Address"].ToString().Trim();
            string Comment = Request["Comment"].ToString().Trim();
            string RequestType = Request["RequestType"].ToString().Trim();


            ViewData["baNumber"] = BANumber;
            ViewData["reqType"] = RequestType;
            ViewData["addressType"] = AddressType;
            ViewData["connectionType"] = ConnectionType;

            TelephoneRequestInfo aInfo = new TelephoneRequestInfo()
            {
                PhoneUserPersonalInfoID = Convert.ToInt32(BANumberID),
                LetterNo = ApplicationNumber,
                AllPhoneInfoID =  Convert.ToInt32(PhoneNumber),
                AddressType = AddressType,
                Address = Address ,
                Comment = Comment,
                RequestDate = DateTime.Now
            };

            string message = string.Empty;
            if (RequestType.ToLower() == "new connection")
            {
                TelephoneRequest aTelephoneRequest = new TelephoneRequest();
                message = aTelephoneRequest.AddNewConnectionRequest(aInfo);
            }

            if (RequestType.ToLower() == "shifting")
            {
                TelephoneRequest aTelephoneRequest = new TelephoneRequest();
                message = aTelephoneRequest.AddShiftingRequest(aInfo);
            }

            if (RequestType.ToLower() == "termination")
            {
                TelephoneRequest aTelephoneRequest = new TelephoneRequest();
                message = aTelephoneRequest.AddTerminationRequest(aInfo);
            }


            ViewData["Message"] = message;
            return View("Index");
        }
        [Authenticate]
        public ActionResult ResolveActiveRequest()
        {

            string requestId = Request["RequestId"].ToString();
            string resolveDate = Request["ResolveDate"].ToString();
            string resolvedBy = Request["ResolveBy"].ToString();
            string actionTaken = Request["ActionTaken"].ToString();

            TelephoneRequest aRequest = new TelephoneRequest();
            string message = aRequest.ResolveRequest(requestId, resolveDate, resolvedBy,actionTaken);

            return View("PendingRequest");
        }

        [Authenticate]
        public ActionResult PendingDataProviderAction(JQueryDataTableParamModel aModel)
        {  
            string baNumber = Convert.ToString(Request["sSearch_0"]);
            string name = Convert.ToString(Request["sSearch_1"]);
            string letterNumber = Convert.ToString(Request["sSearch_2"]);
            string requestDate = Convert.ToString(Request["sSearch_3"]);
            string phoneNumber = Convert.ToString(Request["sSearch_4"]);
            string requestType = Convert.ToString(Request["sSearch_5"]);

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
            TelephoneRequest aRequest = new TelephoneRequest();
            int totalRecords = 0;
            int filteredRecord = 0;
            List<string[]> pendingRequest = aRequest.GetPendingRequest(out totalRecords,out filteredRecord,
                baNumber,name,letterNumber,fromDate,toDate,phoneNumber,requestType,aModel.iDisplayStart,aModel.iDisplayLength);

            return Json(new
            {
                sEcho = aModel.sEcho,
                iTotalRecords = filteredRecord,
                iTotalDisplayRecords = totalRecords ,
                aaData = pendingRequest
            }, JsonRequestBehavior.AllowGet);
  
        }
        [Authenticate]
        public ActionResult ResolvedDataProviderAction(JQueryDataTableParamModel aModel)
        {
            string baNumber = Convert.ToString(Request["sSearch_0"]);
            string name = Convert.ToString(Request["sSearch_1"]);
            string letterNumber = Convert.ToString(Request["sSearch_2"]);
            string requestDate = Convert.ToString(Request["sSearch_3"]);
            string phoneNumber = Convert.ToString(Request["sSearch_4"]);
            string requestType = Convert.ToString(Request["sSearch_5"]);
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
            TelephoneRequest aRequest = new TelephoneRequest();
            int totalRecords = 0;
            int filteredRecord = 0;
            List<string[]> resolvedReqest = aRequest.GetResolveRequest(out totalRecords, out filteredRecord, baNumber, name, letterNumber, fromDate, toDate,phoneNumber,requestType ,aModel.iDisplayStart, aModel.iDisplayLength);

            return Json(new
            {
                sEcho = aModel.sEcho,
                iTotalRecords = filteredRecord,
                iTotalDisplayRecords = totalRecords,
                aaData = resolvedReqest
            }, JsonRequestBehavior.AllowGet);
        }
        [Authenticate]
        public ActionResult TerminatedConnectionDataProviderAction(JQueryDataTableParamModel aModel)
        {
            string baNumber = Convert.ToString(Request["sSearch_0"]);
            string name = Convert.ToString(Request["sSearch_1"]);
            string letterNumber = Convert.ToString(Request["sSearch_2"]);
            string requestDate = Convert.ToString(Request["sSearch_3"]);
            string phoneNumber = Convert.ToString(Request["sSearch_4"]);
           
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
            TelephoneRequest aRequest = new TelephoneRequest();
            int totalRecords = 0;
            int filteredRecord = 0;
            List<string[]> terminatedHistory = aRequest.GetResolveRequest(out totalRecords, out filteredRecord, baNumber, name, letterNumber, fromDate, toDate, phoneNumber, "Termination", aModel.iDisplayStart, aModel.iDisplayLength);

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