using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Excel;
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
                { "RequestDate",aInfo.RequestDate.ToString("yyyy-MM-dd")+" " +DateTime.Now.ToString("HH:mm:ss")}

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
                { "RequestDate",aInfo.RequestDate.ToString("yyyy-MM-dd")+" " +DateTime.Now.ToString("HH:mm:ss")},
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
                { "RequestDate",aInfo.RequestDate.ToString("yyyy-MM-dd") +" " +DateTime.Now.ToString("HH:mm:ss")}

            };
            aGateway.Insert(insertString, aHashtable);
            //string updateString = "UPDATE `signalappdb`.`allphoneinfo` SET `ServiceStatus`='Terminated' WHERE  `ID`='" + aInfo.AllPhoneInfoID + "';";
            //aGateway.Update(updateString);

            return "Termination Request Successfully Submitted";
        }

        public List<string[]> GetPendingRequest(out int totalRecords, out int filteredRecord, string baNumber, string name, 
            string letterNumber, string fromDate, string toDate, string phoneNumber,string requestType,int start, int length)
        {


            string query = "select pendingrequest.ID as PendingRequestID,pendingrequest.LetterNo,pendingrequest.AddressType," +
                           "menurequesttype.Value as RequestType,allphoneinfo.PhoneNumber,pendingrequest.RequestDate,pendingrequest.`Comment`," +
                           "phoneuserpersonalinfo.BANumber,phoneuserpersonalinfo.FullName,menusrank.Value as Rank," +
                           "phoneuserpersonalinfo.OfficeAddress,phoneuserpersonalinfo.PresentAddress,pendingrequest.NewAddressForShifting," +
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
            if (phoneNumber.Trim().Length != 0)
            {
                query += " allphoneinfo.PhoneNumber like '%" + phoneNumber + "%' and ";
            }
            if (requestType.Trim().Length != 0)
            {
                query += " menurequesttype.Value like '%" + requestType + "%' and ";
            }
            if (fromDate.Trim().Length != 0)
            {
                if (toDate.Trim().Length != 0)
                {
                    query += " pendingrequest.RequestDate between '" + Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + Convert.ToDateTime(toDate).ToString("yyyy-MM-dd") + " 23:59:59" + "' and";
                }
                else
                {
                    query += " pendingrequest.RequestDate between '" + Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "' and";
                    
                }
            }

            if (baNumber.Trim().Length == 0 && name.Trim().Length == 0 && letterNumber.Trim().Length == 0 &&
                fromDate.Trim().Length == 0 && toDate.Trim().Length == 0 && phoneNumber.Trim().Length == 0 && requestType.Trim().Length== 0)
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
                string address = string.Empty;
                if (dataRow["AddressType"].ToString().ToLower() == "office")
                {
                    address = dataRow["OfficeAddress"].ToString();
                }
                else
                {
                    address = dataRow["PresentAddress"].ToString();
                }

                if (dataRow["RequestType"].ToString().ToLower() == "shifting")
                {
                    address = "Phone Shifting Form " + address + " To: " + dataRow["NewAddressForShifting"].ToString();
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
                    Convert.ToDateTime(dataRow["RequestDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
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
                "menurequesttype.Value as RequestType,pendingrequest.LetterNo,allphoneinfo.PhoneNumber,allphoneinfo.ID as PhoneId," +
                "phoneuserpersonalinfo.ID as PersonId," +
                "pendingrequest.RequestDate,pendingrequest.`Comment`,pendingrequest.NewAddressForShifting," +
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
                resolveDate = newDate.ToString(toFormat).ToString() + " "+DateTime.Now.ToString("HH:mm:ss");
                aData = new Hashtable()
                {
                    {"ID" , dataRow["PendingRequestID"].ToString()},
                    {"FullName" , dataRow["FullName"].ToString()},
                    {"BANumber" , dataRow["BANumber"].ToString()},
                    {"AddressType" , dataRow["AddressType"].ToString()},
                    {"Address" , address}, 
                    {"Rank" , dataRow["Rank"].ToString()},
                    {"RequestDate" , Convert.ToDateTime(dataRow["RequestDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")},
                    {"LetterNo",dataRow["LetterNo"].ToString()},
                    {"RequestType",dataRow["RequestType"].ToString()},
                    {"Comment" , dataRow["Comment"].ToString()},
                    {"ConnectionType" , dataRow["ConnectionType"].ToString()},
                    {"ResolveDate" ,resolveDate},
                    {"ResolveBy" , resolvedBy},
                    {"ActionTaken" , actionTaken},
                    {"PhoneNumber",dataRow["PhoneNumber"].ToString()},
                    {"AllPhoneIfoId",dataRow["PhoneId"].ToString()},
                    {"PersonId",dataRow["PersonId"].ToString()},
                    {"PresentAddress",dataRow["PresentAddress"].ToString()},
                    {"OfficeAddress",dataRow["OfficeAddress"].ToString()},
                    {"NewAddressForShifting",dataRow["NewAddressForShifting"].ToString()},

                };
               
            }

            if (aData.Count == 0)
            {
                return "Error!";
            }

            #region new Connection
            if (aData["RequestType"].ToString().ToLower() == "new connection")
            {
                string updateActivePhoneList = "INSERT INTO allactivephoneinfo (`PhoneUserPersonalInfoId`, `AllPhoneInfoID`, " +
                                               "`PhoneUsedFor`, `HomeAddress`, `OfficeAddress`, " +
                                               "`RequestDate`, `LetterNo`, `ConnectDate`) " +
                                               "VALUES (@PhoneUserPersonalInfoId, @AllPhoneInfoID, " +
                                               "@PhoneUsedFor, @HomeAddress, @OfficeAddress, " +
                                               "@RequestDate, @LetterNo, @ConnectDate);";

                Hashtable aHashtable = new Hashtable();
                aHashtable.Add("PhoneUserPersonalInfoId", aData["PersonId"].ToString());
                aHashtable.Add("AllPhoneInfoID", aData["AllPhoneIfoId"].ToString());
                aHashtable.Add("PhoneUsedFor", aData["AddressType"].ToString());
                aHashtable.Add("HomeAddress", aData["PresentAddress"].ToString());
                aHashtable.Add("OfficeAddress", aData["OfficeAddress"].ToString());
                aHashtable.Add("RequestDate", aData["RequestDate"].ToString());
                aHashtable.Add("LetterNo", aData["LetterNo"].ToString());
                aHashtable.Add("ConnectDate", resolveDate);
                aGateway.Insert(updateActivePhoneList, aHashtable);


            }

            #endregion
            #region Termination
            if (aData["RequestType"].ToString().ToLower() == "termination")
            {
                string deleteTerminationString = "delete from allactivephoneinfo where AllPhoneInfoID='" + aData["AllPhoneIfoId"].ToString() + "';";
                aGateway.Delete(deleteTerminationString);
                string terminateApplyInAllPhoneInfo = "UPDATE allphoneinfo SET ServiceStatus='Terminated' WHERE  ID='" + aData["AllPhoneIfoId"] + "';";
                aGateway.Update(terminateApplyInAllPhoneInfo);
            }
            #endregion

            #region Shifting
            if (aData["RequestType"].ToString().ToLower() == "shifting")
            {
                string updateStringActivePhoneTable = "UPDATE allactivephoneinfo ";
                string updateStringPersonInfoTable = "UPDATE phoneuserpersonalinfo ";
                if (aData["AddressType"].ToString().ToLower() == "home")
                {
                    updateStringActivePhoneTable += "SET `HomeAddress`='" + aData["NewAddressForShifting"].ToString() + "'";
                    updateStringPersonInfoTable += "SET `PresentAddress`='" + aData["NewAddressForShifting"].ToString() + "'";

                }
                else
                {
                    updateStringActivePhoneTable += "SET `OfficeAddress`='" + aData["NewAddressForShifting"].ToString() + "'";
                    updateStringPersonInfoTable += "SET `OfficeAddress`='" + aData["NewAddressForShifting"].ToString() + "'";

                }
                updateStringActivePhoneTable += "WHERE  AllPhoneInfoID='" + aData["AllPhoneIfoId"] + "';";
                updateStringPersonInfoTable += "WHERE  ID='" + aData["PersonId"] + "';";

                aGateway.Update(updateStringActivePhoneTable);
                aGateway.Update(updateStringPersonInfoTable);
            }
            #endregion

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

        public List<string[]> GetResolveRequest(out int totalRecords, out int filteredRecord, string baNumber, string name, string letterNumber, string fromDate, string toDate, 
            string phoneNumber, string requestType, int start, int length)
        {
            List<string[]> aList = new List<string[]>();
            string query = "select * from resolvedrequest where ";
            
            if (baNumber.Trim().Length != 0)
            {
                query += " BANumber like '%" + baNumber + "%' and ";
            }
            if (name.Trim().Length != 0)
            {
                query += " FullName like '%" + name + "%' and ";
            }
            if (letterNumber.Trim().Length != 0)
            {
                query += " LetterNo like '%" + letterNumber + "%' and ";
            }
            if (phoneNumber.Trim().Length != 0)
            {
                query += " PhoneNumber like '%" + phoneNumber + "%' and ";
            }
            if (requestType.Trim().Length != 0)
            {
                query += " RequestType like '%" + requestType + "%' and ";
            }

            if (fromDate.Trim().Length != 0)
            {
                if (toDate.Trim().Length != 0)
                {
                    query += " resolvedrequest.RequestDate between '" + Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + Convert.ToDateTime(toDate).ToString("yyyy-MM-dd") + " 23:59:59" + "' and";
                }
                else
                {
                    query += " resolvedrequest.RequestDate between '" + Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59" + "' and";

                }
            }

            if (baNumber.Trim().Length == 0 && name.Trim().Length == 0 && letterNumber.Trim().Length == 0 &&
                fromDate.Trim().Length == 0 && toDate.Trim().Length == 0 && phoneNumber.Trim().Length == 0 && requestType.Trim().Length == 0)
            {
                query = query.Trim().Substring(0, query.Trim().LastIndexOf("where"));
            }
            else
            {
                query = query.Trim().Substring(0, query.Trim().LastIndexOf("and"));

            }
            query += " order by RequestDate DESC limit " + start + "," + start + length + ";";


            DataSet aSet = aGateway.Select(query);
            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                aList.Add(new string[]
                {
                    dataRow["BANumber"].ToString(),
                    dataRow["FullName"].ToString(),
                    dataRow["Rank"].ToString(),
                    dataRow["PhoneNumber"].ToString(),
                    Convert.ToDateTime(dataRow["RequestDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                    Convert.ToDateTime(dataRow["ResolveDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                  
                    dataRow["AddressType"].ToString(),
                    dataRow["Address"].ToString(),
                    dataRow["LetterNo"].ToString(),
                    //dataRow["ConnectionType"].ToString(),
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