using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SignalSystem.Libs;

namespace SignalSystemApp.Models.Equipment
{
    public class EquipmentDetails
    {

        public string EquipmentId { get; set; }
        public int Amount { get; set; }

        public string Description { get; set; }

        public int TypeID { get; set; }
        public string TypeName { set; get; }
        public List<EquipmentType> EquipmentTypes { set; get; }


        public List<EquipmentDetails> GetEquipmentList(string equipmentType,string description)
        {
            DBGateway aGateway=new DBGateway();

            string query = "select equipemntdescription.id,equipemntdescription.typeid,equipemntdescription.amount," +
                           "equipemntdescription.description,equipmnttype.typename from equipemntdescription," +
                           "equipmnttype where equipemntdescription.typeid=equipmnttype.id";
            if (equipmentType.Trim().Length != 0)
            {
                query += " and equipmnttype.TypeName like '%"+equipmentType+"%' ";
            }
            if (description.Trim().Length != 0)
            {
                query += " and equipemntdescription.description like '%" + description + "%' ";
            }

            query += ";";

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

            return aList;
        }
        
    }
   
}