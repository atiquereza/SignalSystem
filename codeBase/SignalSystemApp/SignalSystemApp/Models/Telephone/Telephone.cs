using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using SignalSystem.Libs;
using SortDirection = System.Web.Helpers.SortDirection;


namespace SignalSystemApp.Models.Telephone
{

public class TelephoneComplain
{
    public string BANumber { get; set; }
    public string Name { get; set; }
    public string ComplainType { get; set; }
    public string NewPhoneNumber { get; set; }
    public string Rank { get; set; }
    public string Description { get; set; }
    public string ComplainDate { get; set; }
    public string ComplainId { get; set; }
    public List<TelephoneComplainType> ProblemTypes { get; set; }
    
    
    public string Status { set; get; }
    public string ResolvedDate { set; get; }
    public string ResolveBy { get; set; }
    //public string ResolveDescription { get; set; }
    public string ActionTaken { set; get; }
    public string Remarks { set; get; }
    


}

    public class FilteredLists
    {
        public List<TelephoneComplain> aComplainList { set; get; }
        public List<TelphoneUser> aTelePhoneUserList { set; get; } 
        public int FilteredSize { set; get; }
        public int TotalSize { set; get; }
        public List<string> BaNumberList { set; get; }
        public List<string> RankList { set; get; }
        public List<string> PhoneTypeList { set; get; } 
        public List<string> ComplainTypeList { set; get; }
        public List<string> ResolverList { set; get; }
    }

    public class TelephoneComplainType
    {
        public string TypeId { get; set; }
        public string TypeValue { get; set; }
    }

    public class TelphoneUser
    {
        public string ID { get; set; }
        public string BANumber { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string NewPhoneNumber { get; set; }
        public string HomeAddress { get; set; }
        public string OfficeAddress { get; set; }
        public string PhoneType { get; set; }
        public string Gender { get; set; }
        public string PhoneStatus { set; get; }
        public string ConnectedDate { get; set; }
        public string DisconnectedDate { get; set; }

        public List<TelphoneUser> GetTelphoneUsers()
        {
             DBGateway aGateway=new DBGateway();
             string query = "select telephoneusers.Id,telephoneusers.BANumber,telephoneusers.Name,menusrank.Value as Rank,telephoneusers.NewPhoneNumber,telephoneusers.Address,telephoneusers.Gender,telephoneusers.`Status`,telephoneusers.ConnectedDate,telephoneusers.DisconnectedDate from telephoneusers,menusrank where telephoneusers.RankId=menusrank.id";
             string query2 = "select complains.id,phoneuserpersonalinfo.BANumber,phoneuserpersonalinfo.FullName,menucomplainType.Value as ComplainType," +
 " allphoneinfo.PhoneNumber,menusrank.Value as Rank,complains.Description,complains.ComplainDate" +
 " from complains,menucomplainType,phoneuserpersonalinfo,allphoneinfo," +
 " menusRank where menucomplaintype.Id=complains.MenuComplainTypeId" +
 " and phoneuserpersonalinfo.Id = complains.TelephoneUserId and phoneuserpersonalinfo.RankId = menusrank.id and complains.AllPhoneInfoID=allphoneinfo.ID";

             string query3 = "select allactivephoneinfo.id,phoneuserpersonalinfo.BANumber,phoneuserpersonalinfo.FullName,menusrank.Value as Rank,allphoneinfo.PhoneNumber,allactivephoneinfo.PhoneUsedFor,allactivephoneinfo.HomeAddress,allactivephoneinfo.OfficeAddress,phoneuserpersonalinfo.Sex as Gender,allactivephoneinfo.ConnectDate from phoneuserpersonalinfo,menusrank,allactivephoneinfo,allphoneinfo where menusrank.id=phoneuserpersonalinfo.RankId and phoneuserpersonalinfo.ID=allactivephoneinfo.PhoneUserPersonalInfoId and allphoneinfo.ID=allactivephoneinfo.AllPhoneInfoID;";
            List<TelphoneUser> listTelphoneUsers=new List<TelphoneUser>();
             DataSet aDataSet = aGateway.Select(query3);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {

                TelphoneUser aTelphoneUser = new TelphoneUser();
                aTelphoneUser.BANumber = dataRow["BANumber"].ToString();
                aTelphoneUser.Name = dataRow["FullName"].ToString();

                aTelphoneUser.NewPhoneNumber = dataRow["PhoneNumber"].ToString();
                aTelphoneUser.Rank = dataRow["Rank"].ToString();

                aTelphoneUser.ConnectedDate = dataRow["ConnectDate"].ToString();
               // aTelphoneUser.DisconnectedDate = dataRow["DisconnectedDate"].ToString();
                aTelphoneUser.ID = dataRow["Id"].ToString();
               // aTelphoneUser.PhoneStatus = dataRow["Status"].ToString();

                aTelphoneUser.HomeAddress = dataRow["HomeAddress"].ToString();
                    aTelphoneUser.OfficeAddress = dataRow["OfficeAddress"].ToString();
                aTelphoneUser.Gender = dataRow["Gender"].ToString();
                aTelphoneUser.PhoneType = dataRow["PhoneUsedFor"].ToString();

                listTelphoneUsers.Add(aTelphoneUser);
            }

            return listTelphoneUsers;

        }


        public static List<TelphoneUser> GetPhoneUserData(string id)
        {
            string query =
                "select telephoneusers.Id,telephoneusers.BANumber,telephoneusers.Name,menusrank.Value as Rank,telephoneusers.NewPhoneNumber,telephoneusers.Address,telephoneusers.Gender,telephoneusers.`Status`,telephoneusers.ConnectedDate,telephoneusers.DisconnectedDate from telephoneusers,menusrank where telephoneusers.RankId=menusrank.id and telephoneusers.ID=" +
                id + "";

            List<TelphoneUser> phoneList = new List<TelphoneUser>();
            DBGateway aGateway = new DBGateway();
            DataSet aDataSet = aGateway.Select(query);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {
                TelphoneUser aTelphoneUser = new TelphoneUser();
                aTelphoneUser.BANumber = dataRow["BANumber"].ToString();
                aTelphoneUser.Name = dataRow["Name"].ToString();

                aTelphoneUser.NewPhoneNumber = dataRow["NewPhoneNumber"].ToString();
                aTelphoneUser.Rank = dataRow["Rank"].ToString();
                aTelphoneUser.HomeAddress = dataRow["Address"].ToString();
                aTelphoneUser.PhoneStatus = dataRow["Status"].ToString();
                aTelphoneUser.ID = dataRow["Id"].ToString();

                phoneList.Add(aTelphoneUser);
            }
            return phoneList;
        }

        public static void UpdateDeletePhoneUserData(string status, string id, string remarks)
        {
            DBGateway aGateway = new DBGateway();
            string query;
            if (status == "0") //delete phone
            {
                query = "delete from  telephoneusers where id=" + id + ";";
                aGateway.Delete(query);
            }

            else if (status == "1") //connect phone
            {
                query = "UPDATE `signalappdb`.`telephoneusers` SET `Status`='Connected',`ConnectedDate`='" +
                        DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE  `Id`=" + id + ";";
                aGateway.Update(query);
                string resolver = UtilityLibrary.GetUserId();
                string resolveQuery =
                    "INSERT INTO `signalappdb`.`complains` (`Description`, `Status`, `MenuComplainTypeId`, `TelephoneUserId`, `ComplainDate`, `ResolvedDate`, `Remarks`, `ActionTaken`, `ResolvedBy`) VALUES ('New Connection Activation', 'Resolved', 5, " +
                    id + ", '" + DateTime.Now + "', 'DateTime.Now', '" + remarks + "', 'Connection Provided', '" + resolver +
                    "');";
                aGateway.Insert(resolveQuery);
            }
        }


        public static void DisconnectPhoneOperation(string id, DBGateway aGateway, string remarks)
        {
            string query;
            query = "UPDATE `signalappdb`.`telephoneusers` SET `Status`='Connected',`DisconnectedDate`='" +
                    DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE  `Id`=" + id + ";";
            aGateway.Update(query);

            query =
                "INSERT INTO deletedtelephoneusers (deletedtelephoneusers.BANumber,deletedtelephoneusers.Name,deletedtelephoneusers.RankId,deletedtelephoneusers.`Status`,deletedtelephoneusers.NewPhoneNumber,deletedtelephoneusers.Address,deletedtelephoneusers.Gender,deletedtelephoneusers.ConnectedDate,deletedtelephoneusers.DisconnectedDate) SELECT telephoneusers.BANumber,telephoneusers.Name,telephoneusers.RankId,telephoneusers.`Status`,telephoneusers.NewPhoneNumber,telephoneusers.Address,telephoneusers.Gender,telephoneusers.ConnectedDate,telephoneusers.DisconnectedDate FROM telephoneusers where telephoneusers.id=" +
                id + "";

            aGateway.Insert(query);
            query = "delete from complains where TelephoneUserId=" + id;
            aGateway.Delete(query);

            query = "delete from  telephoneusers where id=" + id + ";";
            aGateway.Delete(query);


            string resolver = UtilityLibrary.GetUserId();
            string resolveQuery =
                "INSERT INTO `signalappdb`.`complains` (`Description`, `Status`, `MenuComplainTypeId`, `TelephoneUserId`, `ComplainDate`, `ResolvedDate`, `Remarks`, `ActionTaken`, `ResolvedBy`) VALUES ('Connection Colsed', 'Resolved', 6, " +
                id + ", '" + DateTime.Now + "', 'DateTime.Now', '" + remarks + "', 'Connection Disconnected', '" + resolver +
                "');";
            //  aGateway.Insert(resolveQuery);
        }



           

    }




    public class Telephone
    {
        DBGateway aGateway = new DBGateway("SignalSystemConnectionString");

        public bool AssignNewTelephone(string baNumber, string name, string rank, string telephoneNumber, string address, string sex, out string outputMessage)
        {
            outputMessage = string.Empty;

            if (IsTelephoneIsInUse(telephoneNumber))
            {
                outputMessage = "Error: Telephone Number already in use. ";
                return false;
            }

            string nonQuery = "INSERT INTO `signalappdb`.`telephoneusers` (`BANumber`, `RankId`, `Status`,`Name`, " +
                              "`NewPhoneNumber`, `Address`, `Gender`) " +
                              "VALUES (@BANumber, @RankId, @Status,@Name, " +
                              "@NewPhoneNumber, @Address, @Gender);";
            Hashtable aHashtable = new Hashtable()
            {
                {"BANumber",baNumber},
                {"RankId",rank},
                {"Status","Disconnected"},
                {"NewPhoneNumber",telephoneNumber},
                 {"Address",address},
                {"Gender",sex},
                {"ConnectedDate",DateTime.Now.ToString("yyyy-MM-dd")}, 
                {"Name",name}

            };
            aGateway.Insert(nonQuery, aHashtable);
            bool success = true;
            return success;

        }

        public bool IsTelephoneIsInUse(string telephoneNumber)
        {
            string query = "select * from telephoneusers where NewPhoneNumber=@telephoneNumber ;";
            Hashtable aHashtable = new Hashtable(){{"telephoneNumber",telephoneNumber}};
            DataSet aDataSet = aGateway.Select(query, aHashtable);

            if (aDataSet.Tables[0].Rows.Count > 0)
            {
                return true;
            }


            return false;
        }

        public static void DeleteComplain(string id)
        {
            string query =
                "delete from  complains where id=" + id + ";";


            DBGateway aGateway = new DBGateway();
            string deleteResult = aGateway.Delete(query);
        }

        public static FilteredLists GetFilteredComplaneList(string sSearch,
            string banumberFilter, string phoneFilter,
            string nameFilter, string rankFilter, string complainFilter, DateTime fromDate, DateTime toDate, int skipSize, int takeSize, string sortOrder, int sortColumnIndex)
        {
            string complainQuery =
               "select SQL_CALC_FOUND_ROWS complains.id,phoneuserpersonalinfo.BANumber,phoneuserpersonalinfo.FullName,menucomplainType.Value as ComplainType," +
               " allphoneinfo.PhoneNumber,menusrank.Value as Rank,complains.ComplainDate " +

               " from complains,menucomplainType,phoneuserpersonalinfo,allphoneinfo," +
               " menusRank where menucomplaintype.Id=complains.MenuComplainTypeId" +
               " and phoneuserpersonalinfo.Id = complains.TelephoneUserId and phoneuserpersonalinfo.RankId = menusrank.id and complains.AllPhoneInfoID=allphoneinfo.ID ";
            if (sSearch.Trim().Length > 0)
            {
                complainQuery += "and ((phoneuserpersonalinfo.BANumber like '%" + sSearch + "%')" +
                             "or (menucomplainType.Value like '%" + sSearch + "%')" +
                             "or (phoneuserpersonalinfo.FullName like '%" + sSearch + "%')" +
                             "or (allphoneinfo.PhoneNumber like '%" + sSearch + "%')" +
                             "or (menusrank.Value like '%" + sSearch + "%')";

            }
            if (banumberFilter.Trim().Length > 0)
            {
                complainQuery += "and (phoneuserpersonalinfo.BANumber = '" + banumberFilter + "')";
            }
            if (complainFilter.Trim().Length > 0)
            {
                complainQuery += "and (menucomplainType.Value = '" + complainFilter + "')";
            }
            if (nameFilter.Trim().Length > 0)
            {
                complainQuery += "and (phoneuserpersonalinfo.FullName like '%" + nameFilter + "%')";
            }
            if (phoneFilter.Trim().Length > 0)
            {
                complainQuery += "and (allphoneinfo.PhoneNumber like '%" + phoneFilter + "%')";
            }
            if (rankFilter.Trim().Length > 0)
            {
                complainQuery += "and (menusrank.Value = '" + rankFilter + "')";
            }

            complainQuery += " and ((complains.ComplainDate >= DATE_FORMAT('" + fromDate.ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s')) and (complains.ComplainDate <= DATE_FORMAT('" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s')))";



            List<string> tableDBColumns =
             new List<string>(new string[]
                {
                    "phoneuserpersonalinfo.BANumber", "phoneuserpersonalinfo.FullName", "menusrank.Value", "allphoneinfo.PhoneNumber",
                    "complains.ComplainDate", 
                    "menucomplainType.Value"
                });



            complainQuery += " order by " + tableDBColumns[sortColumnIndex] + " " + sortOrder + " ";

            complainQuery += " limit " + skipSize + ", " + takeSize + ";";
            complainQuery += " SELECT FOUND_ROWS();";
            complainQuery += " SELECT count(*) from complains;";
            complainQuery += "select distinct phoneuserpersonalinfo.BANumber from phoneuserpersonalinfo;select distinct menusrank.Value from menusrank;select distinct menucomplainType.Value from menucomplainType;";

            DataSet ds = MySql.Data.MySqlClient.MySqlHelper.ExecuteDataset(System.Configuration.ConfigurationManager.ConnectionStrings["SignalSystemConnectionString"].ConnectionString, complainQuery);
            int filteredCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            int lTotalCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
            List<TelephoneComplain> aList = new List<TelephoneComplain>();
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {

                TelephoneComplain aTelephoneComplain = new TelephoneComplain();
                aTelephoneComplain.BANumber = dataRow["BANumber"].ToString();
                aTelephoneComplain.Name = dataRow["FullName"].ToString();
                aTelephoneComplain.ComplainType = dataRow["ComplainType"].ToString();
                aTelephoneComplain.NewPhoneNumber = dataRow["PhoneNumber"].ToString();
                aTelephoneComplain.Rank = dataRow["Rank"].ToString();
               
                aTelephoneComplain.ComplainDate = dataRow["ComplainDate"].ToString();
                aTelephoneComplain.ComplainId = dataRow["Id"].ToString();
                // aTelephoneComplain.Status = dataRow["Status"].ToString();

                aList.Add(aTelephoneComplain);
            }
            List<string> BANumberList = new List<string>();

            foreach (DataRow dataRow in ds.Tables[3].Rows)
            {
                BANumberList.Add(dataRow["BANumber"].ToString());
            }

            List<string> RankList = new List<string>();

            foreach (DataRow dataRow in ds.Tables[4].Rows)
            {
                RankList.Add(dataRow["Value"].ToString());
            }


            List<string> ComplainTypeList = new List<string>();

            foreach (DataRow dataRow in ds.Tables[5].Rows)
            {
                ComplainTypeList.Add(dataRow["Value"].ToString());
            }


            FilteredLists aFilteredComplains = new FilteredLists();
            aFilteredComplains.aComplainList = aList;
            aFilteredComplains.FilteredSize = filteredCount;
            aFilteredComplains.TotalSize = lTotalCount;
            aFilteredComplains.BaNumberList = BANumberList;
            aFilteredComplains.RankList = RankList;
            aFilteredComplains.ComplainTypeList = ComplainTypeList;
          
            //List<TelephoneComplain> searchedComplains;
            //sSearch = sSearch.Trim();


            //if ((sSearch == ""))
            //{
            //    searchedComplains = complanList;
            //}
            //else
            //{
            //    searchedComplains =
            //        complanList.Where(c => c.BANumber.ToLower().Trim().Contains(sSearch.ToLower())
            //                               || c.ComplainType.Trim().ToLower().Contains(sSearch.ToLower()) ||
            //                               c.Name.Trim().ToLower().Contains(sSearch.ToLower()) ||
            //                               c.NewPhoneNumber.Trim().ToLower().Contains(sSearch.ToLower()) ||
            //                               c.ComplainType.Trim().ToLower().Contains(sSearch.ToLower())).ToList();
            //}


            //var filteredCompanies = searchedComplains
            //    .Where(c => (banumberFilter == "" || c.BANumber.Trim().ToLower() == banumberFilter.Trim().ToLower())
            //                &&
            //                (phoneFilter == "" || c.NewPhoneNumber.Trim().ToLower().Contains(phoneFilter.Trim()))
            //                &&
            //                (nameFilter == "" || c.Name.ToLower().Contains(nameFilter.Trim().ToLower()))
            //                &&
            //                (rankFilter == "" || c.Rank.Trim().ToLower() == rankFilter.Trim().ToLower())
            //                &&
            //                (complainFilter == "" || c.ComplainType.Trim().ToLower() == complainFilter.Trim().ToLower())
            //                &&
            //                (fromDate == DateTime.MinValue || fromDate < Convert.ToDateTime(c.ComplainDate))
            //                &&
            //                (toDate == DateTime.MaxValue || Convert.ToDateTime(c.ComplainDate) < toDate)
            //    );
            //List<TelephoneComplain> filteredComplaneList = filteredCompanies.ToList();
            return aFilteredComplains;
        }

        public static void EditComplain(TelephoneComplain aTelephoneComplain, string updateQuery)
        {
            string complainStatus;
            DBGateway aGateway = new DBGateway();
            if (aTelephoneComplain.Status == "0")
            {
                complainStatus = "Pending";
                updateQuery = "UPDATE `signalappdb`.`complains` SET `Description`='" + aTelephoneComplain.Description +
                              "', `Status`='" + complainStatus + "', `MenuComplainTypeId`=" +
                              aTelephoneComplain.ComplainType + " WHERE  `Id`=" + aTelephoneComplain.ComplainId + ";";
                string updateResult = aGateway.Update(updateQuery);
            }
            else if (aTelephoneComplain.Status == "1")
            {
                Telephone aTelephone = new Telephone();
                string convertedDateText = aTelephone.DMYToMDY(aTelephoneComplain.ResolvedDate);
                DateTime dt = DateTime.Parse(convertedDateText);
                string userName = UtilityLibrary.GetUserId();

                complainStatus = "Resolved";
                updateQuery = "UPDATE `signalappdb`.`complains` SET `Description`='" + aTelephoneComplain.Description +
                              "', `Status`='" + complainStatus + "', `Remarks`='" + aTelephoneComplain.Remarks +
                              "', `MenuComplainTypeId`=" + aTelephoneComplain.ComplainType + ", `ResolvedDate`='" +
                              dt.ToString("yyyy-MM-dd HH:mm:ss") + "', `ActionTaken`='" + aTelephoneComplain.ActionTaken +
                              "', `ResolvedBy`='" + userName + "' WHERE  `Id`=" + aTelephoneComplain.ComplainId + ";";

                //string insertQuery = "INSERT INTO `signalappdb`.`resolvedcomplains` (`Description`, `Status`, `MenuComplainTypeId`, `TelephoneUserId`, `ComplainDate`, `ResolvedDate`, `Remarks`, `ActionTaken`, `ResolvedBy`, `AllPhoneInfoID`) VALUES ('"+aTelephoneComplain.Description+"', 'resolved', "+aTelephoneComplain.ProblemTypes+", (select ), '2015-12-17 23:07:26', '2015-12-19 23:07:26', 'o', 'dd', 'maruf', 10);";
                string insertQuery = "INSERT INTO `signalappdb`.`resolvedcomplains` (`Description`, `Status`, `MenuComplainTypeId`, `TelephoneUserId`,`AllPhoneInfoID`,`ComplainDate`, `ResolvedDate`, `Remarks`, `ActionTaken`, `ResolvedBy`) select description,status, MenuComplainTypeId,TelephoneUserId,AllPhoneInfoID,ComplainDate,'" +dt.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + aTelephoneComplain.Remarks + "', '" + aTelephoneComplain.ActionTaken + "', '" + aTelephoneComplain.ResolveBy + "' from complains WHERE ID=" + aTelephoneComplain.ComplainId + ";";
                int insertCount = aGateway.Insert(insertQuery);
                string deleteQuery = "Delete From Complains where id=" + aTelephoneComplain.ComplainId + "";
                string delMsg = aGateway.Delete(deleteQuery);
            }


            
            
        }



        public static List<TelephoneComplain> GetTelephoneComplain(string id,string type)
        {
            string queryResolve = "select resolvedcomplains.id,phoneuserpersonalinfo.BANumber,phoneuserpersonalinfo.FullName,menucomplainType.Value as ComplainType," +
" allphoneinfo.PhoneNumber,menusrank.Value as Rank,resolvedcomplains.Description,resolvedcomplains.ComplainDate,resolvedcomplains.Remarks,resolvedcomplains.ResolvedDate,resolvedcomplains.ResolvedBy" +
" from resolvedcomplains,menucomplainType,phoneuserpersonalinfo,allphoneinfo," +
" menusRank where menucomplaintype.Id=resolvedcomplains.MenuComplainTypeId" +
" and phoneuserpersonalinfo.Id = resolvedcomplains.TelephoneUserId and phoneuserpersonalinfo.RankId = menusrank.id and resolvedcomplains.AllPhoneInfoID=allphoneinfo.ID and resolvedcomplains.id=" +
              id + ";";


            string queryPending = "select complains.id,phoneuserpersonalinfo.BANumber,phoneuserpersonalinfo.FullName,menucomplainType.Value as ComplainType," +
" allphoneinfo.PhoneNumber,menusrank.Value as Rank,complains.Description,complains.ComplainDate,complains.Remarks,complains.ResolvedDate,complains.ResolvedBy" +
" from complains,menucomplainType,phoneuserpersonalinfo,allphoneinfo," +
" menusRank where menucomplaintype.Id=complains.MenuComplainTypeId" +
" and phoneuserpersonalinfo.Id = complains.TelephoneUserId and phoneuserpersonalinfo.RankId = menusrank.id and complains.AllPhoneInfoID=allphoneinfo.ID and complains.id=" +
                id + ";";



            List<TelephoneComplain> aTelephoneComplainList = new List<TelephoneComplain>();
            DBGateway aGateway = new DBGateway();
            DataSet aDataSet;
            if (type == "resolved")
            {
                aDataSet=aGateway.Select(queryResolve);
            }
            else
            {
                aDataSet=aGateway.Select(queryPending);
            }
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {
                TelephoneComplain aTelephoneComplain = new TelephoneComplain();
                aTelephoneComplain.BANumber = dataRow["BANumber"].ToString();
                aTelephoneComplain.Name = dataRow["FullName"].ToString();
                aTelephoneComplain.ComplainType = dataRow["ComplainType"].ToString();
                aTelephoneComplain.NewPhoneNumber = dataRow["PhoneNumber"].ToString();
                aTelephoneComplain.Rank = dataRow["Rank"].ToString();
                aTelephoneComplain.Description = dataRow["Description"].ToString();
                aTelephoneComplain.ComplainDate = dataRow["ComplainDate"].ToString();
                aTelephoneComplain.ComplainId = dataRow["Id"].ToString();
                //aTelephoneComplain.ActionTaken = dataRow["ActionTaken"].ToString();
                aTelephoneComplain.ResolvedDate = dataRow["ResolvedDate"].ToString();
                aTelephoneComplain.Remarks = dataRow["Remarks"].ToString();
                aTelephoneComplain.ResolveBy = dataRow["ResolvedBy"].ToString();
                aTelephoneComplainList.Add(aTelephoneComplain);
            }

            List<TelephoneComplainType> complainTypes = new List<TelephoneComplainType>();

            string queryType = "select * from menucomplaintype";
            DataSet ctDataSet = aGateway.Select(queryType);
            foreach (DataRow dataRow in ctDataSet.Tables[0].Rows)
            {
                TelephoneComplainType aType = new TelephoneComplainType();
                aType.TypeId = dataRow["Id"].ToString();
                aType.TypeValue = dataRow["Value"].ToString();
                complainTypes.Add(aType);
            }
            aTelephoneComplainList.ForEach(list => list.ProblemTypes = complainTypes);
            return aTelephoneComplainList;
        }



        public Dictionary<string, string> GetUserInfo(string telephoneNumber)
        {
            Dictionary<string,string> aDictionary = new Dictionary<string, string>();

            string query = "select api.ID as AllPhoneInfoID,api.PhoneNumber,pupi.ID as " +
                           "PhoneUserPersoanlInfoID,pupi.BANumber,pupi.FullName,mr.Value as Rank,aapi.PhoneUsedFor," +
                           "aapi.HomeAddress,aapi.OfficeAddress from  allphoneinfo as api left join allactivephoneinfo " +
                           "as aapi on aapi.AllPhoneInfoID=api.ID left join phoneuserpersonalinfo as pupi on pupi.ID=aapi.PhoneUserPersonalInfoId  " +
                           "left join menusrank as mr on mr.id=pupi.RankId  where api.ServiceStatus='Active' " +
                           "and api.PhoneNumber=@PhoneNumber";
         
            Hashtable hashtable = new Hashtable()
            {
                {"PhoneNumber",telephoneNumber}
            };

            DataSet aSet = aGateway.Select(query, hashtable);
            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                aDictionary.Add("AllPhoneInfoID", dataRow["AllPhoneInfoID"].ToString());
                aDictionary.Add("PhoneNumber", dataRow["PhoneNumber"].ToString());
                aDictionary.Add("BANumber", dataRow["BANumber"].ToString());
                aDictionary.Add("FullName", dataRow["FullName"].ToString());
                aDictionary.Add("PhoneUserPersoanlInfoID", dataRow["PhoneUserPersoanlInfoID"].ToString());

                aDictionary.Add("Rank", dataRow["Rank"].ToString()); 
                if (dataRow["PhoneUsedFor"].ToString().ToLower() == "home")
                {
                    aDictionary.Add("Address", dataRow["HomeAddress"].ToString());
                }
                else
                {
                    aDictionary.Add("Address", dataRow["OfficeAddress"].ToString());
                }  
            }
            return aDictionary;
        }

        public bool SubmitNewComplain(string phoneUserInfoId, string allPhoneInfoID, string description, string complainTypeId)
        {
            string nonQuery =
                "INSERT INTO `signalappdb`.`complains` (`Description`, `Status`, " +
                "`TelephoneUserId`, `MenuComplainTypeId`,`ComplainDate`,`AllPhoneInfoID`) " +
                "VALUES (@Description, @Status, @UserId, @MenuComplainTypeId,@ComplainDate,@AllPhoneInfoID);";

            Hashtable aHashtable = new Hashtable()
            {
                {"Description",description},
                {"Status", "Pending"},
                {"UserId",phoneUserInfoId},
                {"MenuComplainTypeId",complainTypeId},
                {"ComplainDate",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                {"AllPhoneInfoID",allPhoneInfoID}


            };
            aGateway.Insert(nonQuery, aHashtable);

            return true;
        }

        public String DMYToMDY(String input)
        {
            return Regex.Replace(input,
            @"\b(?<day>\d{1,2})/(?<month>\d{1,2})/(?<year>\d{2,4})\b",
            "${month}/${day}/${year}");
        }



        public List<Dictionary<string, string>> GetResolvedComplains()
        {
            List<Dictionary<string, string>> pendingComplains = new List<Dictionary<string, string>>();

            string query =
                "select telephoneusers.BANumber,telephoneusers.Name,menucomplainType.Value as " +
                "ComplainType,telephoneusers.NewPhoneNumber,menusrank.Value as Rank, " +
                "complains.Description,complains.ComplainDate,complains.ResolvedDate,complains.Remarks,complains.ActionTaken,complains.ResolvedBy from complains,menucomplainType,telephoneusers," +
                "menusRank where complains.`Status`<>'Pending' and menucomplaintype.Id=complains.MenuComplainTypeId " +
                "and telephoneusers.Id = complains.TelephoneUserId and telephoneusers.RankId = menusrank.id;";
            DataSet aDataSet = aGateway.Select(query);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {
                Dictionary<string, string> aDictionary = new Dictionary<string, string>();
                aDictionary.Add("BANumber", dataRow["BANumber"].ToString());
                aDictionary.Add("Name", dataRow["Name"].ToString());
                aDictionary.Add("ComplainType", dataRow["ComplainType"].ToString());
                aDictionary.Add("NewPhoneNumber", dataRow["NewPhoneNumber"].ToString());
                aDictionary.Add("Rank", dataRow["Rank"].ToString());
                aDictionary.Add("Description", dataRow["Description"].ToString());
                aDictionary.Add("ComplainDate", dataRow["ComplainDate"].ToString());
                aDictionary.Add("ResolvedDate", dataRow["ResolvedDate"].ToString());
                aDictionary.Add("Remarks", dataRow["Remarks"].ToString());
                aDictionary.Add("ActionTaken", dataRow["ActionTaken"].ToString());
                aDictionary.Add("ResolvedBy", dataRow["ResolvedBy"].ToString());
                pendingComplains.Add(aDictionary);
            }

            return pendingComplains;
        }

        public static FilteredLists GetResolvedFilteredComplaneList(string sSearch, string banumberFilter, string phoneFilter,
            string nameFilter, string rankFilter, string complainFilter, DateTime fromDate, DateTime toDate,
            string resolvedByFilter, DateTime resolverFromdate, DateTime resolverTodate, string actionFilter,
            string remarksFilter,int skipSize,int takeSize,string sortOrder,int sortColumnIndex)
        {
            string complainQuery =
                "select SQL_CALC_FOUND_ROWS resolvedcomplains.id,resolvedcomplains.id,phoneuserpersonalinfo.BANumber,phoneuserpersonalinfo.FullName,menucomplainType.Value as ComplainType," +
                " allphoneinfo.PhoneNumber,menusrank.Value as Rank,resolvedcomplains.Description,resolvedcomplains.ComplainDate,resolvedcomplains.ResolvedBy," +
                " resolvedcomplains.ActionTaken,resolvedcomplains.ResolvedDate,resolvedcomplains.Remarks" +
                " from resolvedcomplains,menucomplainType,phoneuserpersonalinfo,allphoneinfo," +
                " menusRank where menucomplaintype.Id=resolvedcomplains.MenuComplainTypeId" +
                " and phoneuserpersonalinfo.Id = resolvedcomplains.TelephoneUserId and phoneuserpersonalinfo.RankId = menusrank.id and resolvedcomplains.AllPhoneInfoID=allphoneinfo.ID ";
            if (sSearch.Trim().Length > 0)
            {
                complainQuery += "and ((phoneuserpersonalinfo.BANumber like '%"+sSearch+"%')" +
                             "or (menucomplainType.Value like '%"+sSearch+"%')" +
                             "or (phoneuserpersonalinfo.FullName like '%"+sSearch+"%')" +
                             "or (allphoneinfo.PhoneNumber like '%"+sSearch+"%')" +
                             "or (menusrank.Value like '%"+sSearch+"%')" +
                            // "or (resolvedcomplains.Description like '%"+sSearch+"%')" +
                             "or (resolvedcomplains.ResolvedBy like '%"+sSearch+"%')" +
                             "or (resolvedcomplains.ActionTaken like '%"+sSearch+"%')" +
                             "or (resolvedcomplains.Remarks like '%"+sSearch+"%'))";
            }
            if (banumberFilter.Trim().Length > 0)
            {
                complainQuery += "and (phoneuserpersonalinfo.BANumber = '"+banumberFilter+"')";
            }
            if (complainFilter.Trim().Length > 0)
            {
                complainQuery += "and (menucomplainType.Value = '" + complainFilter + "')";
            }
            if (nameFilter.Trim().Length > 0)
            {
                complainQuery += "and (phoneuserpersonalinfo.FullName like '%" + nameFilter + "%')";
            }
            if (phoneFilter.Trim().Length > 0)
            {
                complainQuery += "and (allphoneinfo.PhoneNumber like '%" + phoneFilter + "%')";
            }
            if (rankFilter.Trim().Length > 0)
            {
                complainQuery += "and (menusrank.Value = '" + rankFilter + "')";
            }
            if (resolvedByFilter.Trim().Length > 0)
            {
                complainQuery += "and (resolvedcomplains.ResolvedBy like '%" + resolvedByFilter + "%')";
            }
            if (actionFilter.Trim().Length > 0)
            {
                complainQuery += "and (resolvedcomplains.ActionTaken like '%" + actionFilter + "%')";
            }
            if (remarksFilter.Trim().Length > 0)
            {
                complainQuery += "and (resolvedcomplains.Remarks like '%" + remarksFilter + "%')";
            }
            complainQuery += " and ((resolvedcomplains.ComplainDate >= DATE_FORMAT('" + fromDate.ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s')) and (resolvedcomplains.ComplainDate <= DATE_FORMAT('" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s')))";
            complainQuery += " and ((resolvedcomplains.ResolvedDate >= DATE_FORMAT('" + resolverFromdate.ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s')) and (resolvedcomplains.ResolvedDate <= DATE_FORMAT('" + resolverTodate.ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s'))) ";

     
            List<string> tableDBColumns =
             new List<string>(new string[]
                {
                    "phoneuserpersonalinfo.BANumber", "phoneuserpersonalinfo.FullName", "menusrank.Value", "allphoneinfo.PhoneNumber",
                    "resolvedcomplains.ComplainDate", 
                    "menucomplainType.Value", "resolvedcomplains.ResolvedBy",
                    "resolvedcomplains.ResolvedDate", "resolvedcomplains.ActionTaken", "resolvedcomplains.Remarks"
                });

      
           
                complainQuery += " order by "+tableDBColumns[sortColumnIndex]+" "+sortOrder+" ";
            
            complainQuery+=" limit "+skipSize+", "+takeSize+";"; 
            complainQuery+=" SELECT FOUND_ROWS();";
            complainQuery += " SELECT count(*) from resolvedcomplains;";
            complainQuery += "select distinct phoneuserpersonalinfo.BANumber from phoneuserpersonalinfo;select distinct menusrank.Value from menusrank;select distinct menucomplainType.Value from menucomplainType; select distinct resolvedcomplains.ResolvedBy from resolvedcomplains where resolvedcomplains.ResolvedBy!='';";

            
            DataSet ds = MySql.Data.MySqlClient.MySqlHelper.ExecuteDataset(System.Configuration.ConfigurationManager.ConnectionStrings["SignalSystemConnectionString"].ConnectionString, complainQuery);
            int filteredCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            int lTotalCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
            List<TelephoneComplain> aList = new List<TelephoneComplain>();
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {

                TelephoneComplain aTelephoneComplain = new TelephoneComplain();
                aTelephoneComplain.BANumber = dataRow["BANumber"].ToString();
                aTelephoneComplain.Name = dataRow["FullName"].ToString();
                aTelephoneComplain.ComplainType = dataRow["ComplainType"].ToString();
                aTelephoneComplain.NewPhoneNumber = dataRow["PhoneNumber"].ToString();
                aTelephoneComplain.Rank = dataRow["Rank"].ToString();
                aTelephoneComplain.Description = dataRow["Description"].ToString();
                aTelephoneComplain.ComplainDate = dataRow["ComplainDate"].ToString();
                aTelephoneComplain.ComplainId = dataRow["Id"].ToString();
                // aTelephoneComplain.Status = dataRow["Status"].ToString();

                aTelephoneComplain.ResolveBy = dataRow["ResolvedBy"].ToString();
                aTelephoneComplain.ResolvedDate = dataRow["ResolvedDate"].ToString();
                aTelephoneComplain.ActionTaken = dataRow["ActionTaken"].ToString();
                aTelephoneComplain.Remarks = dataRow["Remarks"].ToString();
                aList.Add(aTelephoneComplain);
            }
            List<string> BANumberList=new List<string>();

            foreach (DataRow dataRow in ds.Tables[3].Rows)
            {
                BANumberList.Add(dataRow["BANumber"].ToString());
            }

             List<string> RankList=new List<string>();

            foreach (DataRow dataRow in ds.Tables[4].Rows)
            {
                RankList.Add(dataRow["Value"].ToString());
            }


            List<string> ComplainTypeList = new List<string>();

            foreach (DataRow dataRow in ds.Tables[5].Rows)
            {
                ComplainTypeList.Add(dataRow["Value"].ToString());
            }

            List<string> ResolverList = new List<string>();

            foreach (DataRow dataRow in ds.Tables[6].Rows)
            {
                ResolverList.Add(dataRow["ResolvedBy"].ToString());
            }

            FilteredLists aFilteredComplains = new FilteredLists();
            aFilteredComplains.aComplainList = aList;
            aFilteredComplains.FilteredSize = filteredCount;
            aFilteredComplains.TotalSize = lTotalCount;
            aFilteredComplains.BaNumberList = BANumberList;
            aFilteredComplains.RankList = RankList;
            aFilteredComplains.ComplainTypeList = ComplainTypeList;
            aFilteredComplains.ResolverList = ResolverList;
            return aFilteredComplains;
        }


        




        internal List<Dictionary<string, string>> GetResolvedComplainsByPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }



        public FilteredLists GetFilteredPhoneList(string sSearch,string banumberFilter, string phoneFilter, string nameFilter, string rankFilter, string homeAddressFilter, string officeAddressFilter, string phoneTypeFilter, string genderFilter, DateTime fromDate, DateTime toDate,int skipSize,int takeSize,string sortOrder,int sortColumnIndex)
        {

            string query3 = "select allactivephoneinfo.id,phoneuserpersonalinfo.BANumber,phoneuserpersonalinfo.FullName,menusrank.Value as Rank,allphoneinfo.PhoneNumber,allactivephoneinfo.PhoneUsedFor,allactivephoneinfo.HomeAddress,allactivephoneinfo.OfficeAddress,phoneuserpersonalinfo.Sex as Gender,allactivephoneinfo.ConnectDate from phoneuserpersonalinfo,menusrank,allactivephoneinfo,allphoneinfo where menusrank.id=phoneuserpersonalinfo.RankId and phoneuserpersonalinfo.ID=allactivephoneinfo.PhoneUserPersonalInfoId and allphoneinfo.ID=allactivephoneinfo.AllPhoneInfoID;";

            string complainQuery =

               " select SQL_CALC_FOUND_ROWS allactivephoneinfo.id,Banumber,fullname,menusrank.value as Rank,Phonenumber,phoneusedfor,Homeaddress,allactivephoneinfo.OfficeAddress,phoneuserpersonalinfo.Sex,connectdate from allactivephoneinfo,phoneuserpersonalinfo,allphoneinfo,menusrank where phoneuserpersonalinfo.ID=allactivephoneinfo.PhoneUserPersonalInfoId and allactivephoneinfo.AllPhoneInfoID=allphoneinfo.ID and phoneuserpersonalinfo.RankId=menusrank.id ";
            if (sSearch.Trim().Length > 0)
            {
                complainQuery += "and ((phoneuserpersonalinfo.BANumber like '%" + sSearch + "%')" +
                             " or (phoneuserpersonalinfo.FullName like '%" + sSearch + "%')" +
                             " or (allphoneinfo.PhoneNumber like '%" + sSearch + "%')" +
                              "  or (allactivephoneinfo.PhoneUsedFor like '%" + sSearch + "%')" +
                              " or (allactivephoneinfo.HomeAddress like '%" + sSearch + "%')" +
                               " or (allactivephoneinfo.OfficeAddress like '%" + sSearch + "%')" +
                               " or (phoneuserpersonalinfo.Sex like '%" + sSearch + "%')" +
                             " or (menusrank.Value like '%" + sSearch + "%'))";

            }
            if (banumberFilter.Trim().Length > 0)
            {
                complainQuery += "and (phoneuserpersonalinfo.BANumber = '" + banumberFilter + "')";
            }
            if (phoneTypeFilter.Trim().Length > 0)
            {
                complainQuery += "and (allactivephoneinfo.PhoneUsedFor like '%" + phoneTypeFilter + "%')";
            }
            if (nameFilter.Trim().Length > 0)
            {
                complainQuery += "and (phoneuserpersonalinfo.FullName like '%" + nameFilter + "%')";
            }
            if (phoneFilter.Trim().Length > 0)
            {
                complainQuery += "and (allphoneinfo.PhoneNumber like '%" + phoneFilter + "%')";
            }
            if (rankFilter.Trim().Length > 0)
            {
                complainQuery += "and (menusrank.Value = '" + rankFilter + "')";
            }

            if (homeAddressFilter.Trim().Length > 0)
            {
                complainQuery += "and (allactivephoneinfo.HomeAddress like '%" + homeAddressFilter + "%')";
            }

            if (officeAddressFilter.Trim().Length > 0)
            {
                complainQuery += "and (allactivephoneinfo.OfficeAddress like '%" + officeAddressFilter + "%')";
            }

            complainQuery += " and ((allactivephoneinfo.ConnectDate >= DATE_FORMAT('" + fromDate.ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s')) and (allactivephoneinfo.ConnectDate <= DATE_FORMAT('" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s')))";



            List<string> tableDBColumns =
             new List<string>(new string[]
                {
                    "phoneuserpersonalinfo.BANumber", "phoneuserpersonalinfo.FullName", "menusrank.Value", "allphoneinfo.PhoneNumber",
                    "allactivephoneinfo.PhoneUsedFor", "allactivephoneinfo.HomeAddress","allactivephoneinfo.OfficeAddress","phoneuserpersonalinfo.Sex",
                    "allactivephoneinfo.ConnectDate"
                });



            complainQuery += " order by " + tableDBColumns[sortColumnIndex] + " " + sortOrder + " ";

            complainQuery += " limit " + skipSize + ", " + takeSize + ";";
            complainQuery += " SELECT FOUND_ROWS();";
            complainQuery += " SELECT count(*) from allactivephoneinfo;";
            complainQuery += "select distinct phoneuserpersonalinfo.BANumber from phoneuserpersonalinfo;select distinct menusrank.Value from menusrank;select distinct allactivephoneinfo.PhoneUsedFor from allactivephoneinfo;";

            DataSet ds = MySql.Data.MySqlClient.MySqlHelper.ExecuteDataset(System.Configuration.ConfigurationManager.ConnectionStrings["SignalSystemConnectionString"].ConnectionString, complainQuery);
            int filteredCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            int lTotalCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
            List<TelphoneUser> aList = new List<TelphoneUser>();
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {TelephoneComplain aTelephoneComplain = new TelephoneComplain();
                TelphoneUser aTelphoneUser = new TelphoneUser();
                aTelphoneUser.BANumber = dataRow["BANumber"].ToString();
                aTelphoneUser.Name = dataRow["FullName"].ToString();

                aTelphoneUser.NewPhoneNumber = dataRow["PhoneNumber"].ToString();
                aTelphoneUser.Rank = dataRow["Rank"].ToString();

                aTelphoneUser.ConnectedDate = dataRow["ConnectDate"].ToString();
                // aTelphoneUser.DisconnectedDate = dataRow["DisconnectedDate"].ToString();
                aTelphoneUser.ID = dataRow["Id"].ToString();
                // aTelphoneUser.PhoneStatus = dataRow["Status"].ToString();

                aTelphoneUser.HomeAddress = dataRow["HomeAddress"].ToString();
                aTelphoneUser.OfficeAddress = dataRow["OfficeAddress"].ToString();
                aTelphoneUser.Gender = dataRow["Sex"].ToString();
                aTelphoneUser.PhoneType = dataRow["PhoneUsedFor"].ToString();

                aList.Add(aTelphoneUser);
                
            }
            List<string> BANumberList = new List<string>();

            foreach (DataRow dataRow in ds.Tables[3].Rows)
            {
                BANumberList.Add(dataRow["BANumber"].ToString());
            }

            List<string> RankList = new List<string>();

            foreach (DataRow dataRow in ds.Tables[4].Rows)
            {
                RankList.Add(dataRow["Value"].ToString());
            }


            List<string> PhoneTypeList = new List<string>();

            foreach (DataRow dataRow in ds.Tables[5].Rows)
            {
                PhoneTypeList.Add(dataRow["PhoneUsedFor"].ToString());
            }
            
            List<TelphoneUser> listTelphoneUsers = new List<TelphoneUser>();
            DBGateway aGateway = new DBGateway();
            DataSet aDataSet = aGateway.Select(query3);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {

                TelphoneUser aTelphoneUser = new TelphoneUser();
                aTelphoneUser.BANumber = dataRow["BANumber"].ToString();
                aTelphoneUser.Name = dataRow["FullName"].ToString();

                aTelphoneUser.NewPhoneNumber = dataRow["PhoneNumber"].ToString();
                aTelphoneUser.Rank = dataRow["Rank"].ToString();

                aTelphoneUser.ConnectedDate = dataRow["ConnectDate"].ToString();
                // aTelphoneUser.DisconnectedDate = dataRow["DisconnectedDate"].ToString();
                aTelphoneUser.ID = dataRow["Id"].ToString();
                // aTelphoneUser.PhoneStatus = dataRow["Status"].ToString();

                aTelphoneUser.HomeAddress = dataRow["HomeAddress"].ToString();
                aTelphoneUser.OfficeAddress = dataRow["OfficeAddress"].ToString();
                aTelphoneUser.Gender = dataRow["Gender"].ToString();
                aTelphoneUser.PhoneType = dataRow["PhoneUsedFor"].ToString();

                listTelphoneUsers.Add(aTelphoneUser);
            }
            List<TelphoneUser> filteredPhoneList = listTelphoneUsers;
            FilteredLists aFilteredList=new FilteredLists();
            aFilteredList.aTelePhoneUserList = aList;
            aFilteredList.FilteredSize = filteredCount;
            aFilteredList.TotalSize = lTotalCount;
            aFilteredList.BaNumberList = BANumberList;
            aFilteredList.RankList = RankList;
            aFilteredList.PhoneTypeList = PhoneTypeList;

            return aFilteredList;
        }



               

    }






}


