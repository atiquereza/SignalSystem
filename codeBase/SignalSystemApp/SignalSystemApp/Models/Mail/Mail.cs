using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalSystem.Libs;

namespace SignalSystemApp.Models.Mail
{
    public class MailData
    {
        public string ID { set; get; }
        public string MailID { set; get; }
        public string MailDescription { set; get; }
        public DateTime MailArrival { set; get; }
        public DateTime MailDeparture { set; get; }
        public string MailTo { set; get; }
        public string MailFrom { set; get; }

    }


    public class Mail
    {
        DBGateway aGateway = new DBGateway("SignalSystemConnectionString");
        public string AddNewMail(MailData aMailData)
        {
            string insertString = "INSERT INTO maildata (`MailID`, `MailDescription`,`DateArrival`,`DateDeparture`,`MailToID`,`MailFromID`) VALUES ('" + aMailData.MailID + "', '" + aMailData.MailDescription + "','" + aMailData.MailArrival.ToString("yyyy-MM-dd") + "','" + aMailData.MailDeparture.ToString("yyyy-MM-dd") + "',"+aMailData.MailTo+","+aMailData.MailFrom+"); ";
            Hashtable aHashtable = new Hashtable()
            {
                {"MailID",aMailData.MailID},
                {"mailDescription",aMailData.MailDescription},
                {"DateArrival",aMailData.MailArrival.ToString("yyyy-MM-dd")},
                 {"DateDeparture",aMailData.MailDeparture.ToString("yyyy-MM-dd")},
                 {"MailTo",aMailData.MailTo},
                 {"MailFrom",aMailData.MailFrom}
            };
            int lastInsertId = aGateway.Insert(insertString, aHashtable);

            return "Data Successfully Inserted.";
        }

        public List<MailData> GetMailData(string mailId, string mailDescription, 
            string mailFrom, string mailTo, string dateArrivalFrom, string dateArrivalTo,int start, int length,out int totalRecords)
        {

            string query = "select m.ID,m.MailID,m.MailDescription,s1.Value as MailFrom," +
                           "s2.Value as MailTo,m.DateArrival,m.DateDeparture from maildata m left join menustations s1 on " +
                           "s1.ID=m.MailFromID  left join menustations s2 on s2.ID=m.MailToID where ";

            if (mailId.Trim().Length != 0)
            {
                query += " m.MailID like '%" + mailId + "%' and ";
            }
            if (mailDescription.Trim().Length != 0)
            {
                query += " m.MailDescription like '%" + mailDescription + "%' and ";
            }
            if (mailFrom.Trim().Length != 0)
            {
                query += " s1.Value like '%" + mailFrom + "%' and ";
            }
            if (mailTo.Trim().Length != 0)
            {
                query += " s2.Value like '%" + mailTo + "%' and ";
            }
            if (dateArrivalFrom.Trim().Length != 0)
            {
                if (dateArrivalTo.Trim().Length != 0)
                {
                    dateArrivalFrom = Convert.ToDateTime(dateArrivalFrom).ToString("yyyy-MM-dd");
                    dateArrivalTo = Convert.ToDateTime(dateArrivalTo).ToString("yyyy-MM-dd");
                    query += " m.DateArrival between " + dateArrivalFrom + " and " + dateArrivalTo + " and ";
                }
                else
                {
                    dateArrivalFrom = Convert.ToDateTime(dateArrivalFrom).ToString("yyyy-MM-dd");
                    query += " m.DateArrival between " + dateArrivalFrom + " and " + dateArrivalFrom + " and ";
                }
               
            }

            if (mailId.Trim().Length == 0 && mailDescription.Trim().Length == 0
                && mailFrom.Trim().Length == 0 && mailTo.Trim().Length == 0
                && dateArrivalFrom.Trim().Length == 0)
            {
                query = query.Trim().Substring(0, query.Trim().LastIndexOf("where"));
            }
            else
            {
                query = query.Trim().Substring(0, query.Trim().LastIndexOf("and"));
            }

            string totalRecordsQuery = query;
            query += " order by m.DateArrival limit " + start + "," + length + ";";
            List<MailData> aList = new List<MailData>();
            DataSet aSet = aGateway.Select(query);
            
            
            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                MailData aData = new MailData();
                aData.ID = dataRow["ID"].ToString();
                aData.MailID = dataRow["MailID"].ToString();
                aData.MailDescription = dataRow["MailDescription"].ToString();
                aData.MailFrom = dataRow["MailFrom"].ToString();
                aData.MailTo = dataRow["MailTo"].ToString();
                aData.MailArrival = Convert.ToDateTime(dataRow["DateArrival"].ToString());
                aData.MailDeparture = Convert.ToDateTime(dataRow["DateDeparture"].ToString()); 
                aList.Add(aData);
            }

            aSet = aGateway.Select("select count(*) as TotalRecords from mailData;");
            totalRecords = Convert.ToInt32(aSet.Tables[0].Rows[0]["TotalRecords"].ToString());

            return aList; 
        }

       


        public bool CheckMailID(string MailID)
        {
            bool status = true;
            DataSet ds = MySql.Data.MySqlClient.MySqlHelper.ExecuteDataset(System.Configuration.ConfigurationManager.ConnectionStrings["SignalSystemConnectionString"].ConnectionString, "select count(*) from mailData where MailID='" + MailID + "';");
            int IdCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            if (IdCount > 0)
            {
                status = false;
            }
            return status;
        }
       


    }
}