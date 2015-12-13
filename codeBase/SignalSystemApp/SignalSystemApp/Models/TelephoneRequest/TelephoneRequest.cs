using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Policy;
using System.Web;
using SignalSystem.Libs;

namespace SignalSystemApp.Models.TelephoneRequest
{
    public class TelephoneRequestInfo
    {
        public int Id { set; get; }
        public int MenuRequestTypeId { set; get; }
        public int PhoneUserPersonalInfoID { set; get; }
        public int AllPhoneInfoID { set; get; }
        public string LetterNo { set; get; }
        public string AddressType { set; get; }
        public string Address { set; get; }
        public string Comment { set; get; }
        public DateTime RequestDate { set; get; }
        
    }
    public class TelephoneRequest
    {
        Dictionary<string,string> menuRequestType = new Dictionary<string, string>();
        DBGateway aGateway = new DBGateway("SignalSystemConnectionString");
        public TelephoneRequest()
        {
            DataSet aDataSet = aGateway.Select("select * from menurequesttype;");
            foreach (DataRow dataRow in aDataSet.Tables[0].Rows)
            {
                if (!menuRequestType.ContainsKey(dataRow["Value"].ToString()))
                {
                    menuRequestType.Add(dataRow["Value"].ToString(), dataRow["ID"].ToString());  
                }
            }
        }
        public string AddNewConnectionRequest(TelephoneRequestInfo aInfo)
        {
            if (!menuRequestType.Keys.ToList().Exists(i => i.ToLower() == "new connection"))
            {
                return "Error! Request Type Not Found in Database. Please Contact with Application Administrator";
            }

            int index = menuRequestType.Keys.ToList().FindIndex(i => i.ToLower() == "new connection");
            aInfo.MenuRequestTypeId = Convert.ToInt32(menuRequestType[menuRequestType.Keys.ToList()[index]]);


            string insertString = "INSERT INTO `signalappdb`.`pendingrequest` (`MenuRequestTypeID`, `PhoneUserPersonalInfoID`, " +
                                  "`AllPhoneInfoID`, `LetterNo`, `AddressType`, `Comment`,`RequestDate`) VALUES (@MenuRequestTypeID, " +
                                  "@PhoneUserPersonalInfoID, " +
                                  "@AllPhoneInfoID, @LetterNo, @AddressType, @Comment,@RequestDate);";

            Hashtable aHashtable = new Hashtable()
            {
                {"MenuRequestTypeID",aInfo.MenuRequestTypeId},
                {"AllPhoneInfoID",aInfo.AllPhoneInfoID},
                {"PhoneUserPersonalInfoID",aInfo.PhoneUserPersonalInfoID} ,
                {"LetterNo",aInfo.LetterNo},
                {"AddressType",aInfo.AddressType},
                {"Comment",aInfo.Comment},
                { "RequestDate",aInfo.RequestDate.ToString("yyyy-MM-dd")}

            };
            aGateway.Insert(insertString,aHashtable);
            string updateString = "UPDATE `signalappdb`.`allphoneinfo` SET `ServiceStatus`='Active' WHERE  `ID`='"+aInfo.AllPhoneInfoID+"';";
            aGateway.Update(updateString);

            return "New Connection Request Successfully Submitted";

        }

        public string AddShiftingRequest(TelephoneRequestInfo aInfo)
        {
            if (!menuRequestType.Keys.ToList().Exists(i => i.ToLower() == "shifting"))
            {
                return "Error! Request Type Not Found in Database. Please Contact with Application Administrator";
            }

            int index = menuRequestType.Keys.ToList().FindIndex(i => i.ToLower() == "shifting");
            aInfo.MenuRequestTypeId = Convert.ToInt32(menuRequestType[menuRequestType.Keys.ToList()[index]]);

            string insertString = "INSERT INTO `signalappdb`.`pendingrequest` (`MenuRequestTypeID`, `PhoneUserPersonalInfoID`, " +
                              "`AllPhoneInfoID`, `LetterNo`, `AddressType`, `Comment`,`RequestDate`,`NewAddressForShifting`) VALUES (@MenuRequestTypeID, " +
                              "@PhoneUserPersonalInfoID, " +
                              "@AllPhoneInfoID, @LetterNo, @AddressType, @Comment,@RequestDate,@NewAddressForShifting);";

            Hashtable aHashtable = new Hashtable()
            {
                {"MenuRequestTypeID",aInfo.MenuRequestTypeId},
                {"AllPhoneInfoID",aInfo.AllPhoneInfoID},
                {"PhoneUserPersonalInfoID",aInfo.PhoneUserPersonalInfoID} ,
                {"LetterNo",aInfo.LetterNo},
                {"AddressType",aInfo.AddressType},
                {"Comment",aInfo.Comment},
                { "RequestDate",aInfo.RequestDate.ToString("yyyy-MM-dd")},
                {"NewAddressForShifting",aInfo.Address}

            };
            aGateway.Insert(insertString, aHashtable);

            return "Shifing Request Successfully Submitted";
        }

        public string AddTerminationRequest(TelephoneRequestInfo aInfo)
        {
            if (!menuRequestType.Keys.ToList().Exists(i => i.ToLower() == "termination"))
            {
                return "Error! Request Type Not Found in Database. Please Contact with Application Administrator";
            }

            int index = menuRequestType.Keys.ToList().FindIndex(i => i.ToLower() == "termination");
            aInfo.MenuRequestTypeId = Convert.ToInt32(menuRequestType[menuRequestType.Keys.ToList()[index]]);


            string insertString = "INSERT INTO `signalappdb`.`pendingrequest` (`MenuRequestTypeID`, `PhoneUserPersonalInfoID`, " +
                                  "`AllPhoneInfoID`, `LetterNo`, `AddressType`, `Comment`,`RequestDate`) VALUES (@MenuRequestTypeID, " +
                                  "@PhoneUserPersonalInfoID, " +
                                  "@AllPhoneInfoID, @LetterNo, @AddressType, @Comment,@RequestDate);";

            Hashtable aHashtable = new Hashtable()
            {
                {"MenuRequestTypeID",aInfo.MenuRequestTypeId},
                {"AllPhoneInfoID",aInfo.AllPhoneInfoID},
                {"PhoneUserPersonalInfoID",aInfo.PhoneUserPersonalInfoID} ,
                {"LetterNo",aInfo.LetterNo},
                {"AddressType",aInfo.AddressType},
                {"Comment",aInfo.Comment},
                { "RequestDate",aInfo.RequestDate.ToString("yyyy-MM-dd")}

            };
            aGateway.Insert(insertString, aHashtable);
            //string updateString = "UPDATE `signalappdb`.`allphoneinfo` SET `ServiceStatus`='Terminated' WHERE  `ID`='" + aInfo.AllPhoneInfoID + "';";
            //aGateway.Update(updateString);

            return "Termination Request Successfully Submitted";
        }

        public List<string[]> GetPendingRequest(out int totalRecords, out int filteredRecord, string baNumber, string name, 
            string letterNumber, string fromDate, string toDate, int start, int length)
        {


            string query = "select pendingrequest.ID as PendingRequestID,pendingrequest.LetterNo,pendingrequest.AddressType," +
                           "menurequesttype.Value as RequestType,allphoneinfo.PhoneNumber,pendingrequest.RequestDate,pendingrequest.`Comment`," +
                           "phoneuserpersonalinfo.BANumber,phoneuserpersonalinfo.FullName,menusrank.Value as Rank," +
                           "phoneuserpersonalinfo.OfficeAddress,phoneuserpersonalinfo.PresentAddress," +
                           "menuconnectiontype.Value as ConnectionType from pendingrequest left join menurequesttype on " +
                           "pendingrequest.MenuRequestTypeID=menurequesttype.ID left join phoneuserpersonalinfo on " +
                           "phoneuserpersonalinfo.ID=pendingrequest.PhoneUserPersonalInfoID left join allphoneinfo on " +
                           "allphoneinfo.ID=pendingrequest.AllPhoneInfoID left join menusrank on " +
                           "menusrank.id=phoneuserpersonalinfo.RankId left join menuconnectiontype " +
                           "on menuconnectiontype.ID=allphoneinfo.ConnectionTypeID where ";


            if (baNumber.Trim().Length != 0)
            {
                query += " phoneuserpersonalinfo.BANumber like '%"+baNumber+"%' and ";
            }
            if (name.Trim().Length != 0)
            {
                query += " phoneuserpersonalinfo.FullName like '%" + name + "%' and ";
            }
            if (letterNumber.Trim().Length != 0)
            {
                query += " pendingrequest.LetterNo like '%" + letterNumber + "%' and ";
            }
            if (fromDate.Trim().Length != 0)
            {
                if (toDate.Trim().Length != 0)
                {
                    query += " pendingrequest.RequestDate between '"+Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd")+"' and '"+Convert.ToDateTime(toDate).ToString("yyyy-MM-dd")+"' and";
                }
                else
                {
                    query += " pendingrequest.RequestDate between '" + Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and";
                    
                }
            }

            if (baNumber.Trim().Length == 0 && name.Trim().Length == 0 && letterNumber.Trim().Length == 0 &&
                fromDate.Trim().Length == 0 && toDate.Trim().Length == 0)
            {
                query = query.Trim().Substring(0, query.Trim().LastIndexOf("where"));
            }
            else
            {
                query = query.Trim().Substring(0, query.Trim().LastIndexOf("and"));

            }
            query += " limit " + start + "," + start + length + ";";
                   

            List<string[]> aList = new List<string[]>();

            DataSet aSet = aGateway.Select(query);

            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                string address = string.Empty;
                if (dataRow["AddressType"].ToString().ToLower() == "office")
                {
                    address = dataRow["OfficeAddress"].ToString();
                }
                else
                {
                    address = dataRow["PresentAddress"].ToString();
                }
                string[] aReqData = new  string[]
                {
                    dataRow["PendingRequestID"].ToString(),
                    dataRow["BANumber"].ToString(),
                    dataRow["FullName"].ToString(),
                    dataRow["Rank"].ToString(),
                    dataRow["LetterNo"].ToString(),
                    dataRow["PhoneNumber"].ToString(),
                    dataRow["AddressType"].ToString(),
                    address,
                    dataRow["RequestType"].ToString(),
                    dataRow["RequestDate"].ToString(),
                    dataRow["Comment"].ToString(),
                    ""
                };


                aList.Add(aReqData);
            }

            query = "select count(*) as TotalData from pendingrequest"; 
            aSet = aGateway.Select(query);
            totalRecords = Convert.ToInt32(aSet.Tables[0].Rows[0]["TotalData"].ToString());
            
            filteredRecord = aList.Count;
            return aList;
        }

        public string ResolveRequest(string requestId, string resolveDate, string resolvedBy, string actionTaken)
        {

            string query =
                " select pendingrequest.ID as PendingRequestID,phoneuserpersonalinfo.FullName,phoneuserpersonalinfo.BANumber," +
                "pendingrequest.AddressType,phoneuserpersonalinfo.OfficeAddress,phoneuserpersonalinfo.PresentAddress,menusrank.Value as Rank," +
                "menurequesttype.Value as RequestType,pendingrequest.LetterNo,allphoneinfo.PhoneNumber," + 
                "pendingrequest.RequestDate,pendingrequest.`Comment`," +
                "menuconnectiontype.Value as ConnectionType from pendingrequest left join menurequesttype " +
                "on pendingrequest.MenuRequestTypeID=menurequesttype.ID left join phoneuserpersonalinfo " +
                "on phoneuserpersonalinfo.ID=pendingrequest.PhoneUserPersonalInfoID left join allphoneinfo " +
                "on allphoneinfo.ID=pendingrequest.AllPhoneInfoID left join menusrank " +
                "on menusrank.id=phoneuserpersonalinfo.RankId left join menuconnectiontype " +
                "on menuconnectiontype.ID=allphoneinfo.ConnectionTypeID where pendingrequest.ID='" + requestId + "';";

            Hashtable aData = new Hashtable();
            DataSet aSet = aGateway.Select(query);
            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                string address = string.Empty;

                if (dataRow["AddressType"].ToString().ToLower() == "office")
                {
                    address = dataRow["OfficeAddress"].ToString();
                }
                else
                {
                    address = dataRow["PresentAddress"].ToString();
                }

                string fromFormat = "dd/MM/yyyy";
                string toFormat = "yyyy-MM-dd";

                DateTime newDate = DateTime.ParseExact(resolveDate, fromFormat, null); 
                resolveDate = newDate.ToString(toFormat).ToString();
                aData = new Hashtable()
                {
                    {"ID" , dataRow["PendingRequestID"].ToString()},
                    {"FullName" , dataRow["FullName"].ToString()},
                    {"BANumber" , dataRow["BANumber"].ToString()},
                    {"AddressType" , dataRow["AddressType"].ToString()},
                    {"Address" , address}, 
                    {"Rank" , dataRow["Rank"].ToString()},
                    {"RequestDate" , Convert.ToDateTime(dataRow["RequestDate"].ToString()).ToString("yyyy-MM-dd")},
                    {"LetterNo",dataRow["LetterNo"].ToString()},
                    {"RequestType",dataRow["RequestType"].ToString()},
                    {"Comment" , dataRow["Comment"].ToString()},
                    {"ConnectionType" , dataRow["ConnectionType"].ToString()},
                    {"ResolveDate" ,resolveDate},
                    {"ResolveBy" , resolvedBy},
                    {"ActionTaken" , actionTaken},
                    {"PhoneNumber",dataRow["PhoneNumber"].ToString()}
                };
               
            }
            string insertString = "INSERT INTO resolvedrequest (`FullName`, `BANumber`, `Address`, `AddressType`, " +
                           "`Rank`,`RequestType`, `LetterNo`, `RequestDate`, `ResolveDate`, `PhoneNumber`,  " +
                           "`ResolveBy`, `ActionTaken`,`ConnectionType`) VALUES (  @FullName, @BANumber, @Address, @AddressType, " +
                           "@Rank,@RequestType, @LetterNo, @RequestDate, @ResolveDate, @PhoneNumber,  " +
                           "@ResolveBy, @ActionTaken,@ConnectionType);";
            aGateway.Insert(insertString, aData);

            string deleteString = "delete from pendingrequest where ID='" + aData["ID"] + "'";
            aGateway.Delete(deleteString);

            return "Solved";

        }

        public List<string[]> GetResolveRequest(out int totalRecords, out int filteredRecord, string baNumber, string name, string letterNumber, string fromDate, string toDate, int start, int length)
        {
            List<string[]> aList = new List<string[]>();
            string query = "select * from resolvedrequest limit " + start + "," + start + length + ";";

            DataSet aSet = aGateway.Select(query);
            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                aList.Add(new string[]
                {
                    dataRow["BANumber"].ToString(),
                    dataRow["FullName"].ToString(),
                    dataRow["Rank"].ToString(),
                    dataRow["PhoneNumber"].ToString(),
                    dataRow["RequestDate"].ToString(),
                    dataRow["ResolveDate"].ToString(),
                    dataRow["AddressType"].ToString(),
                    dataRow["Address"].ToString(),
                    dataRow["LetterNo"].ToString(),
                    dataRow["ConnectionType"].ToString(),
                    dataRow["RequestType"].ToString(),
                    dataRow["ResolveBy"].ToString(),
                    dataRow["ActionTaken"].ToString(),
                   
                });
            }

            query = "select Count(*) as TotalData from resolvedrequest;";
            
            totalRecords = Convert.ToInt32(aGateway.Select(query).Tables[0].Rows[0]["TotalData"].ToString());
            filteredRecord = aList.Count;
            return aList;
        }
    }
}