using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalSystem.Libs;
using SignalSystemApp.Models.TelephoneNumberAndUser;
using SignalSystemApp.Models.TelephoneRequest;
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
            string addressType = Request["AddressType"].Trim().ToString();
            string connectionType = Request["ConnectionType"].Trim().ToString();
            if (baNumber.Length == 0 || reqType.Length == 0 || reqType == "--Request Type--")
            {
                ViewData["Message"] = "Error! Complete Search Fields";
                return View("Index");
            }


            TelephoneNumber aTelephoneNumber = new TelephoneNumber();
            TelephoneUser aUser = new TelephoneUser();
            TelephoneUserInfo aInfo = aUser.GetTelephoneUserInfo(baNumber);

            if (aInfo.BANumber == null)
            {
                string url = UtilityLibrary.GetBaseURL() + "TelephoneUser/AddTelephoneUser";
                ViewData["Message"] = "Error! Telephone User not found. To Add Telephone User Click <a href=\""+url+"\" style=\"color: blue;\">Here</a>";
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
                return View("Index");
            }
           
            return View("Index");
        }

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
    }
}