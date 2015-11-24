using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SignalSystem.Libs;

namespace SignalSystemApp.Models.Application
{
    public class ApplicationConfiguration
    {
        DBGateway aGateway = new DBGateway("SignalSystemConnectionString");
        public string AddNewComplainType(string complainType)
        {
            string message = string.Empty;
            if (AlreadyHasThisComplainType(complainType))
            {
                return "Error! Complain Type Already Exists.";
            }

            //string queryString = "select * from ";
            string insertString = "insert into menucomplaintype (value,name) values(@value,@name)";
            Hashtable aHashtable = new Hashtable()
            {
                {"value",complainType},
                {"name","Complain"}
            };
            int lastInsertId = aGateway.Insert(insertString, aHashtable);
            return "Data Successfully Added";
        }

        private bool AlreadyHasThisComplainType(string complainType)
        {
            string query = "select * from menucomplaintype where value=@value";
            Hashtable aHashtable = new Hashtable()
            {
                {"value",complainType}
            };

            DataSet aSet = aGateway.Select(query, aHashtable);

            if (aSet.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;

        }
    }
}