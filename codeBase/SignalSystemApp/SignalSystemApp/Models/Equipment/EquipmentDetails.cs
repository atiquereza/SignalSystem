using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SignalSystem.Libs;
using SignalSystemApp.Models.Telephone;

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


        public static List<EquipmentDetails> GetSingleEquipmentDetails(string id)
        {
            string query =
                "select equipemntdescription.id,equipemntdescription.typeid,equipemntdescription.amount,equipemntdescription.description,equipmnttype.typename from equipemntdescription,equipmnttype where equipemntdescription.typeid=equipmnttype.id and equipemntdescription.id=" +
                id;

            DBGateway aGateway=new DBGateway();
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


        public static void DeleteEquipment(string id)
        {
            string deleteQuery;
            deleteQuery = "DELETE FROM `signalappdb`.`equipemntdescription` WHERE  `Id`=" + id + ";";

            DBGateway aGateway = new DBGateway();
            string updateResult = aGateway.Delete(deleteQuery);
        }

        public static void UpdateEquipmentDetails(EquipmentDetails aEquipmentDetails)
        {
            string updateQuery;
            updateQuery = "UPDATE `signalappdb`.`equipemntdescription` SET `TypeId`=" + aEquipmentDetails.TypeID + ", `Amount`=" +
                          aEquipmentDetails.Amount + ", `Description`='" + aEquipmentDetails.Description + "' WHERE  `Id`=" +
                          aEquipmentDetails.EquipmentId + ";";

            DBGateway aGateway = new DBGateway();
            string updateResult = aGateway.Update(updateQuery);
        }

        public static void AddEquipmentDetails(string type, string amount, string description)
        {
            DBGateway aGateway = new DBGateway();
            string insertQuery =
                "INSERT INTO `signalappdb`.`equipemntdescription` (`TypeId`, `Amount`, `Description`) VALUES (" + type + ", " +
                amount + ", '" + description + "');";
            aGateway.Insert(insertQuery);
        }



    }


    public class EquipmentType
    {
        public int TypeID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<EquipmentDetails> Equipments { get; set; }

        public static List<EquipmentType> GEtEquipmentTypes()
        {
            DBGateway aGateway = new DBGateway();
            //List<EquipmentType> aList=new List<EquipmentType>();
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

            return aList;


        }


        public static List<EquipmentType> GetEquipmentTypes()
        {
            List<EquipmentType> equipmentTypes = new List<EquipmentType>();
            DBGateway aGateway = new DBGateway();
            string queryType = "select * from equipmnttype";
            DataSet ctDataSet = aGateway.Select(queryType);
            foreach (DataRow dataRow in ctDataSet.Tables[0].Rows)
            {
                TelephoneComplainType aType = new TelephoneComplainType();
                EquipmentType aEquipmentType = new EquipmentType();
                aEquipmentType.TypeID = (int)dataRow["Id"];
                aEquipmentType.Name = dataRow["TypeName"].ToString();

                equipmentTypes.Add(aEquipmentType);
            }
            return equipmentTypes;
        }

        public static void AddEquimpnetType(string typeName)
        {
            DBGateway aGateway = new DBGateway();
            string insertType = "INSERT INTO `signalappdb`.`equipmnttype` (`TypeName`) VALUES ('" + typeName + "');";
            aGateway.Insert(insertType);
        }
    }
   
}