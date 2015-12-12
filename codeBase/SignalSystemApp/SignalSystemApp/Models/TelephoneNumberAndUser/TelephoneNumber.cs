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

        public List<Dictionary<string,string>> GetAvailableTelephoneNumberForNewConnection()
        {
            List<Dictionary<string,string> > availablePhones = new List<Dictionary<string, string>>();
            string query = "select * from allphoneinfo where ServiceStatus='Terminated'";
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
    }
}