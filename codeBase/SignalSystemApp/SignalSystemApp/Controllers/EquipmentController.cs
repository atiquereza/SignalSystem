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
         [Authenticate]
        public ActionResult Index()
        {
            List<EquipmentType> aList = EquipmentType.GEtEquipmentTypes();

           ViewBag.EquipmentTypes = aList;

            return View(aList);
        }


         [Authenticate]
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

          [Authenticate]
        public ActionResult GetSingleEquipmentData(string id)
        {

            List<EquipmentDetails> aList = EquipmentDetails.GetSingleEquipmentDetails(id);

            List<EquipmentType> equipmentTypes = EquipmentType.GEtEquipmentTypes();
            aList.ForEach(list => list.EquipmentTypes = equipmentTypes);
            return Json(aList, JsonRequestBehavior.AllowGet);


        }


         [Authenticate]
        public ActionResult DeleteSingleEquipment(EquipmentDetails aEquipmentDetails)
        {
       
            string id = Request["IdDel"];
            string deleteQuery = string.Empty;

            EquipmentDetails.DeleteEquipment(id);

            return RedirectToAction("Index", "Equipment");

        }

         [Authenticate]

        public ActionResult EditSingleEquipment(EquipmentDetails aEquipmentDetails)
        {

            string id = Request["EquipmentId"];

         
            string updateQuery = string.Empty;

            EquipmentDetails.UpdateEquipmentDetails(aEquipmentDetails);

            return RedirectToAction("Index", "Equipment");

        }

         [Authenticate]
        public ActionResult AddEquipment()
        {

            string type = Request["addTypeSelect"];
            string amount = Request["addAmount"];
            string description = Request["addDescription"];

            EquipmentDetails.AddEquipmentDetails(type, amount, description);

            return RedirectToAction("Index", "Equipment");
        }
          [Authenticate]
        
        public ActionResult AddEquipmentType()
        {
            string typeName = Request["AddTypeName"];

            List<EquipmentType> equipmentTypes = EquipmentType.GetEquipmentTypes();

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
                EquipmentType.AddEquimpnetType(typeName);
            }
            return RedirectToAction("Index", "Equipment");
        }

         [Authenticate]
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