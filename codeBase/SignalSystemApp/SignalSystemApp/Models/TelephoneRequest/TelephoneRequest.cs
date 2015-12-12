using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    }
}