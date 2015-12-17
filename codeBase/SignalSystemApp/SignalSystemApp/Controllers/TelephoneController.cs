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
//using SignalSystemApp.Libs;
using SignalSystemApp.Models;
using SignalSystemApp.Models.Telephone;

using SortDirection = System.Web.Helpers.SortDirection;

namespace SignalSystemApp.Controllers
{
    public class TelephoneController : Controller
    {
        // GET: Telephone


        public ActionResult NewComplain()
        {
            return View();
        }

        public ActionResult ListPhones()
        {

            return View();
        }

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

        public ActionResult AddTelephone()
        {
            return View();
        }

        public ActionResult PendingComplains()
        {
            Telephone telephone = new Telephone();

            List<Dictionary<string, string>> pendingComplains = new List<Dictionary<string, string>>();
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

        public ActionResult GetSingleComplainData(string id)
        {
            List<Dictionary<string, string>> pendingComplains = new List<Dictionary<string, string>>();

            List<TelephoneComplain> aTelephoneComplainList = Telephone.GetTelephoneComplain(id);


            return Json(aTelephoneComplainList, JsonRequestBehavior.AllowGet);


        }

        

        public ActionResult DeleteSingleComplainData()
        {
            List<Dictionary<string, string>> pendingComplains = new List<Dictionary<string, string>>();


            string id = Request["deleteComplainId"].ToString();
            Telephone.DeleteComplain(id);


            return RedirectToAction("PendingComplains", "Telephone");



        }

        


        public ActionResult EditSingleComplainData(TelephoneComplain aTelephoneComplain)
        {
            string complainStatus = string.Empty;
            string updateQuery = string.Empty;
            Telephone.EditComplain(aTelephoneComplain, updateQuery);


            return RedirectToAction("PendingComplains", "Telephone");



        }

        
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
                    toDate = a1.Split('~')[1] == "" ? DateTime.MaxValue : Convert.ToDateTime(a1.Split('~')[1]);
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

            Telephone aTelephone = new Telephone();
            List<TelephoneComplain> complanList = aTelephone.GetVariousComplainList("pending");




            List<TelephoneComplain> filteredComplaneList = Telephone.GetFilteredComplaneList(sSearch, complanList, banumberFilter,
                phoneFilter, nameFilter, rankFilter, complainFilter, fromDate, toDate);

            //SortList.GetSortedList(aModel, filteredComplaneList, columnlist);




            List<TelephoneComplain> displayedCompanies =
                filteredComplaneList.Skip(aModel.iDisplayStart).Take(aModel.iDisplayLength).ToList();


            var result = from c in displayedCompanies
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
            List<string> aRankList = new List<string>();
            List<string> banumberList = new List<string>();
            List<string> complainTypeList = new List<string>();


            foreach (TelephoneComplain a1 in complanList)
            {
                if (!aRankList.Contains(a1.Rank))
                {
                    aRankList.Add(a1.Rank);
                }
                if (!banumberList.Contains(a1.BANumber))
                {
                    banumberList.Add(a1.BANumber);

                }
                if (!complainTypeList.Contains(a1.ComplainType))
                {
                    complainTypeList.Add(a1.ComplainType);
                }

            }




            return Json(new
            {
                sEcho = aModel.sEcho,
                yadcf_data_0 = banumberList,
                yadcf_data_2 = aRankList,
                yadcf_data_5 = complainTypeList,

                iTotalRecords = complanList.Count(),
                iTotalDisplayRecords = filteredComplaneList.Count(),
                aaData = resultList
            },
                JsonRequestBehavior.AllowGet);




            //Extract only current page


        }


        public List<TelphoneUser> GetFilteredPhoneList(string sSearch, List<TelphoneUser> phoneUserList,
            string banumberFilter, string phoneFilter,string nameFilter, string rankFilter, string addressFilter,string genderFilter,string statusFilter, DateTime fromDate, DateTime toDate)
        {
            List<TelphoneUser> searchedPhones;

            sSearch = sSearch.Trim();

            if ((sSearch == ""))
            {
                searchedPhones = phoneUserList;
            }
            else
            {
                searchedPhones =
                    phoneUserList.Where(c => c.BANumber.ToLower().Contains(sSearch.ToLower())
                                           || c.Rank.ToLower().Contains(sSearch.ToLower()) ||
                                           c.Name.ToLower().Contains(sSearch.ToLower()) ||
                                           c.NewPhoneNumber.ToLower().Contains(sSearch.ToLower()) ||
                                           c.Gender.ToLower().Contains(sSearch.ToLower()) ||
                                           c.PhoneStatus.ToLower().Contains(sSearch.ToLower()) ||
                                           c.Address.ToLower().Contains(sSearch.ToLower())).ToList();
            }


            var filteredPhones = searchedPhones
                .Where(c => (banumberFilter == "" || c.BANumber.Trim().ToLower() == banumberFilter.ToLower())
                            &&
                            (phoneFilter == "" || c.NewPhoneNumber.Trim().ToLower().Contains(phoneFilter))
                            &&
                            (nameFilter == "" || c.Name.Trim().ToLower().Contains(nameFilter.ToLower()))
                            &&
                            (rankFilter == "" || c.Rank.Trim().ToLower() == rankFilter.ToLower())
                            &&
                            (addressFilter == "" || c.Address.Trim().ToLower().Contains(addressFilter.ToLower()))
                            &&
                              (genderFilter == "" || c.Gender.Trim().ToLower() == genderFilter.ToLower())
                            &&
                              (statusFilter == "" || c.PhoneStatus.Trim().ToLower() == statusFilter.Trim().ToLower())
                            //&&

                            //(fromDate == DateTime.MinValue || fromDate < Convert.ToDateTime(c.ConnectedDate))
                            //&&
                            //(toDate == DateTime.MaxValue || Convert.ToDateTime(c.ConnectedDate) < toDate)
                );
            List<TelphoneUser> filteredPhoneList = filteredPhones.ToList();
            return filteredPhoneList;
        }



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
            Telephone aTelephone = new Telephone();
            List<TelephoneComplain> complainList = aTelephone.GetVariousComplainList(status);
            

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

            List<TelephoneComplain> filteredComplainsList = Telephone.GetFilteredComplaneList(overAllSearch, complainList,
                baNumber.Trim(), phone.Trim(), name.Trim(),
                rank.Trim(), complainType.Trim(), fromDate, toDate);

            if (filteredComplainsList.Count > 0)
            {

                DateTime dt = DateTime.Now;
                DateTime dDate = DateTime.Now;
                string[] sDate = dDate.ToString().Split(' ');
                string time = dt.ToString("hh:mm");


                GridView aGridView = new GridView();
                aGridView.DataSource = filteredComplainsList;
                aGridView.DataBind();


                foreach (TableCell cell in aGridView.HeaderRow.Cells)
                {
                    cell.BackColor = Color.Cornsilk;
                }

                foreach (GridViewRow row in aGridView.Rows)
                {
                    // row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex%2 == 0)
                        {
                            cell.BackColor = Color.Gainsboro;
                        }
                        else
                        {
                            cell.BackColor = Color.GhostWhite;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                Response.ClearContent();
                Response.AddHeader("content-disposition",
                    "attachment;filename=PendingTelephoneComplain_" + sDate[0] + "_" + sDate[1] + sDate[2] + ".xls");
                Response.ContentType = "application/excel";
                StringWriter swr = new StringWriter();
                HtmlTextWriter tw = new HtmlTextWriter(swr);
                aGridView.RenderControl(tw);
                Response.Write(swr.ToString());

                Response.End();
            }
            return null;
        }



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
                toDate = a1.Split('~')[1] == "" ? DateTime.MaxValue : Convert.ToDateTime(a1.Split('~')[1]);
            }

            DateTime resolvefromDate = DateTime.MinValue;
            DateTime resolvetoDate = DateTime.MaxValue;
            if (resolveDateFilter.Contains("-yadcf_delim-"))
            {
                string b1 = resolveDateFilter.Replace("-yadcf_delim-", "~");
                //Split date range filters with ~
                resolvefromDate = b1.Split('~')[0] == "" ? DateTime.MinValue : Convert.ToDateTime(b1.Split('~')[0]);
                resolvetoDate = b1.Split('~')[1] == "" ? DateTime.MaxValue : Convert.ToDateTime(b1.Split('~')[1]);
            }



            Telephone aTelephone = new Telephone();
            List<TelephoneComplain> complanList = aTelephone.GetVariousComplainList("resolved");




            List<TelephoneComplain> filteredComplaneList = Telephone.GetResolvedFilteredComplaneList(sSearch, complanList,
                banumberFilter, phoneFilter, nameFilter, rankFilter, complainFilter, fromDate, toDate, resolverFilter,
                resolvefromDate, resolvetoDate, actionTakenFilter, remarksFilter);

            //SortList.GetSortedList(aModel, filteredComplaneList, columnlist);

          
            List<TelephoneComplain> displayedCompanies =
                filteredComplaneList.Skip(aModel.iDisplayStart).Take(aModel.iDisplayLength).ToList();


            var result = from c in displayedCompanies
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
            List<string> aRankList = new List<string>();
            List<string> banumberList = new List<string>();
            List<string> complainTypeList = new List<string>();
            List<string> resolverList = new List<string>();



            foreach (TelephoneComplain a1 in complanList)
            {
                if (!aRankList.Contains(a1.Rank))
                {
                    aRankList.Add(a1.Rank);
                }
                if (!banumberList.Contains(a1.BANumber))
                {
                    banumberList.Add(a1.BANumber);

                }
                if (!complainTypeList.Contains(a1.ComplainType))
                {
                    complainTypeList.Add(a1.ComplainType);
                }
                if (!resolverList.Contains(a1.ResolveBy))
                {
                    resolverList.Add(a1.ResolveBy);
                }

            }




            return Json(new
            {
                sEcho = aModel.sEcho,
                yadcf_data_0 = banumberList,
                yadcf_data_2 = aRankList,
                yadcf_data_5 = complainTypeList,
                yadcf_data_6 = resolverList,

                iTotalRecords = complanList.Count(),
                iTotalDisplayRecords = filteredComplaneList.Count(),
                aaData = resultList
            },
                JsonRequestBehavior.AllowGet);




            //Extract only current page


        }



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

            string a1 = baNumber + rank + complainType;
            Telephone aTelephone = new Telephone();
            List<TelephoneComplain> complainList = aTelephone.GetVariousComplainList(status);


            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;

            int fromLength = fromDateRange.Length;
            int toLenght = toDateRange.Length;

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



            List<TelephoneComplain> filteredComplainsList = Telephone.GetResolvedFilteredComplaneList(overAllSearch, complainList,
                baNumber, phone, name, rank, complainType, fromDate, toDate, resolvedBy, resolveFromDate, resolveToDate,
                actionTaken, remarks);


            DateTime dt = DateTime.Now;
            DateTime dDate = DateTime.Now;
            string[] sDate = dDate.ToString().Split(' ');
            string time = dt.ToString("hh:mm");


            GridView aGridView = new GridView();
            aGridView.DataSource = filteredComplainsList;
            aGridView.DataBind();


            foreach (TableCell cell in aGridView.HeaderRow.Cells)
            {
                cell.BackColor = Color.Cornsilk;
            }

            foreach (GridViewRow row in aGridView.Rows)
            {

                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex%2 == 0)
                    {
                        cell.BackColor = Color.Gainsboro;
                    }
                    else
                    {
                        cell.BackColor = Color.GhostWhite;
                    }
                    cell.CssClass = "textmode";
                }
            }

            Response.ClearContent();
            Response.AddHeader("content-disposition",
                "attachment;filename=PendingTelephoneComplain_" + sDate[0] + "_" + sDate[1] + sDate[2] + ".xls");
            Response.ContentType = "application/excel";
            StringWriter swr = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(swr);
            aGridView.RenderControl(tw);
            Response.Write(swr.ToString());

            Response.End();
            return null;
        }







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
            var addressFilter = Convert.ToString(Request["sSearch_4"]).Trim();
            var genderFilter = Convert.ToString(Request["sSearch_5"]).Trim();
            var statusFilter = Convert.ToString(Request["sSearch_6"]).Trim();
            var connectedDateFilter = Convert.ToString(Request["sSearch_7"]);
           
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
      
            TelphoneUser aTelephoneUser = new TelphoneUser();
            List<TelphoneUser> aUsersList = aTelephoneUser.GetTelphoneUsers();
            Telephone aTelephone = new Telephone();
           

            List<TelphoneUser> filterTelphoneUsers = GetFilteredPhoneList(sSearch, aUsersList, banumberFilter, phoneFilter, nameFilter,
                rankFilter, addressFilter, genderFilter, statusFilter, fromDate, toDate);


            // List<TelephoneComplain> filteredComplaneList = GetFilteredComplaneList(sSearch, complanList, banumberFilter, phoneFilter, nameFilter, rankFilter, complainFilter, fromDate, toDate);


            //SortList.GetSortedList(aModel, filterTelphoneUsers, columnlist);



            List<TelphoneUser> displayedPhones =
                filterTelphoneUsers.Skip(aModel.iDisplayStart).Take(aModel.iDisplayLength).ToList();


            var result = from c in displayedPhones
                select new[]
                {

                    c.BANumber,
                    c.Name,
                    c.Rank,
                    c.NewPhoneNumber,
                    c.Address,
                    c.Gender,
                    c.PhoneStatus,
                    c.ConnectedDate,
                    "Edit",

                    Convert.ToString(c.ID)
                };
            List<string[]> resultList = result.ToList();
            List<string> aRankList = new List<string>();
            List<string> banumberList = new List<string>();
            List<string> complainTypeList = new List<string>();
            List<string> statusList=new List<string>();


            foreach (TelphoneUser a1 in aUsersList)
            {
                if (!aRankList.Contains(a1.Rank))
                {
                    aRankList.Add(a1.Rank);
                }
                if (!banumberList.Contains(a1.BANumber))
                {
                    banumberList.Add(a1.BANumber);

                }
                if (!complainTypeList.Contains(a1.Gender))
                {
                    complainTypeList.Add(a1.Gender);
                }

                if (!statusList.Contains(a1.PhoneStatus))
                {
                    statusList.Add(a1.PhoneStatus);
                }
            }


            return Json(new
            {
                sEcho = aModel.sEcho,
                yadcf_data_0 = banumberList,
                yadcf_data_2 = aRankList,
                yadcf_data_5 = complainTypeList,
                yadcf_data_6 = statusList,

                iTotalRecords = aUsersList.Count(),
                iTotalDisplayRecords = filterTelphoneUsers.Count(),
                aaData = resultList
            },
             JsonRequestBehavior.AllowGet);
    }

        

        public ActionResult GetSinglePhoneData(string id)
        {
            List<TelphoneUser> phoneList = TelphoneUser.GetPhoneUserData(id);


            return Json(phoneList, JsonRequestBehavior.AllowGet);
        }

        

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