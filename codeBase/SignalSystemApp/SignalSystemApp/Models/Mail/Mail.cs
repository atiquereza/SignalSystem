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
        public DateTime MailDate { set; get; }
    }
    public class Mail
    {
        DBGateway aGateway = new DBGateway("SignalSystemConnectionString");
        public string AddNewMail(string mailId, string mailDescription)
        {
            string insertString = "INSERT INTO maildata (`MailID`, `MailDescription`,`Date`) VALUES (@MailID, @mailDescription,@date); ";
            Hashtable aHashtable = new Hashtable()
            {
                {"MailID",mailId},
                {"mailDescription",mailDescription},
                {"date",DateTime.Now.ToString("yyyy-MM-dd")}
            };
            int lastInsertId = aGateway.Insert(insertString, aHashtable);

            return "Data Successfully Inserted.";
        }

        public List<MailData> GetMailData()
        {
            List<MailData> aList = new List<MailData>();
            DataSet aSet = aGateway.Select("select * from maildata;");

            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                MailData aData = new MailData();
                aData.ID = dataRow["ID"].ToString();
                aData.MailDate = Convert.ToDateTime(dataRow["Date"].ToString());
                aData.MailDescription = dataRow["MailDescription"].ToString();
                aData.MailID = dataRow["MailID"].ToString();
                aList.Add(aData);
            } 
            return aList; 
        }
    }
}