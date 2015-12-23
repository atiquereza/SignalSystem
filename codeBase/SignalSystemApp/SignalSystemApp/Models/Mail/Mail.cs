using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
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


    public class Station
    {
        public string ID { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        

    }
    public class Mail
    {
        DBGateway aGateway = new DBGateway("SignalSystemConnectionString");
        public string AddNewMail(MailData aMailData)
        {
            string insertString = "INSERT INTO maildata (`MailID`, `MailDescription`,`DateArrival`,`DateDeparture`,`MailTo`,`MailFrom`) VALUES ('" + aMailData.MailID + "', '" + aMailData.MailDescription + "','" + aMailData.MailArrival.ToString("yyyy-MM-dd") + "','" + aMailData.MailDeparture.ToString("yyyy-MM-dd") + "',"+aMailData.MailTo+","+aMailData.MailFrom+"); ";
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

        public List<MailData> GetMailData()
        {
            List<MailData> aList = new List<MailData>();
            DataSet aSet = aGateway.Select("DROP TABLE IF EXISTS Results;CREATE TEMPORARY TABLE Results"+
"(select maildata.ID as ID,mailData.MailID as MailID,mailData.MailDescription as MailDescription,mailData.DateArrival as DateArrival,mailData.DateDeparture as DateDeparture," +
" stations.Name as MailTo,mailData.MailFrom as MailFrom "+
" from maildata left join stations on maildata.MailTo=stations.ID);"+
" select Results.ID,Results.MailID as MailID,Results.MailDescription as MailDescription," +
" Results.MailTo,DateArrival,DateDeparture,Stations.Name as MailFrom from Results left join stations on stations.ID=Results.MailFrom; DROP TEMPORARY TABLE Results;");
            
            
            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                MailData aData = new MailData();
                aData.ID = dataRow["ID"].ToString();
                aData.MailArrival = Convert.ToDateTime(dataRow["DateArrival"].ToString());
                aData.MailDeparture = Convert.ToDateTime(dataRow["DateDeparture"].ToString());
                aData.MailDescription = dataRow["MailDescription"].ToString();
                aData.MailID = dataRow["MailID"].ToString();
                aData.MailFrom = dataRow["MailFrom"].ToString();
                aData.MailTo = dataRow["MailTo"].ToString();
                aList.Add(aData);
            } 
            return aList; 
        }


        public bool CheckMailID(string MailID)
        {
            bool status = true;
            DataSet ds = MySql.Data.MySqlClient.MySqlHelper.ExecuteDataset(System.Configuration.ConfigurationManager.ConnectionStrings["SignalSystemConnectionString"].ConnectionString, "select count(*) from mailData where MailID='"+MailID+"';");
            int IdCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            if (IdCount > 0)
            {
                status=false;
            }
            return status;
        }
       


    }
}