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

        public ActionResult GetSingleComplainData(string id)
        {
            List<Dictionary<string, string>> pendingComplains = new List<Dictionary<string, string>>();

            string query =
                "select complains.id,telephoneusers.BANumber,telephoneusers.Name,menucomplainType.Value as " +
                "ComplainType,telephoneusers.NewPhoneNumber,menusrank.Value as Rank, " +
                "complains.Description,complains.ComplainDate,complains.ResolvedBy,complains.ResolvedDate,complains.Remarks,complains.ActionTaken from complains,menucomplainType,telephoneusers," +
                "menusRank where menucomplaintype.Id=complains.MenuComplainTypeId " +
                "and telephoneusers.Id = complains.TelephoneUserId and telephoneusers.RankId = menusrank.id and complains.id=" + id + ";";
            //TelephoneComplain aTelephoneComplain = new TelephoneComplain();
            //complains.`Status`='Pending' and 
            List<TelephoneComplain> aTelephoneComplainList=new List<TelephoneComplain>();
            DBGateway aGateway=new DBGateway();
            DataSet aDataSet = aGateway.Select(query);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {
               
                TelephoneComplain aTelephoneComplain=new TelephoneComplain();
                aTelephoneComplain.BANumber = dataRow["BANumber"].ToString();
                aTelephoneComplain.Name = dataRow["Name"].ToString();
                aTelephoneComplain.ComplainType = dataRow["ComplainType"].ToString();
                aTelephoneComplain.NewPhoneNumber = dataRow["NewPhoneNumber"].ToString();
                aTelephoneComplain.Rank = dataRow["Rank"].ToString();
                aTelephoneComplain.Description = dataRow["Description"].ToString();
                aTelephoneComplain.ComplainDate = dataRow["ComplainDate"].ToString();
                aTelephoneComplain.ComplainId = dataRow["Id"].ToString();
                aTelephoneComplain.ActionTaken = dataRow["ActionTaken"].ToString();
                aTelephoneComplain.ResolvedDate = dataRow["ResolvedDate"].ToString();
                aTelephoneComplain.Remarks = dataRow["Remarks"].ToString();
                aTelephoneComplain.ResolveBy = dataRow["ResolvedBy"].ToString();
                aTelephoneComplainList.Add(aTelephoneComplain);
            }

            List<TelephoneComplainType> complainTypes=new List<TelephoneComplainType>();

            string queryType = "select * from menucomplaintype";
            DataSet ctDataSet=aGateway.Select(queryType);
            foreach (DataRow dataRow in ctDataSet.Tables[0].Rows)
            {
                TelephoneComplainType aType=new TelephoneComplainType();
                aType.TypeId = dataRow["Id"].ToString();
                aType.TypeValue = dataRow["Value"].ToString();
               complainTypes.Add(aType);
            }
            aTelephoneComplainList.ForEach(list => list.ProblemTypes = complainTypes);

           

            return Json(aTelephoneComplainList, JsonRequestBehavior.AllowGet);
          

        }


        public ActionResult DeleteSingleComplainData()
        {
            List<Dictionary<string, string>> pendingComplains = new List<Dictionary<string, string>>();

            //string id=
            string id = Request["deleteComplainId"].ToString();
            string query =
                "delete from  complains where id=" + id + ";";
            //TelephoneComplain aTelephoneComplain = new TelephoneComplain();
            List<TelephoneComplain> aTelephoneComplainList = new List<TelephoneComplain>();
            DBGateway aGateway = new DBGateway();
            string deleteResult = aGateway.Delete(query);



            return RedirectToAction("PendingComplains", "Telephone");
          //  return Json(aTelephoneComplainList, JsonRequestBehavior.AllowGet);


        }



        public ActionResult EditSingleComplainData(TelephoneComplain aTelephoneComplain)
        {
            string complainStatus = string.Empty;
            string updateQuery = string.Empty;
            if (aTelephoneComplain.Status == "0")

            {
                complainStatus = "Pending";
                updateQuery = "UPDATE `signalappdb`.`complains` SET `Description`='" + aTelephoneComplain.Description + "', `Status`='" + complainStatus + "', `MenuComplainTypeId`=" + aTelephoneComplain.ComplainType + " WHERE  `Id`=" + aTelephoneComplain.ComplainId + ";";
            }
            else if (aTelephoneComplain.Status == "1")
            {
              
                   
                 
                Telephone aTelephone=new Telephone();
                string convertedDateText = aTelephone.DMYToMDY(aTelephoneComplain.ResolvedDate);
                DateTime dt = DateTime.Parse(convertedDateText);

                complainStatus = "Resolved";
                updateQuery = "UPDATE `signalappdb`.`complains` SET `Description`='" + aTelephoneComplain.Description + "', `Status`='" + complainStatus + "', `MenuComplainTypeId`=" + aTelephoneComplain.ComplainType + ", `ResolvedDate`='" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "', `ActionTaken`='" + aTelephoneComplain.ActionTaken + "', `ResolvedBy`='Habib' WHERE  `Id`=" + aTelephoneComplain.ComplainId + ";";
            }

            

           

            string query =
                "delete from  complains where id=" + aTelephoneComplain.ComplainId + ";";
            
            DBGateway aGateway = new DBGateway();
            string deleteResult = aGateway.Update(updateQuery);



            return RedirectToAction("PendingComplains", "Telephone");
            //  return Json(aTelephoneComplainList, JsonRequestBehavior.AllowGet);


        }

        public JsonResult DataProviderAction(JQueryDataTableParamModel aModel)
        {
            List<string> columnlist = new List<string>(new string[] { "BANumber", "Name", "Rank", "NewPhoneNumber", "ComplainDate", "ComplainType" });
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
            // var sortingColumnNumber = Convert.ToInt32(aModel.iSortCol_0);
            //var sortingColumnName = aModel[string.Format("mDataProp_{0}", sortingColumnNumber)]; 

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
            



            List<TelephoneComplain> filteredComplaneList = GetFilteredComplaneList(sSearch, complanList, banumberFilter, phoneFilter, nameFilter, rankFilter, complainFilter, fromDate, toDate);


            if (aModel.sSortDir_0 == "asc")
            {
                SortList(filteredComplaneList, columnlist[aModel.iSortCol_0], SortDirection.Ascending);
            }
            else { SortList(filteredComplaneList, columnlist[aModel.iSortCol_0], SortDirection.Descending); }



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

        public List<TelephoneComplain> GetFilteredComplaneList(string sSearch, List<TelephoneComplain> complanList, string banumberFilter, string phoneFilter,
            string nameFilter, string rankFilter, string complainFilter, DateTime fromDate, DateTime toDate)
        {
            List<TelephoneComplain> searchedComplains;


            if ((sSearch == ""))
            {
                searchedComplains = complanList;
            }
            else
            {
                searchedComplains =
                    complanList.Where(c => c.BANumber.ToLower().Contains(sSearch.ToLower())
                                           || c.ComplainType.ToLower().Contains(sSearch.ToLower()) ||
                                           c.Name.ToLower().Contains(sSearch.ToLower()) ||
                                           c.NewPhoneNumber.ToLower().Contains(sSearch.ToLower()) ||
                                           c.ComplainType.ToLower().Contains(sSearch.ToLower())).ToList();
            }


            var filteredCompanies = searchedComplains
                .Where(c => (banumberFilter == "" || c.BANumber.ToLower() == banumberFilter.ToLower())
                            &&
                            (phoneFilter == "" || c.NewPhoneNumber.ToLower().Contains(phoneFilter))
                            &&
                            (nameFilter == "" || c.Name.ToLower().Contains(nameFilter.ToLower()))
                            &&
                            (rankFilter == "" || c.Rank.ToLower() == rankFilter.ToLower())
                            &&
                            (complainFilter == "" || c.ComplainType.ToLower() == complainFilter.ToLower())
                            &&
                            (fromDate == DateTime.MinValue || fromDate < Convert.ToDateTime(c.ComplainDate))
                            &&
                            (toDate == DateTime.MaxValue || Convert.ToDateTime(c.ComplainDate) < toDate)
                );
            List<TelephoneComplain> filteredComplaneList = filteredCompanies.ToList();
            return filteredComplaneList;
        }




        public List<TelephoneComplain> GetResolvedFilteredComplaneList(string sSearch, List<TelephoneComplain> complanList, string banumberFilter, string phoneFilter,
            string nameFilter, string rankFilter, string complainFilter, DateTime fromDate, DateTime toDate, string resolvedByFilter, DateTime resolverFromdate,DateTime resolverTodate,string actionFilter,string remarksFilter)
        {
            List<TelephoneComplain> searchedComplains;


            if ((sSearch == ""))
            {
                searchedComplains = complanList;
            }
            else
            {
                searchedComplains =
                    complanList.Where(c => c.BANumber.ToLower().Contains(sSearch.ToLower())
                                           || c.ComplainType.ToLower().Contains(sSearch.ToLower()) ||
                                           c.Name.ToLower().Contains(sSearch.ToLower()) ||
                                           c.NewPhoneNumber.ToLower().Contains(sSearch.ToLower()) ||
                                           c.ComplainType.ToLower().Contains(sSearch.ToLower()) ||
                                           c.ResolveBy.ToLower().Contains(sSearch.ToLower()) ||
                                           c.ActionTaken.ToLower().Contains(sSearch.ToLower()) ||
                                           c.Remarks.ToLower().Contains(sSearch.ToLower()) 
                                           
                                           ).ToList();
            }


            var filteredCompanies = searchedComplains
                .Where(c => (banumberFilter == "" || c.BANumber.ToLower() == banumberFilter.ToLower())
                            &&
                            (phoneFilter == "" || c.NewPhoneNumber.ToLower().Contains(phoneFilter))
                            &&
                            (nameFilter == "" || c.Name.ToLower().Contains(nameFilter.ToLower()))
                            &&
                            (rankFilter == "" || c.Rank.ToLower() == rankFilter.ToLower())
                            &&
                            (complainFilter == "" || c.ComplainType.ToLower() == complainFilter.ToLower())
                            &&
                            (fromDate == DateTime.MinValue || fromDate < Convert.ToDateTime(c.ComplainDate))
                            &&
                            (toDate == DateTime.MaxValue || Convert.ToDateTime(c.ComplainDate) < toDate)
                             &&
                            (resolverFromdate == DateTime.MinValue || resolverFromdate < Convert.ToDateTime(c.ResolvedDate))
                            &&
                            (resolverTodate == DateTime.MaxValue || Convert.ToDateTime(c.ResolvedDate) < resolverTodate)
                            &&
                            (resolvedByFilter == "" || c.ResolveBy.ToLower() == resolvedByFilter.ToLower())
                            &&
                            (actionFilter == "" || c.ActionTaken.ToLower().Contains(actionFilter.ToLower()))
                             &&
                            (remarksFilter == "" || c.Remarks.ToLower().Contains(remarksFilter.ToLower()))
                            
                );
            List<TelephoneComplain> filteredComplaneList = filteredCompanies.ToList();
            return filteredComplaneList;
        }






        public JsonResult GetomplainData(JQueryDataTableParamModel aModel)
        {
            DBGateway aGateway = new DBGateway();
            return null;
        }

        public void SortList<T>(List<T> list, string columnName, SortDirection direction)
        {
            var property = typeof(T).GetProperty(columnName);
            var multiplier = direction == SortDirection.Descending ? -1 : 1;
            list.Sort((t1, t2) =>
            {
                var col1 = property.GetValue(t1);
                var col2 = property.GetValue(t2);
                return multiplier * Comparer<object>.Default.Compare(col1, col2);
            });
        }

        public ActionResult GetExcelFile(string baNumber, string name, string rank, string phone, string complainType, string fromDateRange, string toDateRange, string overAllSearch, string hourFilter,string status)
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

            List<TelephoneComplain> filteredComplainsList = GetFilteredComplaneList(overAllSearch,complainList, baNumber, phone, name,
                rank, complainType, fromDate, toDate);


            //var filteredComplains = (searchedComplains
            //    .Where(c => (baNumber == "" || c.BANumber.ToLower() == baNumber.ToLower())
            //                &&
            //                (phone == "" || c.NewPhoneNumber.ToLower().Contains(phone))
            //                &&
            //                (name == "" || c.Name.ToLower().Contains(name.ToLower()))
            //                &&
            //                (rank == "" || c.Rank.ToLower() == rank.ToLower())
            //                &&
            //                (complainType == "" || c.ComplainType.ToLower() == complainType.ToLower())
            //               &&
            //                (fromDate == DateTime.MinValue || fromDate < Convert.ToDateTime(c.ComplainDate))
            //                &&
            //                (toDate == DateTime.MaxValue || Convert.ToDateTime(c.ComplainDate) < toDate)
            //                )
            //    );

            //filteredComplains=filteredComplains.ToList();

            DateTime dt = DateTime.Now;
            DateTime dDate = DateTime.Now;
            string[] sDate = dDate.ToString().Split(' ');
            string time = dt.ToString("hh:mm");

            
            GridView aGridView=new GridView();
            aGridView.DataSource = filteredComplainsList;
            aGridView.DataBind();

          //  aGridView.HeaderRow.BackColor = Color.Cornsilk;
            foreach (TableCell cell in aGridView.HeaderRow.Cells)
            {
                cell.BackColor = Color.Cornsilk;
            }

            foreach (GridViewRow row in aGridView.Rows)
            {
               // row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
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
            Response.AddHeader("content-disposition", "attachment;filename=PendingTelephoneComplain_"+sDate[0]+"_"+sDate[1]+sDate[2]+".xls");
            Response.ContentType = "application/excel";
            StringWriter swr = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(swr);
            aGridView.RenderControl(tw);
            Response.Write(swr.ToString());

            Response.End();
            return null;
        }



        public JsonResult ResolvedDataProviderAction(JQueryDataTableParamModel aModel)
        {
            List<string> columnlist = new List<string>(new string[] { "BANumber", "Name", "Rank", "NewPhoneNumber", "ComplainDate", "ComplainType", "ResolveBy", "ResolvedDate", "ActionTaken", "Remarks" });
           

           
            var banumberFilter = Convert.ToString(Request["sSearch_0"]);
            var nameFilter = Convert.ToString(Request["sSearch_1"]);


            var rankFilter = Convert.ToString(Request["sSearch_2"]);
            var dateFilter = Convert.ToString(Request["sSearch_4"]);
            var phoneFilter = Convert.ToString(Request["sSearch_3"]);
            var complainFilter = Convert.ToString(Request["sSearch_5"]);
            var hourlyFilter = Convert.ToString(Request["sSearch_6"]);
            var resolverFilter = Convert.ToString(Request["sSearch_6"]);
            var resolveDateFilter = Convert.ToString(Request["sSearch_7"]);
            var actionTakenFilter = Convert.ToString(Request["sSearch_8"]);
            var remarksFilter = Convert.ToString(Request["sSearch_9"]);
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
            // var sortingColumnNumber = Convert.ToInt32(aModel.iSortCol_0);
            //var sortingColumnName = aModel[string.Format("mDataProp_{0}", sortingColumnNumber)]; 

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




            List<TelephoneComplain> filteredComplaneList = GetResolvedFilteredComplaneList(sSearch, complanList, banumberFilter, phoneFilter, nameFilter, rankFilter, complainFilter, fromDate, toDate,resolverFilter,resolvefromDate,resolvetoDate,actionTakenFilter,remarksFilter);


            if (aModel.sSortDir_0 == "asc")
            {
                SortList(filteredComplaneList, columnlist[aModel.iSortCol_0], SortDirection.Ascending);
            }
            else { SortList(filteredComplaneList, columnlist[aModel.iSortCol_0], SortDirection.Descending); }



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



        public ActionResult GetResolveExcelFile(string baNumber, string name, string rank, string phone, string complainType, string fromDateRange, string toDateRange, string overAllSearch,string resolvedBy,string resolvefromDateRange,string resolvetoDateRange,string actionTaken,string remarks,string status)
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

                //List<TelephoneComplain> filteredComplainsList = GetFilteredComplaneList(overAllSearch, complainList, baNumber, phone, name,
                  //  rank, complainType, fromDate, toDate);


          
                List<TelephoneComplain> filteredComplainsList = GetResolvedFilteredComplaneList(overAllSearch, complainList, baNumber, phone, name,rank, complainType, fromDate, toDate, resolvedBy,resolveFromDate,resolveToDate,actionTaken,remarks);


            DateTime dt = DateTime.Now;
            DateTime dDate = DateTime.Now;
            string[] sDate = dDate.ToString().Split(' ');
            string time = dt.ToString("hh:mm");


            GridView aGridView = new GridView();
            aGridView.DataSource = filteredComplainsList;
            aGridView.DataBind();

            //  aGridView.HeaderRow.BackColor = Color.Cornsilk;
            foreach (TableCell cell in aGridView.HeaderRow.Cells)
            {
                cell.BackColor = Color.Cornsilk;
            }

            foreach (GridViewRow row in aGridView.Rows)
            {
                // row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
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
            Response.AddHeader("content-disposition", "attachment;filename=PendingTelephoneComplain_" + sDate[0] + "_" + sDate[1] + sDate[2] + ".xls");
            Response.ContentType = "application/excel";
            StringWriter swr = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(swr);
            aGridView.RenderControl(tw);
            Response.Write(swr.ToString());

            Response.End();
            return null;
        }





    }
}