using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SignalSystem.Libs;

namespace SignalSystemApp.Models.TelephoneUser
{
    public class TelephoneUserInfo
    {
        public int Id { set; get; }
        public string BANumber { set; get; }
        public string FullName { set; get; }
        public string Appointment { set; get; }
        public string PlateName { set; get; }
        public string ServiceStatus { set; get; }
        public string RankID { set; get; }
        public string Sex { set; get; } // 
        public string Unit { set; get; }  
        public DateTime JoiningDate { set; get; } 
        public DateTime LPRDate { set; get; }//
        public string MaritalStatus { set; get; }
        public string PresentAddress { set; get; }
        public string PermanentAddress { set; get; }
        public string OfficeAddress { set; get; }  
        public string EmailAddress { set; get; }
        public string PersonalPhoneNumber { set; get; }
   

    }
    public class TelephoneUser
    {
        DBGateway aGateway = new DBGateway("SignalSystemConnectionString");
        public string AddTelephoneUser(TelephoneUserInfo aTelephoneUserInfo)
        {
            if (!IsBANumberUnique(aTelephoneUserInfo))
            {
                return "Error! BANumber Already Exist In Database";
            }


            InsertDataInfoDatabase(aTelephoneUserInfo);


            return "Telephone User Successfully Added";
        }

        private void InsertDataInfoDatabase(TelephoneUserInfo aTelephoneUserInfo)
        {
            string nonQuery = "INSERT INTO `phoneuserpersonalinfo` " +
                              "(`BANumber`, `FullName`, `Appointment`, `PlateName`, `ServiceStatus`, " +
                              "`RankId`, `Sex`, `Unit`, `JoiningDate`, `LPRDate`, `MaritalStatus`, " +
                              "`PresentAddress`, `PermanentAddress`, `OfficeAddress`, `PersonalPhoneNumber`, " +
                              "`EmailAddress`) VALUES " +
                              "(@BANumber, @FullName, @Appointment, @PlateName, @ServiceStatus, " +
                              "@RankId, @Sex, @Unit, @JoiningDate, @LPRDate, @MaritalStatus, " +
                              "@PresentAddress, @PermanentAddress, @OfficeAddress, @PersonalPhoneNumber, " +
                              "@EmailAddress);";
            Hashtable aHashtable = new Hashtable()
            {
                {"BANumber", aTelephoneUserInfo.BANumber},
                {"FullName", aTelephoneUserInfo.FullName},
                {"Appointment", aTelephoneUserInfo.Appointment},
                {"PlateName", aTelephoneUserInfo.PlateName},
                {"ServiceStatus", aTelephoneUserInfo.ServiceStatus},
                {"RankId", aTelephoneUserInfo.RankID},
                {"Sex", aTelephoneUserInfo.Sex},
                {"Unit", aTelephoneUserInfo.Unit},
                {"JoiningDate", aTelephoneUserInfo.JoiningDate.ToString("yyyy-MM-dd")},
                {"LPRDate", aTelephoneUserInfo.LPRDate.ToString("yyyy-MM-dd")},
                {"MaritalStatus", aTelephoneUserInfo.MaritalStatus},
                {"PresentAddress", aTelephoneUserInfo.PresentAddress},
                {"PermanentAddress", aTelephoneUserInfo.PermanentAddress},
                {"OfficeAddress", aTelephoneUserInfo.OfficeAddress},
                {"PersonalPhoneNumber", aTelephoneUserInfo.PersonalPhoneNumber},
                {"EmailAddress", aTelephoneUserInfo.EmailAddress},
            };

            aGateway.Insert(nonQuery, aHashtable);
        }

        private bool IsBANumberUnique(TelephoneUserInfo aTelephoneUserInfo)
        {
            string query = "select * from phoneuserpersonalinfo where BANumber=@BANumber;";
            Hashtable aHashtable = new Hashtable()
            {
                {"BANumber",aTelephoneUserInfo.BANumber}
            };
            if (aGateway.Select(query, aHashtable).Tables[0].Rows.Count == 0)
            {
                return true;
            }
            return false;
        }

        public TelephoneUserInfo GetTelephoneUserInfo(string baNumber)
        {
            TelephoneUserInfo aInfo = new TelephoneUserInfo();
            string query =
                "select * from phoneuserpersonalinfo " +
                "left join menusrank on menusrank.id = phoneuserpersonalinfo.RankId " +
                "where phoneuserpersonalinfo.BANumber='"+baNumber+"';";
          
            DataSet aSet = aGateway.Select(query);
            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                aInfo.Id = Convert.ToInt32(dataRow["ID"].ToString().Trim());
                aInfo.BANumber = dataRow["BANumber"].ToString().Trim();
                aInfo.FullName = dataRow["FullName"].ToString().Trim();
                aInfo.RankID = dataRow["Value"].ToString().Trim();
                aInfo.OfficeAddress = dataRow["OfficeAddress"].ToString().Trim();
                aInfo.PresentAddress = dataRow["PresentAddress"].ToString().Trim();
                
            }
            return aInfo;
        }
    }
}