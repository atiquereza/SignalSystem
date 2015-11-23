using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using SignalSystem.Libs;


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



        public Dictionary<string, string> GetUserInfo(string telephoneNumber)
        {
            Dictionary<string,string> aDictionary = new Dictionary<string, string>();
            string query =
                "select telephoneUsers.Id,telephoneUsers.BANumber,telephoneUsers.Name,telephoneUsers.NewPhoneNumber," +
                "telephoneUsers.Address,menusrank.Value,telephoneUsers.Status from telephoneUsers,menusrank " +
                "where telephoneusers.RankId = menusrank.id and telephoneUsers.Status='Connected' and " +
                "telephoneUsers.NewPhoneNumber=@NewPhoneNumber;";
            Hashtable hashtable = new Hashtable()
            {
                {"NewPhoneNumber",telephoneNumber}
            };

            DataSet aSet = aGateway.Select(query, hashtable);
            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                aDictionary.Add("ID", dataRow["Id"].ToString());
                aDictionary.Add("Name", dataRow["Name"].ToString());
                aDictionary.Add("BANumber", dataRow["BANumber"].ToString());
                aDictionary.Add("NewPhoneNumber", dataRow["NewPhoneNumber"].ToString());
                aDictionary.Add("Address", dataRow["Address"].ToString());
                aDictionary.Add("Rank", dataRow["Value"].ToString());
                
            }
            return aDictionary;
        }

        public bool SubmitNewComplain(string userId, string description, string complainTypeId)
        {
            string nonQuery =
                "INSERT INTO `signalappdb`.`complains` (`Description`, `Status`, " +
                "`TelephoneUserId`, `MenuComplainTypeId`,`ComplainDate`) " +
                "VALUES (@Description, @Status, @UserId, @MenuComplainTypeId,@ComplainDate);";

            Hashtable aHashtable = new Hashtable()
            {
                {"Description",description},
                {"Status", "Pending"},
                {"UserId",userId},
                {"MenuComplainTypeId",complainTypeId},
                {"ComplainDate",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}

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



        internal List<Dictionary<string, string>> GetResolvedComplainsByPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

       

    }
}