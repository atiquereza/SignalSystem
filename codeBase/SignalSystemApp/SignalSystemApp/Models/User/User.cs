using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SignalSystem.Libs;


using System.Web;

namespace SignalSystemApp.Models.User
{
    public class UserInfo
    {
        public string BANumber { set; get; }
        public string Name { set; get; }
        public string Rank { set; get; }
        public string Role { set; get; }
        public string Email { set; get; }


    }
    public class User
    {
        DBGateway aGateway = new DBGateway("SignalSystemConnectionString");
        public bool CreateUser(string baNumber, string name, string rank, string email, string password, string sex, string role, out string outputMessage)
        {

            if (UserExists(name, email,baNumber,out outputMessage))
            {   
                return false;
            }

            string nonQuery = "insert into users (`UserName`,`UserCredential`,`UserRoleId`) Values(@name,@password,@role);";
            Hashtable aHashtable = new Hashtable();
            aHashtable.Add("name", name);
            aHashtable.Add("password", password);
            aHashtable.Add("role", Convert.ToInt32(role));
            aGateway.Insert(nonQuery,aHashtable);
            int id = Convert.ToInt32(aGateway.Select("SELECT LAST_INSERT_ID();").Tables[0].Rows[0][0]);
            nonQuery =
                "INSERT INTO `signalappdb`.`userinfo` (`UserID`, `UserName`, `FullName`, `Email`, `Gender`, `BANumber`,`RankId`) " +
                "VALUES (@id, @name, @name, @email, @sex, @baNumber,@rank);";
            aHashtable = new Hashtable();
            aHashtable.Add("id", id);
            aHashtable.Add("name", name);
            aHashtable.Add("email", email);
            aHashtable.Add("sex", sex);
            aHashtable.Add("baNumber", baNumber);
            aHashtable.Add("rank",rank);
            aGateway.Insert(nonQuery,aHashtable);
            
            
            outputMessage = string.Empty;
            return true;
        }

        private bool UserExists(string name, string email,string baNumber, out string outputMessage)
        {
            outputMessage = string.Empty;
            string query = "select * from userinfo where username=@userName or email=@email or BANumber=@baNumber;";
            Hashtable aHashtable = new Hashtable(){{"userName",name},{"email",email},{"baNumber",baNumber}};
            DataSet aSet = aGateway.Select(query,aHashtable);

            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                if (dataRow["Email"].ToString() == email)
                {
                    outputMessage += "Email Exists !";
                }
                if (dataRow["UserName"].ToString() == name)
                {
                    outputMessage += "Name Exists !";
                }

                if (dataRow["BANumber"].ToString() == baNumber)
                {
                    outputMessage += "BANumber Exists !";
                }
            }

            if (outputMessage.Trim().Length == 0)
            {
                return false;
            }
            return true;
        }

        public List<UserInfo> GetUserInfo()
        {
            string query =
                "select userinfo.UserName,userinfo.Email,userinfo.BANumber,roles.RoleName,menusrank.Value as Rank" +
                " from users,userinfo,roles,menusrank where "
                + "users.UserRoleId = roles.ID and users.ID = userInfo.userID and userinfo.RankId=menusrank.id " +
                ";";
            DataSet aSet = aGateway.Select(query);

            List<UserInfo> userList = new List<UserInfo>();

            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                UserInfo aInfo = new UserInfo();
                aInfo.BANumber = dataRow["BANumber"].ToString();
                aInfo.Name = dataRow["UserName"].ToString();
                aInfo.Email = dataRow["Email"].ToString();
                aInfo.Rank = dataRow["Rank"].ToString();
                aInfo.Role = dataRow["RoleName"].ToString();
                userList.Add(aInfo);

            }
            
            return userList;
        }
    }
}