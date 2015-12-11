using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalSystem.Libs;
using System.Data;
using SignalSystem.Libs;

namespace SignalSystemApp.Views.Classes
{
    public class FrameworkItems
    {
        public static List<Dictionary<string,string>> GetRanks()
        {
            List<Dictionary<string, string>> ranks = new List<Dictionary<string, string>>();
            DBGateway gateway = new DBGateway("SignalSystemConnectionString");
            string query = "select * from menusrank;";

            DataSet aSet = gateway.Select(query);

            foreach(DataRow aRow in aSet.Tables[0].Rows)
            {
                Dictionary<string, string> aDictionary = new Dictionary<string, string>();
                aDictionary.Add("ID", aRow["id"].ToString());
                aDictionary.Add("Value", aRow["Value"].ToString());
                ranks.Add(aDictionary);

            }

            return ranks;
        }

        public static List<Dictionary<string, string>> ConnectionType()
        {
            List<Dictionary<string, string>> connectionTypes = new List<Dictionary<string, string>>();
            DBGateway gateway = new DBGateway("SignalSystemConnectionString");
            string query = "select * from menuconnectiontype;";

            DataSet aSet = gateway.Select(query);

            foreach (DataRow aRow in aSet.Tables[0].Rows)
            {
                Dictionary<string, string> aDictionary = new Dictionary<string, string>();
                aDictionary.Add("ID", aRow["id"].ToString());
                aDictionary.Add("Value", aRow["Value"].ToString());
                connectionTypes.Add(aDictionary);

            }

            return connectionTypes;
        }

        public static List<UserRole> GetRoles(HttpSessionStateBase session)
        {
           UserRoleManager aManager = new SignalSystem.Libs.UserRoleManager();
           int roleId = Convert.ToInt32(SessionHandler.GetSessionData(session)["UserRoleId"]);
           List<UserRole> roles = aManager.GetRoleLevels(roleId);

            return roles;
        }

        public static List<Dictionary<string, string>> GetComplainTypes()
        {
            List<Dictionary<string, string>> ranks = new List<Dictionary<string, string>>();
            DBGateway gateway = new DBGateway("SignalSystemConnectionString");
            string query = "select * from menucomplaintype;";

            DataSet aSet = gateway.Select(query);

            foreach (DataRow aRow in aSet.Tables[0].Rows)
            {
                Dictionary<string, string> aDictionary = new Dictionary<string, string>();
                aDictionary.Add("ID", aRow["Id"].ToString());
                aDictionary.Add("Value", aRow["Value"].ToString());
                ranks.Add(aDictionary);

            }

            return ranks;
        }
    
    }
}