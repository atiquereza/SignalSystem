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

        public List<string[]> GetActivePhoneNumberDetails(out int totalRecords, out int filteredRecord, string baNumber, string name, string plateName, string fromDate, string toDate, string phoneNumber, string rank, int start, int length)
        {
            string query = "select phoneuserpersonalinfo.BANumber,phoneuserpersonalinfo.FullName," +
                           "phoneuserpersonalinfo.PlateName,menusrank.Value as Rank,allphoneinfo.PhoneNumber," +
                           "allactivephoneinfo.RequestDate,allactivephoneinfo.ConnectDate,allactivephoneinfo.PhoneUsedFor," +
                           "allactivephoneinfo.OfficeAddress,allactivephoneinfo.HomeAddress from allactivephoneinfo " +
                           "left join allphoneinfo on allphoneinfo.ID=allactivephoneinfo.AllPhoneInfoID " +
                           "left join phoneuserpersonalinfo on allactivephoneinfo.PhoneUserPersonalInfoId = phoneuserpersonalinfo.id " +
                           "left join menusrank on menusrank.id=phoneuserpersonalinfo.RankId where ";


            if (baNumber.Trim().Length != 0)
            {
                query += " phoneuserpersonalinfo.BANumber like '%" + baNumber + "%' and ";
            }
            if (name.Trim().Length != 0)
            {
                query += " phoneuserpersonalinfo.FullName like '%" + name + "%' and ";
            }
            if (plateName.Trim().Length != 0)
            {
                query += " phoneuserpersonalinfo.PlateName like '%" + plateName + "%' and ";
            }
            if (phoneNumber.Trim().Length != 0)
            {
                query += " allphoneinfo.PhoneNumber like '%" + phoneNumber + "%' and ";
            }
            if (rank.Trim().Length != 0)
            {
                query += " menurequesttype.Value like '%" + rank + "%' and ";
            }
            if (fromDate.Trim().Length != 0)
            {
                if (toDate.Trim().Length != 0)
                {
                    query += " allactivephoneinfo.RequestDate between '" + Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + Convert.ToDateTime(toDate).ToString("yyyy-MM-dd") + " 23:59:59" + "' and";
                }
                else
                {
                    query += " allactivephoneinfo.RequestDate between '" + Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59" + "' and";

                }
            }

            if (baNumber.Trim().Length == 0 && name.Trim().Length == 0 && plateName.Trim().Length == 0 &&
                fromDate.Trim().Length == 0 && toDate.Trim().Length == 0 && phoneNumber.Trim().Length == 0 && rank.Trim().Length == 0)
            {
                query = query.Trim().Substring(0, query.Trim().LastIndexOf("where"));
            }
            else
            {
                query = query.Trim().Substring(0, query.Trim().LastIndexOf("and"));

            }
            query += "order by RequestDate DESC limit " + start + "," + start + length + ";";


            List<string[]> aList = new List<string[]>();

            DataSet aSet = aGateway.Select(query);

            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                string address = dataRow["OfficeAddress"].ToString();
                if (dataRow["PhoneUsedFor"].ToString().ToLower().Trim() == "home")
                {
                    address = dataRow["HomeAddress"].ToString();
                }
                aList.Add(new string[]
                {
                     dataRow["BANumber"].ToString(),
                     dataRow["FullName"].ToString(),
                     dataRow["PlateName"].ToString(),
                     dataRow["Rank"].ToString(),
                     dataRow["PhoneNumber"].ToString(),
                     Convert.ToDateTime(dataRow["RequestDate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                     Convert.ToDateTime(dataRow["ConnectDate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                     dataRow["PhoneUsedFor"].ToString(),
                     address,
                   

                });
            }
            query = "select count(*) as TotalRecord from allactivephoneinfo;";
            totalRecords = Convert.ToInt32(aGateway.Select(query).Tables[0].Rows[0]["TotalRecord"].ToString());

            filteredRecord = aList.Count;
            return aList;
        }
    }
}