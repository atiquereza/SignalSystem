using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Server;
using SignalSystem.Libs;

namespace SignalSystemApp.Models.TelephoneNumberAndUser
{
    public class TelephoneNumberInfo
    {
        public int ID { set; get; }
        public string PhoneNumber { set; get; }
        public string ConnectionStatus { set; get; }
        public string ConnectionTypeID { set; get; }
        public string Remarks { set; get; }


    }

    public class TelephoneNumber
    {
        private DBGateway aGateway = new DBGateway("SignalSystemConnectionString");

        public string AddNewTelephoneNumber(TelephoneNumberInfo aTelephoneNumberInfo)
        {
            if (IsNumberAlreadyAvailable(aTelephoneNumberInfo))
            {
                return "Error! Number Already Exist in Database.";
            }

            InsertIntoDatabase(aTelephoneNumberInfo);

            return "Number Success Fully Added.";
        }

        private void InsertIntoDatabase(TelephoneNumberInfo aTelephoneNumberInfo)
        {
            string nonQuery = "INSERT INTO allphoneinfo (`PhoneNumber`," +
                              " `ServiceStatus`, `ConnectionTypeID`, `Remarks`)" +
                              " VALUES (@PhoneNumber," +
                              " @ServiceStatus, @ConnectionTypeID, @Remarks);";

            Hashtable aHashtable = new Hashtable()
            {
                {"PhoneNumber", aTelephoneNumberInfo.PhoneNumber},
                {"ServiceStatus", "Terminated"},
                {"ConnectionTypeID", aTelephoneNumberInfo.ConnectionTypeID},
                {"Remarks","New Phone"},

            };
            aGateway.Insert(nonQuery, aHashtable);

        }

        private bool IsNumberAlreadyAvailable(TelephoneNumberInfo aTelephoneNumberInfo)
        {
            string query = "select * from allphoneinfo where PhoneNumber=@PhoneNumber;";
            Hashtable aHastable = new Hashtable()
            {
                {"PhoneNumber", aTelephoneNumberInfo.PhoneNumber}
            };

            if (aGateway.Select(query, aHastable).Tables[0].Rows.Count == 0)
            {
                return false;
            }

            return true;

        }

        public List<Dictionary<string,string>> GetAvailableTelephoneNumberForNewConnection(string connectionType)
        {
            List<Dictionary<string,string> > availablePhones = new List<Dictionary<string, string>>();
            string query = "select * from allphoneinfo left join menuconnectiontype on menuconnectiontype.ID=allPhoneInfo.ConnectionTypeID where menuconnectiontype.Value='" + connectionType + "' and allPhoneInfo.ServiceStatus='Terminated';";
            DataSet aSet = aGateway.Select(query);

            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                Dictionary<string,string> aDictionary = new Dictionary<string, string>()
                {
                   { "ID",dataRow["ID"].ToString()},
                   { "Value",dataRow["PhoneNumber"].ToString() + " Current Status: " + dataRow["ServiceStatus"].ToString()},

                };
                availablePhones.Add(aDictionary); 
            }

            return availablePhones;
        }

        public List<Dictionary<string, string>> GetAllotedNumbersForExistingUser(string connectionType, string addressType, string baNumber)
        {
            List<Dictionary<string,string>> aList = new List<Dictionary<string, string>>();

            string query = "select allActivePhoneInfo.PhoneUsedFor, allphoneinfo.ID,allPhoneInfo.PhoneNumber," +
                           "menuconnectiontype.Value from phoneuserpersonalInfo  left join " +
                           "allactivephoneinfo  on allactivephoneinfo.PhoneUserPersonalInfoId=phoneuserpersonalInfo.Id " +
                           "left join allphoneinfo on allphoneinfo.ID=allactivephoneinfo.AllPhoneInfoID left join" +
                           " menuconnectiontype on menuconnectiontype.ID=allphoneinfo.ConnectionTypeID where " +
                           "phoneuserpersonalInfo.BANumber='"+baNumber+"';";

            DataSet aSet = aGateway.Select(query);
            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                if (dataRow["PhoneUsedFor"].ToString().ToLower() == addressType.ToLower() && dataRow["Value"].ToString().ToLower()==connectionType.ToLower())
                {
                    Dictionary<string, string> aDictionary = new Dictionary<string, string>()
                    {
                        {"ID",dataRow["ID"].ToString()},
                        {"Value",dataRow["PhoneNumber"].ToString()},

                    };
                    aList.Add(aDictionary);
                }
                
            }

            return aList;
        }
    }
}