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
            string query = "select * from phoneuserpersonalinfo where BANumber='" + aTelephoneUserInfo.BANumber + "';";
           
            if (aGateway.Select(query).Tables[0].Rows.Count == 0)
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

        public List<string[]> GetAllTelephoneUser(out int totalRecords, out int filteredRecord, int start, int length,string baNumber,string name,string palteName,string rank)
        {


            string query = "select * from  phoneuserpersonalinfo left join menusrank on "+
            "phoneuserpersonalinfo.RankId = menusrank.id where ";

            string countQuery = "select count(*) from  phoneuserpersonalinfo left join menusrank on " +
           "phoneuserpersonalinfo.RankId = menusrank.id where ";
            if (baNumber.Trim().Length != 0)
            {
                query += " phoneuserpersonalinfo.BANumber like '%" + baNumber + "%' and ";
                countQuery += " phoneuserpersonalinfo.BANumber like '%" + baNumber + "%' and ";
            }
            if (name.Trim().Length != 0)
            {
                query += " phoneuserpersonalinfo.FullName like '%" + name + "%' and ";
                countQuery += " phoneuserpersonalinfo.FullName like '%" + name + "%' and ";
            }
            if (palteName.Trim().Length != 0)
            {
                query += " phoneuserpersonalinfo.PlateName like '%" + palteName + "%' and ";
                countQuery += " phoneuserpersonalinfo.PlateName like '%" + palteName + "%' and ";
            }
            if (rank.Trim().Length != 0)
            {
                query += " menusrank.Value like '%" + rank + "%' and ";
                countQuery += " menusrank.Value like '%" + rank + "%' and ";
            }


            if (baNumber.Trim().Length == 0 && name.Trim().Length == 0 && palteName.Trim().Length == 0 && rank.Trim().Length == 0)
            {
                query = query.Trim().Substring(0, query.Trim().LastIndexOf("where"));
                countQuery = countQuery.Trim().Substring(0, countQuery.Trim().LastIndexOf("where"));

            }
            else
            {
                query = query.Trim().Substring(0, query.Trim().LastIndexOf("and"));
                countQuery = countQuery.Trim().Substring(0, countQuery.Trim().LastIndexOf("and"));


            }
            query += " limit " + start + "," + start + length + ";" + countQuery+ ";";

            DataSet aSet = aGateway.Select(query);

            List<string[]> allUsers = new List<string[]>();

            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                allUsers.Add(new string[]
                {
                     dataRow["BANumber"].ToString(),
                     dataRow["FullName"].ToString(),
                     dataRow["PlateName"].ToString(),
                     dataRow["Value"].ToString(),
                     dataRow["PresentAddress"].ToString(),
                     dataRow["OfficeAddress"].ToString()
                }
                );
            }

            totalRecords = Convert.ToInt32(aSet.Tables[1].Rows[0][0]);
            filteredRecord = allUsers.Count;
            return allUsers;
        }
    }
}