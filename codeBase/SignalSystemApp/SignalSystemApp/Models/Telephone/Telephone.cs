using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
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
        public string Address { get; set; }
        public string Gender { get; set; }
        public string PhoneStatus { set; get; }
        public string ConnectedDate { get; set; }
        public string DisconnectedDate { get; set; }

        public List<TelphoneUser> GetTelphoneUsers()
        {
             DBGateway aGateway=new DBGateway();
             string query = "select telephoneusers.Id,telephoneusers.BANumber,telephoneusers.Name,menusrank.Value as Rank,telephoneusers.NewPhoneNumber,telephoneusers.Address,telephoneusers.Gender,telephoneusers.`Status`,telephoneusers.ConnectedDate,telephoneusers.DisconnectedDate from telephoneusers,menusrank where telephoneusers.RankId=menusrank.id";
             List<TelphoneUser> listTelphoneUsers=new List<TelphoneUser>();
             DataSet aDataSet = aGateway.Select(query);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {

                TelphoneUser aTelphoneUser = new TelphoneUser();
                aTelphoneUser.BANumber = dataRow["BANumber"].ToString();
                aTelphoneUser.Name = dataRow["Name"].ToString();

                aTelphoneUser.NewPhoneNumber = dataRow["NewPhoneNumber"].ToString();
                aTelphoneUser.Rank = dataRow["Rank"].ToString();

                aTelphoneUser.ConnectedDate = dataRow["ConnectedDate"].ToString();
                aTelphoneUser.DisconnectedDate = dataRow["DisconnectedDate"].ToString();
                aTelphoneUser.ID = dataRow["Id"].ToString();
                aTelphoneUser.PhoneStatus = dataRow["Status"].ToString();

                aTelphoneUser.Address = dataRow["Address"].ToString();
                aTelphoneUser.Gender = dataRow["Gender"].ToString();

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
                aTelphoneUser.Address = dataRow["Address"].ToString();
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

        public static List<TelephoneComplain> GetFilteredComplaneList(string sSearch, List<TelephoneComplain> complanList,
            string banumberFilter, string phoneFilter,
            string nameFilter, string rankFilter, string complainFilter, DateTime fromDate, DateTime toDate)
        {
            List<TelephoneComplain> searchedComplains;
            sSearch = sSearch.Trim();


            if ((sSearch == ""))
            {
                searchedComplains = complanList;
            }
            else
            {
                searchedComplains =
                    complanList.Where(c => c.BANumber.ToLower().Trim().Contains(sSearch.ToLower())
                                           || c.ComplainType.Trim().ToLower().Contains(sSearch.ToLower()) ||
                                           c.Name.Trim().ToLower().Contains(sSearch.ToLower()) ||
                                           c.NewPhoneNumber.Trim().ToLower().Contains(sSearch.ToLower()) ||
                                           c.ComplainType.Trim().ToLower().Contains(sSearch.ToLower())).ToList();
            }


            var filteredCompanies = searchedComplains
                .Where(c => (banumberFilter == "" || c.BANumber.Trim().ToLower() == banumberFilter.Trim().ToLower())
                            &&
                            (phoneFilter == "" || c.NewPhoneNumber.Trim().ToLower().Contains(phoneFilter.Trim()))
                            &&
                            (nameFilter == "" || c.Name.ToLower().Contains(nameFilter.Trim().ToLower()))
                            &&
                            (rankFilter == "" || c.Rank.Trim().ToLower() == rankFilter.Trim().ToLower())
                            &&
                            (complainFilter == "" || c.ComplainType.Trim().ToLower() == complainFilter.Trim().ToLower())
                            &&
                            (fromDate == DateTime.MinValue || fromDate < Convert.ToDateTime(c.ComplainDate))
                            &&
                            (toDate == DateTime.MaxValue || Convert.ToDateTime(c.ComplainDate) < toDate)
                );
            List<TelephoneComplain> filteredComplaneList = filteredCompanies.ToList();
            return filteredComplaneList;
        }

        public static void EditComplain(TelephoneComplain aTelephoneComplain, string updateQuery)
        {
            string complainStatus;
            if (aTelephoneComplain.Status == "0")
            {
                complainStatus = "Pending";
                updateQuery = "UPDATE `signalappdb`.`complains` SET `Description`='" + aTelephoneComplain.Description +
                              "', `Status`='" + complainStatus + "', `MenuComplainTypeId`=" +
                              aTelephoneComplain.ComplainType + " WHERE  `Id`=" + aTelephoneComplain.ComplainId + ";";
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
            }


            DBGateway aGateway = new DBGateway();
            string updateResult = aGateway.Update(updateQuery);
        }



        public static List<TelephoneComplain> GetTelephoneComplain(string id)
        {
            string query =
                "select complains.id,telephoneusers.BANumber,telephoneusers.Name,menucomplainType.Value as " +
                "ComplainType,telephoneusers.NewPhoneNumber,menusrank.Value as Rank, " +
                "complains.Description,complains.ComplainDate,complains.ResolvedBy,complains.ResolvedDate,complains.Remarks,complains.ActionTaken from complains,menucomplainType,telephoneusers," +
                "menusRank where menucomplaintype.Id=complains.MenuComplainTypeId " +
                "and telephoneusers.Id = complains.TelephoneUserId and telephoneusers.RankId = menusrank.id and complains.id=" +
                id + ";";

            List<TelephoneComplain> aTelephoneComplainList = new List<TelephoneComplain>();
            DBGateway aGateway = new DBGateway();
            DataSet aDataSet = aGateway.Select(query);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {
                TelephoneComplain aTelephoneComplain = new TelephoneComplain();
                aTelephoneComplain.BANumber = dataRow["BANumber"].ToString();
                aTelephoneComplain.Name = dataRow["Name"].ToString();
                aTelephoneComplain.ComplainType = dataRow["ComplainType"].ToString();
                aTelephoneComplain.NewPhoneNumber = dataRow["NewPhoneNumber"].ToString();
                aTelephoneComplain.Rank = dataRow["Rank"].ToString();
                aTelephoneComplain.Description = dataRow["Description"].ToString();
                aTelephoneComplain.ComplainDate = dataRow["ComplainDate"].ToString();
                aTelephoneComplain.ComplainId = dataRow["Id"].ToString();
                aTelephoneComplain.ActionTaken = dataRow["ActionTaken"].ToString();
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

        public List<Dictionary<string, string>> GetPendingComplains()
        {
            List<Dictionary<string,string>> pendingComplains = new List<Dictionary<string, string>>();

            string query =
                "select complains.id,telephoneusers.BANumber,telephoneusers.Name,menucomplainType.Value as " +
                "ComplainType,telephoneusers.NewPhoneNumber,menusrank.Value as Rank, " +
                "complains.Description,complains.ComplainDate from complains,menucomplainType,telephoneusers," +
                "menusRank where complains.`Status`='Pending' and menucomplaintype.Id=complains.MenuComplainTypeId " +
                "and telephoneusers.Id = complains.TelephoneUserId and telephoneusers.RankId = menusrank.id;";
            DataSet aDataSet = aGateway.Select(query);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {
                Dictionary<string,string> aDictionary = new Dictionary<string, string>();
                aDictionary.Add("BANumber", dataRow["BANumber"].ToString());
                aDictionary.Add("Name", dataRow["Name"].ToString());
                aDictionary.Add("ComplainType", dataRow["ComplainType"].ToString());
                aDictionary.Add("NewPhoneNumber", dataRow["NewPhoneNumber"].ToString());
                aDictionary.Add("Rank", dataRow["Rank"].ToString());
                aDictionary.Add("Description", dataRow["Description"].ToString());
                aDictionary.Add("ComplainDate", dataRow["ComplainDate"].ToString());
                aDictionary.Add("Id", dataRow["Id"].ToString());
                pendingComplains.Add(aDictionary);
            }

            return pendingComplains;
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


        public List<TelephoneComplain> GetVariousComplainList(string type)
        {

            List<Dictionary<string, string>> pendingComplains = new List<Dictionary<string, string>>();

            string query =
                "select complains.id,telephoneusers.BANumber,telephoneusers.Name,menucomplainType.Value as " +
                "ComplainType,telephoneusers.NewPhoneNumber,menusrank.Value as Rank, " +
                "complains.Description,complains.ComplainDate,complains.Status,complains.ActionTaken,complains.ResolvedBy,complains.ResolvedDate,complains.ActionTaken,complains.Remarks from complains,menucomplainType,telephoneusers," +
                "menusRank where complains.`Status`='"+type+"' and menucomplaintype.Id=complains.MenuComplainTypeId " +
                "and telephoneusers.Id = complains.TelephoneUserId and telephoneusers.RankId = menusrank.id;";
            //TelephoneComplain aTelephoneComplain = new TelephoneComplain();
            List<TelephoneComplain> aTelephoneComplainList = new List<TelephoneComplain>();
            DBGateway aGateway = new DBGateway();
            DataSet aDataSet = aGateway.Select(query);
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {

                TelephoneComplain aTelephoneComplain = new TelephoneComplain();
                aTelephoneComplain.BANumber = dataRow["BANumber"].ToString();
                aTelephoneComplain.Name = dataRow["Name"].ToString();
                aTelephoneComplain.ComplainType = dataRow["ComplainType"].ToString();
                aTelephoneComplain.NewPhoneNumber = dataRow["NewPhoneNumber"].ToString();
                aTelephoneComplain.Rank = dataRow["Rank"].ToString();
                aTelephoneComplain.Description = dataRow["Description"].ToString();
                aTelephoneComplain.ComplainDate = dataRow["ComplainDate"].ToString();
                aTelephoneComplain.ComplainId = dataRow["Id"].ToString();
                aTelephoneComplain.Status = dataRow["Status"].ToString();
                
                aTelephoneComplain.ResolveBy = dataRow["ResolvedBy"].ToString();
                aTelephoneComplain.ResolvedDate = dataRow["ResolvedDate"].ToString();
                aTelephoneComplain.ActionTaken = dataRow["ActionTaken"].ToString();
                aTelephoneComplain.Remarks = dataRow["Remarks"].ToString();
                aTelephoneComplainList.Add(aTelephoneComplain);
            }

            return aTelephoneComplainList;
        }


        public static List<TelephoneComplain> GetResolvedFilteredComplaneList(string sSearch,
            List<TelephoneComplain> complanList, string banumberFilter, string phoneFilter,
            string nameFilter, string rankFilter, string complainFilter, DateTime fromDate, DateTime toDate,
            string resolvedByFilter, DateTime resolverFromdate, DateTime resolverTodate, string actionFilter,
            string remarksFilter)
        {
            List<TelephoneComplain> searchedComplains;


            if ((sSearch == ""))
            {
                searchedComplains = complanList;
            }
            else
            {
                searchedComplains =
                    complanList.Where(c => c.BANumber.ToLower().Contains(sSearch.ToLower())
                                           || c.ComplainType.ToLower().Contains(sSearch.ToLower()) ||
                                           c.Name.ToLower().Contains(sSearch.ToLower()) ||
                                           c.NewPhoneNumber.ToLower().Contains(sSearch.ToLower()) ||
                                           c.ComplainType.ToLower().Contains(sSearch.ToLower()) ||
                                           c.ResolveBy.ToLower().Contains(sSearch.ToLower()) ||
                                           c.ActionTaken.ToLower().Contains(sSearch.ToLower()) ||
                                           c.Remarks.ToLower().Contains(sSearch.ToLower())

                        ).ToList();
            }


            var filteredCompanies = searchedComplains
                .Where(c => (banumberFilter == "" || c.BANumber.Trim().ToLower() == banumberFilter.Trim().ToLower())
                            &&
                            (phoneFilter == "" || c.NewPhoneNumber.ToLower().Contains(phoneFilter))
                            &&
                            (nameFilter == "" || c.Name.ToLower().Contains(nameFilter.ToLower()))
                            &&
                            (rankFilter == "" || c.Rank.ToLower() == rankFilter.ToLower())
                            &&
                            (complainFilter == "" || c.ComplainType.ToLower() == complainFilter.ToLower())
                            &&
                            (fromDate == DateTime.MinValue || fromDate < Convert.ToDateTime(c.ComplainDate))
                            &&
                            (toDate == DateTime.MaxValue || Convert.ToDateTime(c.ComplainDate) < toDate)
                            &&
                            (resolverFromdate == DateTime.MinValue ||
                             resolverFromdate < Convert.ToDateTime(c.ResolvedDate))
                            &&
                            (resolverTodate == DateTime.MaxValue || Convert.ToDateTime(c.ResolvedDate) < resolverTodate)
                            &&
                            (resolvedByFilter == "" || c.ResolveBy.ToLower() == resolvedByFilter.ToLower())
                            &&
                            (actionFilter == "" || c.ActionTaken.ToLower().Contains(actionFilter.ToLower()))
                            &&
                            (remarksFilter == "" || c.Remarks.ToLower().Contains(remarksFilter.ToLower()))

                );
            List<TelephoneComplain> filteredComplaneList = filteredCompanies.ToList();
            return filteredComplaneList;
        }


        




        internal List<Dictionary<string, string>> GetResolvedComplainsByPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

       

    }
}