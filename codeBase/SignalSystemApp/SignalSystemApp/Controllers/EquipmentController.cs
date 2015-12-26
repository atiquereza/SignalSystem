using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using SignalSystem.Libs;
using SignalSystemApp.Models;
using SignalSystemApp.Models.Equipment;
using SignalSystemApp.Models.Telephone;
using SortDirection = System.Web.Helpers.SortDirection;
namespace SignalSystemApp.Controllers
{
    public class EquipmentController : Controller
    {
        DBGateway aGateway=new DBGateway();
        // GET: Equipment
        public ActionResult Index()
        {
           List<EquipmentType> aEquipmentTypeList=new List<EquipmentType>();

           string query1 = "select * from equipmnttype";

           List<EquipmentType> aList = new List<EquipmentType>();
           DataSet aDataSet = aGateway.Select(query1);
           foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
           {

               EquipmentType aTypeList = new EquipmentType();
               aTypeList.TypeID = (int)dataRow["Id"];
               aTypeList.Name = dataRow["TypeName"].ToString();

               aList.Add(aTypeList);
           }

           ViewBag.EquipmentTypes = aList;

           

            return View(aList);
        }

        
    //[HttpPost]
    //    public ActionResult Index(string search)
    //    {
    //        string query = "select * from equipemntdescription";
    //        string query1 = "select * from equipmnttype";

    //        List<EquipmentDetails> aList = new List<EquipmentDetails>();
    //        DataSet aDataSet = aGateway.Select(query);
    //        foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
    //        {

    //            EquipmentDetails aEquipmentDetails = new EquipmentDetails();
    //            aEquipmentDetails.EquipmentId = dataRow["Id"].ToString();
    //            aEquipmentDetails.TypeID = (int)dataRow["TypeID"];
    //            aEquipmentDetails.Amount = (int)dataRow["Amount"];
    //            aEquipmentDetails.Description = dataRow["Description"].ToString();
    //            aList.Add(aEquipmentDetails);
    //        }

    //        return View(aList);
    //    }


       

        public JsonResult EquipmentDataProviderAction(JQueryDataTableParamModel aModel)
        {
            string equipmentType = Convert.ToString(Request["sSearch_0"]);
            string description = Convert.ToString(Request["sSearch_1"]);
            List<string> columnlist = new List<string>(new string[] { "TypeName", "Amount", "Description"});
            
            EquipmentDetails aEquipmentDetails=new EquipmentDetails();
            List<EquipmentDetails> aEquipmentList = aEquipmentDetails.GetEquipmentList(equipmentType, description);

          
            List<EquipmentDetails> searchedList=new List<EquipmentDetails>();


            if (string.IsNullOrEmpty(aModel.sSearch))
            {
                searchedList = aEquipmentList;
            }
            else
            {
                searchedList =
                    aEquipmentList.Where(c => c.TypeName.ToLower().Contains(aModel.sSearch.ToLower())
                                           || c.Description.ToLower().Contains(aModel.sSearch.ToLower())
                                           ).ToList();
            }

           
          


          
            if (aModel.sSortDir_0 == "asc")
            {
                SortList(searchedList, columnlist[aModel.iSortCol_0], SortDirection.Ascending);
            }
            else { SortList(searchedList, columnlist[aModel.iSortCol_0], SortDirection.Descending); }



            List<EquipmentDetails> displayedEquipments =
                searchedList.Skip(aModel.iDisplayStart).Take(aModel.iDisplayLength).ToList();


            var result = from c in displayedEquipments
                         select new []
                {

                    c.TypeName,
                    c.Amount.ToString(),
                    c.Description,
                    c.EquipmentId,
                    "View"
                   
                  
                };
            List<string[]> resultList = result.ToList();
        
            return Json(new
            {
                sEcho = aModel.sEcho,
                iTotalRecords = aEquipmentList.Count(),
                iTotalDisplayRecords = searchedList.Count(),
                aaData = resultList
            },
               JsonRequestBehavior.AllowGet);

       

        }





        //public ActionResult GetExcelFile(string overAllSearch)
        //{
           
        //    if ((overAllSearch == null))
        //    {
        //        overAllSearch = "";
        //    }

           
        //    DateTime fromDate = DateTime.MinValue;
        //    DateTime toDate = DateTime.MaxValue;
            
        //    EquipmentDetails aEquipmentDetails=new EquipmentDetails();
        //    List<EquipmentDetails> equipmentList=aEquipmentDetails.GetEquipmentList();

        //     List<EquipmentDetails> filteredEquipmentList=new List<EquipmentDetails>();


        //     if (string.IsNullOrEmpty(overAllSearch))
        //     {
        //         filteredEquipmentList = equipmentList;
        //     }
        //     else
        //     {
        //         filteredEquipmentList =
        //             equipmentList.Where(c => c.TypeName.ToLower().Contains(overAllSearch.ToLower())
        //                                    || c.Description.ToLower().Contains(overAllSearch.ToLower())
        //                                    ).ToList();
        //     }





        //     if (filteredEquipmentList.Count > 0)
        //    {

        //        DateTime dt = DateTime.Now;
        //        DateTime dDate = DateTime.Now;
        //        string[] sDate = dDate.ToString().Split(' ');
        //        string time = dt.ToString("hh:mm");


        //        GridView aGridView = new GridView();
        //        aGridView.DataSource = filteredEquipmentList;
        //        aGridView.DataBind();


        //        foreach (TableCell cell in aGridView.HeaderRow.Cells)
        //        {
        //            cell.BackColor = Color.Cornsilk;
        //        }

        //        foreach (GridViewRow row in aGridView.Rows)
        //        {
        //            // row.BackColor = Color.White;
        //            foreach (TableCell cell in row.Cells)
        //            {
        //                if (row.RowIndex % 2 == 0)
        //                {
        //                    cell.BackColor = Color.Gainsboro;
        //                }
        //                else
        //                {
        //                    cell.BackColor = Color.GhostWhite;
        //                }
        //                cell.CssClass = "textmode";
        //            }
        //        }

        //        Response.ClearContent();
        //        Response.AddHeader("content-disposition",
        //            "attachment;filename=EquipmentList_" + sDate[0] + "_" + sDate[1] + sDate[2] + ".xls");
        //        Response.ContentType = "application/excel";
        //        StringWriter swr = new StringWriter();
        //        HtmlTextWriter tw = new HtmlTextWriter(swr);
        //        aGridView.RenderControl(tw);
        //        Response.Write(swr.ToString());

        //        Response.End();
        //    }
        //    return null;
        //}



        public ActionResult GetSingleEquipmentData(string id)
        {

            string query = "select equipemntdescription.id,equipemntdescription.typeid,equipemntdescription.amount,equipemntdescription.description,equipmnttype.typename from equipemntdescription,equipmnttype where equipemntdescription.typeid=equipmnttype.id and equipemntdescription.id="+id;
          

            List<EquipmentDetails> aList = new List<EquipmentDetails>();
            DataSet aDataSet = aGateway.Select(query);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {

                EquipmentDetails aEquipmentDetails = new EquipmentDetails();
                aEquipmentDetails.EquipmentId = dataRow["Id"].ToString();
                aEquipmentDetails.TypeID = (int)dataRow["TypeID"];
                aEquipmentDetails.Amount = (int)dataRow["Amount"];
                aEquipmentDetails.Description = dataRow["Description"].ToString();
                aEquipmentDetails.TypeName = dataRow["typename"].ToString();
                aList.Add(aEquipmentDetails);
            }

            List<EquipmentType> equipmentTypes = GetEquipmentTypes();
            aList.ForEach(list => list.EquipmentTypes = equipmentTypes);
            return Json(aList, JsonRequestBehavior.AllowGet);


        }

        private List<EquipmentType> GetEquipmentTypes()
        {
            List<EquipmentType> equipmentTypes = new List<EquipmentType>();

            string queryType = "select * from equipmnttype";
            DataSet ctDataSet = aGateway.Select(queryType);
            foreach (DataRow dataRow in ctDataSet.Tables[0].Rows)
            {
                TelephoneComplainType aType = new TelephoneComplainType();
                EquipmentType aEquipmentType = new EquipmentType();
                aEquipmentType.TypeID = (int) dataRow["Id"];
                aEquipmentType.Name = dataRow["TypeName"].ToString();

                equipmentTypes.Add(aEquipmentType);
            }
            return equipmentTypes;
        }


        public ActionResult DeleteSingleEquipment(EquipmentDetails aEquipmentDetails)
        {
       
            string id = Request["IdDel"];
            string deleteQuery = string.Empty;

            deleteQuery = "DELETE FROM `signalappdb`.`equipemntdescription` WHERE  `Id`=" + id + ";";
            
            DBGateway aGateway = new DBGateway();
            string updateResult = aGateway.Delete(deleteQuery);
            
            return RedirectToAction("Index", "Equipment");

        }


        public ActionResult EditSingleEquipment(EquipmentDetails aEquipmentDetails)
        {

            string id = Request["EquipmentId"];

         
            string updateQuery = string.Empty;

            updateQuery = "UPDATE `signalappdb`.`equipemntdescription` SET `TypeId`=" + aEquipmentDetails.TypeID + ", `Amount`=" + aEquipmentDetails.Amount + ", `Description`='" + aEquipmentDetails.Description + "' WHERE  `Id`=" + aEquipmentDetails.EquipmentId + ";";

            DBGateway aGateway = new DBGateway();
            string updateResult = aGateway.Update(updateQuery);

            return RedirectToAction("Index", "Equipment");

        }

        public ActionResult AddEquipment()
        {

            string type = Request["addTypeSelect"];
            string amount = Request["addAmount"];
            string description = Request["addDescription"];

            string insertQuery = "INSERT INTO `signalappdb`.`equipemntdescription` (`TypeId`, `Amount`, `Description`) VALUES ("+type+", "+amount+", '"+description+"');";
            aGateway.Insert(insertQuery);

            return RedirectToAction("Index", "Equipment");
        }


        public ActionResult AddEquipmentType()
        {
            string typeName = Request["AddTypeName"];

            List<EquipmentType> equipmentTypes = GetEquipmentTypes();

            bool existingType = false;
            foreach (EquipmentType aType in equipmentTypes)
            {
                if (aType.Name.ToLower() == typeName.ToLower())
                {
                    existingType = true;
                }
            }

            if (existingType)
            {
                ModelState.AddModelError("TypeName", "TypeName Already Exist.");
            }
            else
            {
                string insertType = "INSERT INTO `signalappdb`.`equipmnttype` (`TypeName`) VALUES ('"+typeName+"');";
                aGateway.Insert(insertType);
            }
            return RedirectToAction("Index", "Equipment");
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


    }
}