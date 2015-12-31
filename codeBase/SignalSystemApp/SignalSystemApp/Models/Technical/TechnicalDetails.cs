using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SignalSystem.Libs;
using SignalSystemApp.Models.Equipment;
using SignalSystemApp.Models.Telephone;

namespace SignalSystemApp.Models.Technical
{
    public class TechnicalDetails
    {
        public string TechnicalId { get; set; }
        public int Amount { get; set; }

        public string Description { get; set; }
      

        public int TypeID { get; set; }
        public string TypeName { set; get; }
        public DateTime OnAirDate { set; get; }
        public List<TechnicalEquipmentType> EquipmentTypes { set; get; }


        public List<TechnicalDetails> GetEquipmentList(string equipmentType, string description)
        {
            DBGateway aGateway = new DBGateway();

            string query = "select technicaldescription.id,technicaldescription.typeid,technicaldescription.amount," +
                           "technicaldescription.description,technicaldescription.OnAirDate,equipmnttype.typename from technicaldescription," +
                           "equipmnttype where technicaldescription.typeid=equipmnttype.id";
            if (equipmentType.Trim().Length != 0)
            {
                query += " and equipmnttype.TypeName like '%" + equipmentType + "%' ";
            }
            if (description.Trim().Length != 0)
            {
                query += " and technicaldescription.description like '%" + description + "%' ";
            }

            query += ";";

            List<TechnicalDetails> aList = new List<TechnicalDetails>();
            DataSet aDataSet = aGateway.Select(query);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {

                TechnicalDetails aTechnicalDetails = new TechnicalDetails();
                aTechnicalDetails.TechnicalId = dataRow["Id"].ToString();
                aTechnicalDetails.TypeID = (int) dataRow["TypeID"];
                aTechnicalDetails.Amount = (int) dataRow["Amount"];
                aTechnicalDetails.Description = dataRow["Description"].ToString();
                aTechnicalDetails.OnAirDate = Convert.ToDateTime(dataRow["OnAirDate"].ToString());
                aTechnicalDetails.TypeName = dataRow["typename"].ToString();
                aList.Add(aTechnicalDetails);
            }

            return aList;
        }


        public static List<TechnicalDetails> GetSingleTechnicalDetails(string id)
        {
            string query =
                "select technicaldescription.id,technicaldescription.typeid,technicaldescription.amount,technicaldescription.description,technicaldescription.OnAirDate,equipmnttype.typename from technicaldescription,equipmnttype where technicaldescription.typeid=equipmnttype.id and technicaldescription.id=" +
                id;

            DBGateway aGateway = new DBGateway();
            List<TechnicalDetails> aList = new List<TechnicalDetails>();
            DataSet aDataSet = aGateway.Select(query);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {
                TechnicalDetails aTechnicalDetails = new TechnicalDetails();
                aTechnicalDetails.TechnicalId = dataRow["Id"].ToString();
                aTechnicalDetails.TypeID = (int) dataRow["TypeID"];
                aTechnicalDetails.Amount = (int) dataRow["Amount"];
                aTechnicalDetails.Description = dataRow["Description"].ToString();
                aTechnicalDetails.OnAirDate = Convert.ToDateTime(dataRow["OnAirDate"].ToString());
              //  aData.MailArrivalDate = Convert.ToDateTime(dataRow["DateArrival"].ToString());
                aTechnicalDetails.TypeName = dataRow["typename"].ToString();
                aList.Add(aTechnicalDetails);
            }
            return aList;
        }


        public static void DeleteEquipment(string id)
        {
            string deleteQuery;
            deleteQuery = "DELETE FROM `signalappdb`.`technicaldescription` WHERE  `Id`=" + id + ";";

            DBGateway aGateway = new DBGateway();
            string updateResult = aGateway.Delete(deleteQuery);
        }

        public static void UpdateTechnicalDetails(TechnicalDetails aTechnicalDetails, string OAD)
        {
            DateTime dt = Convert.ToDateTime(aTechnicalDetails.OnAirDate);
           // var date = DateTime.Parse(aTechnicalDetails.OnAirDate);
            string updateQuery;
            updateQuery = "UPDATE `signalappdb`.`technicaldescription` SET `TypeId`=" + aTechnicalDetails.TypeID +
                          ", `Amount`=" +
                          aTechnicalDetails.Amount + ",`OnAirDate`='" + OAD + "', `Description`='" + aTechnicalDetails.Description +
                          "' WHERE  `Id`=" +
                          aTechnicalDetails.TechnicalId + ";";

            DBGateway aGateway = new DBGateway();
            string updateResult = aGateway.Update(updateQuery);
        }

        public static void AddTechnicalDetails(string type, string amount, string description, string OnAirDate)
        {
            DBGateway aGateway = new DBGateway();
            string insertQuery =
                "INSERT INTO `signalappdb`.`technicaldescription` (`TypeId`, `Amount`, `Description`,`OnAirDate`) VALUES (" + type +
                ", " +
                amount + ", '" + description + "','"+OnAirDate+"');";
            aGateway.Insert(insertQuery);
        }
    }


    public class TechnicalEquipmentType
    {
        public int TypeID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TechnicalDetails> Equipments { get; set; }

        public static List<TechnicalEquipmentType> GEtEquipmentTypes()
        {
            DBGateway aGateway = new DBGateway();
            //List<EquipmentType> aList=new List<EquipmentType>();
            string query1 = "select * from equipmnttype";

            List<TechnicalEquipmentType> aList = new List<TechnicalEquipmentType>();
            DataSet aDataSet = aGateway.Select(query1);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {

                TechnicalEquipmentType aTypeList = new TechnicalEquipmentType();
                aTypeList.TypeID = (int) dataRow["Id"];
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
                aEquipmentType.TypeID = (int) dataRow["Id"];
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