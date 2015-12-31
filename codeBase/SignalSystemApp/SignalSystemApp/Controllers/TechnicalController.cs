using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalSystemApp.Models;
using SortDirection = System.Web.Helpers.SortDirection;
using SignalSystem.Libs;
using SignalSystemApp.Models.Equipment;
using SignalSystemApp.Models.Technical;
using EquipmentType = SignalSystemApp.Models.Equipment.EquipmentType;

namespace SignalSystemApp.Controllers
{
    public class TechnicalController : Controller
    {
        public ActionResult Index()
        {
            List<EquipmentType> aList = EquipmentType.GEtEquipmentTypes();

            ViewBag.EquipmentTypes = aList;

            return View(aList);
        }



        public JsonResult TechnicalDataProviderAction(JQueryDataTableParamModel aModel)
        {
            string equipmentType = Convert.ToString(Request["sSearch_0"]);
            string description = Convert.ToString(Request["sSearch_1"]);
            List<string> columnlist = new List<string>(new string[] { "TypeName", "Amount", "Description" });

            TechnicalDetails aTechnicalDetails = new TechnicalDetails();
            List<TechnicalDetails> aEquipmentList = aTechnicalDetails.GetEquipmentList(equipmentType, description);


            List<TechnicalDetails> searchedList = new List<TechnicalDetails>();


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



            List<TechnicalDetails> displayedEquipments =
                searchedList.Skip(aModel.iDisplayStart).Take(aModel.iDisplayLength).ToList();


            var result = from c in displayedEquipments
                         select new[]
                {

                    c.TypeName,
                    c.Amount.ToString(),
                    c.Description,
                    c.OnAirDate.ToString("yyyy-MM-dd"),
                    c.TechnicalId,
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


        public ActionResult GetSingleTechnicalData(string id)
        {

            List<TechnicalDetails> aList = TechnicalDetails.GetSingleTechnicalDetails(id);

            List<TechnicalEquipmentType> equipmentTypes = TechnicalEquipmentType.GEtEquipmentTypes();
            aList.ForEach(list => list.EquipmentTypes = equipmentTypes);
            return Json(aList);


        }



        public ActionResult DeleteSingleTechnical(TechnicalDetails aTechnicalDetails)
        {

            string id = Request["IdDel"];
            string deleteQuery = string.Empty;

            TechnicalDetails.DeleteEquipment(id);

            return RedirectToAction("Index", "Technical");

        }



        public ActionResult EditSingleTechnical(TechnicalDetails aTechnicalDetails)
        {

            string id = Request["TechnicalId"];
            string TypeId = Request["TypeID"];
            string OAD = Request["OnAirDate"];




            string updateQuery = string.Empty;

            TechnicalDetails.UpdateTechnicalDetails(aTechnicalDetails,OAD);

            return RedirectToAction("Index", "Technical");

        }


        public ActionResult AddTechnical()
        {

            string type = Request["addTypeSelect"];
            string amount = Request["addAmount"];
            string description = Request["addDescription"];
            string OAD = Request["OnAirDate"];

            TechnicalDetails.AddTechnicalDetails(type, amount, description, OAD);

            return RedirectToAction("Index", "Technical");
        }


        public ActionResult AddTechnicalType()
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
            return RedirectToAction("Index", "Technical");
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