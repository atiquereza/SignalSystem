using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using SignalSystem.Libs;
using SignalSystemApp.Libs;
//using SignalSystemApp.Libs;
using SignalSystemApp.Models;
using SignalSystemApp.Models.Telephone;

using SortDirection = System.Web.Helpers.SortDirection;

namespace SignalSystemApp.Controllers
{
    public class TelephoneController : Controller
    {
        // GET: Telephone

        [Authenticate]
        public ActionResult NewComplain()
        {
            return View();
        }
        [Authenticate]
        public ActionResult ListPhones()
        {

            return View();
        }
        [Authenticate]
        public ActionResult SearchActiveTelephoneInfo()
        {

            string telephoneNumber = Request["search"].ToString();

            Telephone telephone = new Telephone();
            Dictionary<string, string> userInfo = telephone.GetUserInfo(telephoneNumber);

            if (userInfo.Count == 0)
            {
                ViewData["Message"] = "Error! Telephone Number Not in use....";
                return View("NewComplain");
            }
            ViewData["AllPhoneInfoID"] = userInfo["AllPhoneInfoID"];
            ViewData["PhoneNumber"] = userInfo["PhoneNumber"];
            ViewData["BANumber"] = userInfo["BANumber"];
         
            ViewData["FullName"] = userInfo["FullName"];
            ViewData["PhoneUserPersoanlInfoID"] = userInfo["PhoneUserPersoanlInfoID"];
            ViewData["Rank"] = userInfo["Rank"];
            ViewData["Address"] = userInfo["Address"];



            ViewData["Message"] = "Searched Result";
            return View("NewComplain");
        }

        [Authenticate]
        public ActionResult SubmitComplains()
        {

            string phoneUserInfoId = Request["phoneUserInfoId"].ToString();
            string allPhoneInfoID = Request["allPhoneInfoID"].ToString();


            string complainTypeId = Request["type"].ToString();
            string description = Request["description"].ToString();

            Telephone aTelephone = new Telephone();
            bool success = aTelephone.SubmitNewComplain(phoneUserInfoId, allPhoneInfoID, description, complainTypeId);


            ViewData["Message"] = "Submitted Complains";
            return View("NewComplain");
        }
        [Authenticate]
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

            if (baNumber.Trim().Length == 0 || name.Trim().Length == 0 || rank.Trim().Length == 0 ||
                telephoneNumber.Trim().Length == 0
                || address.Trim().Length == 0 || sex.Trim().Length == 0)
            {
                ViewData["Message"] = "Error! Fields can not left blank";
                return View("AddTelephone");
            }

            string outputMessage = string.Empty;
            Telephone aTelephone = new Telephone();
            bool success = aTelephone.AssignNewTelephone(baNumber, name, rank, telephoneNumber, address, sex,
                out outputMessage);

            if (!success)
            {
                ViewData["Message"] = outputMessage;

                return View("AddTelephone");
            }


            ViewData["Message"] = "Telephone User Successfully Added";

            return View("AddTelephone");
        }
        [Authenticate]
        public ActionResult AddTelephone()
        {
            return View();
        }
        [Authenticate]
        public ActionResult PendingComplains()
        {
         
            return View();
        }
        [Authenticate]
        public ActionResult ResolvedComplains()
        {
            Telephone telephone = new Telephone();
            List<Dictionary<string, string>> resolved = new List<Dictionary<string, string>>();
            resolved = telephone.GetResolvedComplains();
            ViewData["Resolved"] = resolved;

            return View();
        }
        [Authenticate]
        public ActionResult SearchResolvedComplains()
        {
            string phoneNumber = Request["search"].ToString();
            Telephone telephone = new Telephone();
            List<Dictionary<string, string>> resolved = new List<Dictionary<string, string>>();
            resolved = telephone.GetResolvedComplainsByPhoneNumber(phoneNumber);
            ViewData["Resolved"] = resolved;

            return View("ResolvedComplains");
        }
        [Authenticate]
        public ActionResult GetSingleComplainData(string id)
        {
            List<Dictionary<string, string>> pendingComplains = new List<Dictionary<string, string>>();

            List<TelephoneComplain> aTelephoneComplainList = Telephone.GetTelephoneComplain(id,"pending");


            return Json(aTelephoneComplainList, JsonRequestBehavior.AllowGet);


        }
        [Authenticate]
        public ActionResult GetSingleResolvedComplainData(string id)
        {
            List<Dictionary<string, string>> pendingComplains = new List<Dictionary<string, string>>();

            List<TelephoneComplain> aTelephoneComplainList = Telephone.GetTelephoneComplain(id,"resolved");


            return Json(aTelephoneComplainList, JsonRequestBehavior.AllowGet);


        }


        [Authenticate]
        public ActionResult DeleteSingleComplainData()
        {
            List<Dictionary<string, string>> pendingComplains = new List<Dictionary<string, string>>();


            string id = Request["deleteComplainId"].ToString();
            Telephone.DeleteComplain(id);


            return RedirectToAction("PendingComplains", "Telephone");



        }



        [Authenticate]
        public ActionResult EditSingleComplainData(TelephoneComplain aTelephoneComplain)
        {
            string complainStatus = string.Empty;
            string updateQuery = string.Empty;
            Telephone.EditComplain(aTelephoneComplain, updateQuery);


            return RedirectToAction("PendingComplains", "Telephone");



        }

        [Authenticate]
        public JsonResult DataProviderAction(JQueryDataTableParamModel aModel)
        {
            List<string> columnlist =
                new List<string>(new string[]
                {"BANumber", "Name", "Rank", "NewPhoneNumber", "ComplainDate", "ComplainType"});
            string serachValue = Request["columns[0][search][value]"];

            var roleId = Request.Params["max"];
            var banumberFilter = Convert.ToString(Request["sSearch_0"]);
            var nameFilter = Convert.ToString(Request["sSearch_1"]);


            var rankFilter = Convert.ToString(Request["sSearch_2"]);
            var dateFilter = Convert.ToString(Request["sSearch_4"]);
            var phoneFilter = Convert.ToString(Request["sSearch_3"]);
            var complainFilter = Convert.ToString(Request["sSearch_5"]);
            var hourlyFilter = Convert.ToString(Request["sSearch_6"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);

            string sSearch = string.Empty;
            if (aModel.sSearch == null)
            {
                sSearch = "";
            }
            else
            {
                sSearch = aModel.sSearch.ToString();
            }


            var banameSortDirection = Request["sSortDir_0"];

            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;

            if (hourlyFilter == "")
            {

                if (dateFilter.Contains("-yadcf_delim-"))
                {
                    string a1 = dateFilter.Replace("-yadcf_delim-", "~");
                    //Split date range filters with ~
                    fromDate = a1.Split('~')[0] == "" ? DateTime.MinValue : Convert.ToDateTime(a1.Split('~')[0]);
                    toDate = a1.Split('~')[1] == "" ? DateTime.MaxValue : Convert.ToDateTime(a1.Split('~')[1]).AddHours(23.99);
                   // toDate = toDate.AddHours(23.99);
                }
            }
            else if (hourlyFilter.Trim() == "Last 12 Hour Data")
            {
                toDate = DateTime.Now;
                fromDate = DateTime.Now.AddHours(-12.5);

            }
            else
            {
                toDate = DateTime.Now;
                fromDate = DateTime.Now.AddHours(-24.5);
            }

            FilteredLists filteredComplaneList = Telephone.GetFilteredComplaneList(sSearch, banumberFilter,
                phoneFilter, nameFilter, rankFilter, complainFilter, fromDate, toDate, aModel.iDisplayStart, aModel.iDisplayLength, aModel.sSortDir_0, aModel.iSortCol_0);

          

            var result = from c in filteredComplaneList.aComplainList
                select new[]
                {

                    c.BANumber,
                    c.Name,
                    c.Rank,
                    c.NewPhoneNumber,
                    c.ComplainDate,
                    c.ComplainType,
                    "Edit",
                    "Delete",
                    Convert.ToString(c.ComplainId)
                };
            List<string[]> resultList = result.ToList();
          
            return Json(new
            {
                sEcho = aModel.sEcho,
                yadcf_data_0 = filteredComplaneList.BaNumberList,
                yadcf_data_2 = filteredComplaneList.RankList,
                yadcf_data_5 = filteredComplaneList.ComplainTypeList,

                iTotalRecords = filteredComplaneList.TotalSize,
                iTotalDisplayRecords = filteredComplaneList.FilteredSize,
                aaData = resultList
            },
                JsonRequestBehavior.AllowGet);




            //Extract only current page


        }





        [Authenticate]
        public ActionResult GetExcelFile(string baNumber, string name, string rank, string phone, string complainType,
            string fromDateRange, string toDateRange, string overAllSearch, string hourFilter, string status)
        {
            if (baNumber.ToLower() == "select ba")
            {
                baNumber = "";
            }

            if (rank.ToLower() == "select rank")
            {
                rank = "";
            }


            if (complainType.ToLower() == "select complain")
            {
                complainType = "";
            }
            if (hourFilter.ToLower() == "select hourly data")
            {
                hourFilter = "";
            }
            if ((overAllSearch == null))
            {
                overAllSearch = "";
            }

            string a1 = baNumber + rank + complainType;
          
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;
            int fromLength = fromDateRange.Length;
            int toLenght = toDateRange.Length;
            if (hourFilter == "")
            {
                if (fromDateRange.Length > 0)
                {

                    fromDate = Convert.ToDateTime(fromDateRange);

                }
                else
                {
                    fromDate = DateTime.MinValue;
                }

                if (toDateRange.Length > 0)
                {

                    toDate = Convert.ToDateTime(toDateRange);

                }
                else
                {
                    toDate = DateTime.MaxValue;
                }
            }
            else if (hourFilter.Trim() == "Last 12 Hour Data")
            {
                toDate = DateTime.Now;
                fromDate = DateTime.Now.AddHours(-12.5);

            }
            else
            {
                toDate = DateTime.Now;
                fromDate = DateTime.Now.AddHours(-24.5);
            }

            FilteredLists filteredComplainsList = Telephone.GetFilteredComplaneList(overAllSearch,
                baNumber.Trim(), phone.Trim(), name.Trim(),
                rank.Trim(), complainType.Trim(), fromDate, toDate, 0, 9551615, "desc", 4);

            if (filteredComplainsList.aComplainList.Count > 0)
            {
                ExcelWriter.ExcelGenerator(filteredComplainsList.aComplainList);
            }

            return null;
        }


        [Authenticate]
        public JsonResult ResolvedDataProviderAction(JQueryDataTableParamModel aModel)
        {
            List<string> columnlist =
                new List<string>(new string[]
                {
                    "BANumber", "Name", "Rank", "NewPhoneNumber", "ComplainDate", "ComplainType", "ResolveBy",
                    "ResolvedDate", "ActionTaken", "Remarks"
                });
            var banumberFilter = Convert.ToString(Request["sSearch_0"]);
            var nameFilter = Convert.ToString(Request["sSearch_1"]);
            var rankFilter = Convert.ToString(Request["sSearch_2"]).Trim();
            var dateFilter = Convert.ToString(Request["sSearch_4"]);
            var phoneFilter = Convert.ToString(Request["sSearch_3"]).Trim();
            var complainFilter = Convert.ToString(Request["sSearch_5"]).Trim();
            var hourlyFilter = Convert.ToString(Request["sSearch_6"]).Trim();
            var resolverFilter = Convert.ToString(Request["sSearch_6"]).Trim();
            var resolveDateFilter = Convert.ToString(Request["sSearch_7"]);
            var actionTakenFilter = Convert.ToString(Request["sSearch_8"]).Trim();
            var remarksFilter = Convert.ToString(Request["sSearch_9"]).Trim();
            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);

            string sSearch = string.Empty;
            if (aModel.sSearch == null)
            {
                sSearch = "";
            }
            else
            {
                sSearch = aModel.sSearch.ToString();
            }


            var banameSortDirection = Request["sSortDir_0"];

            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;

            if (dateFilter.Contains("-yadcf_delim-"))
            {
                string a1 = dateFilter.Replace("-yadcf_delim-", "~");
                //Split date range filters with ~
                fromDate = a1.Split('~')[0] == "" ? DateTime.MinValue : Convert.ToDateTime(a1.Split('~')[0]);
                toDate = a1.Split('~')[1] == "" ? DateTime.MaxValue : Convert.ToDateTime(a1.Split('~')[1]).AddHours(23.99);
            
            }

            DateTime resolvefromDate = DateTime.MinValue;
            DateTime resolvetoDate = DateTime.MaxValue;
            if (resolveDateFilter.Contains("-yadcf_delim-"))
            {
                string b1 = resolveDateFilter.Replace("-yadcf_delim-", "~");
                //Split date range filters with ~
                resolvefromDate = b1.Split('~')[0] == "" ? DateTime.MinValue : Convert.ToDateTime(b1.Split('~')[0]);
                resolvetoDate = b1.Split('~')[1] == "" ? DateTime.MaxValue : Convert.ToDateTime(b1.Split('~')[1]).AddHours(23.99);
            }

            FilteredLists filteredComplaneList = Telephone.GetResolvedFilteredComplaneList(sSearch,
                banumberFilter, phoneFilter, nameFilter, rankFilter, complainFilter, fromDate, toDate, resolverFilter,
                resolvefromDate, resolvetoDate, actionTakenFilter, remarksFilter,aModel.iDisplayStart,aModel.iDisplayLength,aModel.sSortDir_0,aModel.iSortCol_0);

            var result = from c in filteredComplaneList.aComplainList
                select new[]
                {

                    c.BANumber,
                    c.Name,
                    c.Rank,
                    c.NewPhoneNumber,
                    c.ComplainDate,
                    c.ComplainType,
                    c.ResolveBy,
                    c.ResolvedDate,
                    c.ActionTaken,
                    c.Remarks,
                    "View",
                    Convert.ToString(c.ComplainId)
                };
            List<string[]> resultList = result.ToList();
            
            return Json(new
            {
                sEcho = aModel.sEcho,
                yadcf_data_0 = filteredComplaneList.BaNumberList,
                yadcf_data_2 = filteredComplaneList.RankList,
                yadcf_data_5 = filteredComplaneList.ComplainTypeList,
                yadcf_data_6 = filteredComplaneList.ResolverList,

                //iTotalRecords = complanList.Count(),
                iTotalRecords = filteredComplaneList.TotalSize,
                iTotalDisplayRecords = filteredComplaneList.FilteredSize,
                aaData = resultList
            },
                JsonRequestBehavior.AllowGet);
            //Extract only current page
            }


        [Authenticate]
        public ActionResult GetResolveExcelFile(string baNumber, string name, string rank, string phone,
            string complainType, string fromDateRange, string toDateRange, string overAllSearch, string resolvedBy,
            string resolvefromDateRange, string resolvetoDateRange, string actionTaken, string remarks, string status)
        {
            if (baNumber.ToLower() == "select ba")
            {
                baNumber = "";
            }

            if (rank.ToLower() == "select rank")
            {
                rank = "";
            }
            
            if (complainType.ToLower() == "select complain")
            {
                complainType = "";
            }
            if (resolvedBy.ToLower() == "select resolver")
            {
                resolvedBy = "";
            }
            if ((overAllSearch == null))
            {
                overAllSearch = "";
            }
            
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;
            
            if (fromDateRange.Length > 0)
            {

                fromDate = Convert.ToDateTime(fromDateRange);

            }
            else
            {
                fromDate = DateTime.MinValue;
            }

            if (toDateRange.Length > 0)
            {

                toDate = Convert.ToDateTime(toDateRange);

            }
            else
            {
                toDate = DateTime.MaxValue;
            }

            DateTime resolveFromDate = DateTime.MinValue;
            DateTime resolveToDate = DateTime.MaxValue;
            
            if (resolvefromDateRange.Length > 0)
            {
                resolveFromDate = Convert.ToDateTime(resolvefromDateRange);
            }
            else
            {
                resolveFromDate = DateTime.MinValue;
            }

            if (resolvetoDateRange.Length > 0)
            {

                resolveToDate = Convert.ToDateTime(resolvetoDateRange);

            }
            else
            {
                resolveToDate = DateTime.MaxValue;
            }

            FilteredLists filteredComplainsList = Telephone.GetResolvedFilteredComplaneList(overAllSearch,
                baNumber, phone, name, rank, complainType, fromDate, toDate, resolvedBy, resolveFromDate, resolveToDate,
                actionTaken, remarks, 0, 9551615,"desc", 7);

            if (filteredComplainsList.aComplainList.Count>0)
            {
                
                ExcelWriter.ExcelGenerator(filteredComplainsList.aComplainList);
            }

    return null;
        }



        [Authenticate]
        public JsonResult TelephoneDataProviderAction(JQueryDataTableParamModel aModel)
        {
            List<string> columnlist =
                new List<string>(new string[]
                {
                    "BANumber", "Name", "Rank", "NewPhoneNumber", "Address", "Gender", "Status", "ConnectedDate",
                    "DisconnectedDate"
                });
            string serachValue = Request["columns[0][search][value]"];

            var roleId = Request.Params["max"];
            var banumberFilter = Convert.ToString(Request["sSearch_0"]).Trim();
            var nameFilter = Convert.ToString(Request["sSearch_1"]).Trim();
            
            var rankFilter = Convert.ToString(Request["sSearch_2"]).Trim();
            var phoneFilter = Convert.ToString(Request["sSearch_3"]).Trim();
           // var addressFilter = Convert.ToString(Request["sSearch_4"]).Trim();
            var phoneTypeFilter = Convert.ToString(Request["sSearch_4"]).Trim();
            var homeAddressFilter = Convert.ToString(Request["sSearch_5"]).Trim();
            var officeAddressFilter = Convert.ToString(Request["sSearch_6"]);
            var genderFilter = Convert.ToString(Request["sSearch_7"]);
           
            var complainFilter = Convert.ToString(Request["sSearch_5"]);
            var hourlyFilter = Convert.ToString(Request["sSearch_8"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);

            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;
            if (hourlyFilter.Contains("-yadcf_delim-"))
            {
                string a1 = hourlyFilter.Replace("-yadcf_delim-", "~");
                //Split date range filters with ~
                fromDate = a1.Split('~')[0] == "" ? DateTime.MinValue : Convert.ToDateTime(a1.Split('~')[0]);
                toDate = a1.Split('~')[1] == "" ? DateTime.MaxValue : Convert.ToDateTime(a1.Split('~')[1]).AddHours(23.99);

            }

            string sSearch = string.Empty;
            if (aModel.sSearch == null)
            {
                sSearch = "";
            }
            else
            {
                sSearch = aModel.sSearch.ToString();
            }


            var banameSortDirection = Request["sSortDir_0"];

          
      
            TelphoneUser aTelephoneUser = new TelphoneUser();
           // List<TelphoneUser> aUsersList = aTelephoneUser.GetTelphoneUsers();
            Telephone aTelephone = new Telephone();


            FilteredLists filterTelphoneUsers = aTelephone.GetFilteredPhoneList(sSearch, banumberFilter, phoneFilter, nameFilter, rankFilter, homeAddressFilter, officeAddressFilter, phoneTypeFilter, genderFilter, fromDate, toDate, aModel.iDisplayStart, aModel.iDisplayLength, aModel.sSortDir_0, aModel.iSortCol_0);


            var result = from c in filterTelphoneUsers.aTelePhoneUserList
                select new[]
                {

                    c.BANumber,
                    c.Name,
                    c.Rank,
                    c.NewPhoneNumber,
                    c.PhoneType,
                    c.HomeAddress,
                    c.OfficeAddress,
                    c.Gender,
                   
                    c.ConnectedDate,
                    "Edit",

                    Convert.ToString(c.ID)
                };
            List<string[]> resultList = result.ToList();
            List<string> aRankList = new List<string>();
            List<string> banumberList = new List<string>();
            List<string> complainTypeList = new List<string>();
            List<string> statusList=new List<string>();


            return Json(new
            {
                sEcho = aModel.sEcho,
                yadcf_data_0 = filterTelphoneUsers.BaNumberList,
                yadcf_data_2 = filterTelphoneUsers.RankList,
                yadcf_data_4 = filterTelphoneUsers.PhoneTypeList,
               // yadcf_data_6 = statusList,

                iTotalRecords = filterTelphoneUsers.TotalSize,
                iTotalDisplayRecords = filterTelphoneUsers.FilteredSize,
                aaData = resultList
            },
             JsonRequestBehavior.AllowGet);
    }


        [Authenticate]
        public ActionResult GetSinglePhoneData(string id)
        {
            List<TelphoneUser> phoneList = TelphoneUser.GetPhoneUserData(id);


            return Json(phoneList, JsonRequestBehavior.AllowGet);
        }


        [Authenticate]
        public ActionResult PhoneConnectDeleteAction(TelphoneUser aTelphoneUser)
        {
           
            string complainStatus = string.Empty;
            string query = string.Empty;
            string status = Request["Status"];
            string id = Request["Connect_PhoneId"];
            string remarks = Request["Connect_Remarks"];

            TelphoneUser.UpdateDeletePhoneUserData(status, id, remarks);
            return RedirectToAction("ListPhones", "Telephone");



        }

        [Authenticate]
        public ActionResult PhoneDisconnect(TelphoneUser aTelphoneUser)
        {
            DBGateway aGateway = new DBGateway();
            string complainStatus = string.Empty;
            string query = string.Empty;
         
            string id = Request["Disconnect_PhoneId"];
            string remarks = Request["Disconnect_Remarks"];


            TelphoneUser.DisconnectPhoneOperation(id, aGateway, remarks);


            return RedirectToAction("ListPhones", "Telephone");



        }

        
    }
}