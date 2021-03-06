﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Security;
using System.DirectoryServices.AccountManagement;
using SignalSystem.Libs;

namespace SignalSystem.Libs
{

    public class Authentication
    {

        public static bool Authenticate(string userName, string userPassword, HttpSessionStateBase Session)
        {

            DBAuthentication authentication = new DBAuthentication(userName, userPassword); 

            bool result = authentication.IsValid();

            if (result == false)
            {
                return result;
            }
            

            DBGateway aDbGateway = new DBGateway();
            Hashtable conditionTable = new Hashtable();
            string query = "select * from users,roles where users.UserName='" + userName + "' and users.UserCredential='" + userPassword + "' and  users.UserRoleId = roles.ID";
            conditionTable["UserName"] = userName;
            DataSet aDataSet = aDbGateway.Select(query, conditionTable);


            aDataSet.Tables[0].Columns.Add("LogInValue");
            aDataSet.Tables[0].Rows[0]["LogInValue"] = userPassword;

            List<string> cols = new List<string>();

            Dictionary<string,string> userData = new Dictionary<string, string>();
            foreach (DataColumn column in aDataSet.Tables[0].Columns)
            {
                cols.Add(column.ColumnName);
            }

            foreach (DataRow row in aDataSet.Tables[0].Rows)
            {
                foreach (string col in cols)
                {
                    userData.Add(col,row[col].ToString());
                }
            }


            SessionHandler.SetSessionData(userData, Session);

            return true;

        }



        public static bool Authenticate(Dictionary<string, string> sessionData, HttpSessionStateBase Session)
        {
            string code = EncrDecrAction.Encrypt(
                           EncrDecrAction.Encrypt(EncrDecrAction.Encrypt(Session["UserId"].ToString(), true), true)
                         + EncrDecrAction.Encrypt(EncrDecrAction.Encrypt(Session["UserRoleId"].ToString(), true), true)
                         + EncrDecrAction.Encrypt(EncrDecrAction.Encrypt(Session["UserName"].ToString(), true), true)
                         + EncrDecrAction.Encrypt(EncrDecrAction.Encrypt(Session["RoleName"].ToString(), true), true)
                         + EncrDecrAction.Encrypt(EncrDecrAction.Encrypt(Session["ParentRoleName"].ToString(), true), true), true);

            


            if (code == Session["SRES"].ToString())
            {
                UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

                var routeValueDictionary = urlHelper.RequestContext.RouteData.Values;
                string controller = routeValueDictionary["controller"].ToString();
                string action = routeValueDictionary["action"].ToString();



                string query = "select * from appviews where LOWER(Controller) = LOWER(@Controller) and LOWER(Action) = LOWER(@Action) and " + sessionData["RoleName"] + "= 1";
                Hashtable conditionTable = new Hashtable();
                conditionTable["Controller"] = controller;
                conditionTable["Action"] = action;
                DBGateway aDbGateway = new DBGateway();
                DataSet aDataSet = aDbGateway.Select(query, conditionTable);
                if (aDataSet.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }

            return false;

        }


        public static bool ApiAuthenticate(Dictionary<string, string> sessionData, HttpSessionStateBase Session, System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string code = EncrDecrAction.Encrypt(
                           EncrDecrAction.Encrypt(EncrDecrAction.Encrypt(Session["UserId"].ToString(), true), true)
                         + EncrDecrAction.Encrypt(EncrDecrAction.Encrypt(Session["UserRoleId"].ToString(), true), true)
                         + EncrDecrAction.Encrypt(EncrDecrAction.Encrypt(Session["UserName"].ToString(), true), true)
                         + EncrDecrAction.Encrypt(EncrDecrAction.Encrypt(Session["RoleName"].ToString(), true), true)
                         + EncrDecrAction.Encrypt(EncrDecrAction.Encrypt(Session["ParentRoleName"].ToString(), true), true), true);




            if (code == Session["SRES"].ToString())
            {
                UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

                var routeValueDictionary = urlHelper.RequestContext.RouteData.Values;
                string controller = routeValueDictionary["controller"].ToString();
                string action = actionContext.Request.Method.ToString();

                int argument = actionContext.Request.RequestUri.Segments.Count() - 3;



                string query = "select * from appviews where LOWER(Controller) = LOWER(@Controller) and LOWER(Action) = LOWER(@Action) and " + sessionData["RoleName"] + "= 1 and Argument=@Argument and ControllerType='api'";
                Hashtable conditionTable = new Hashtable();
                conditionTable["Controller"] = controller;
                conditionTable["Action"] = action;
                conditionTable["Argument"] = argument;
                DBGateway aDbGateway = new DBGateway();
                DataSet aDataSet = aDbGateway.Select(query, conditionTable);
                if (aDataSet.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }

            return false;

        }
    }


}


public struct Credentials
{
    public string Username;
    public string UserPassword;
}

class DBAuthentication
{
    public Credentials Credentials;

    DBGateway aGateway = new DBGateway();
    public DBAuthentication(string Username, string userPassword)
    {
        Credentials.Username = Username;
        Credentials.UserPassword = userPassword;

    }

    public DBAuthentication(string Username)
    {
        Credentials.Username = Username;
    }

    public bool IsValid()
    {
        string query = "select * from users,roles where users.UserName=@username and users.UserCredential=@userCredential and  users.UserRoleId = roles.ID";
        Hashtable aHashtable = new Hashtable();
        aHashtable["username"] = Credentials.Username;
        aHashtable["userCredential"] = Credentials.UserPassword;


        DataSet aDataSet = aGateway.Select(query, aHashtable);

        if (aDataSet.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        return false;
    }

    public Dictionary<string, string> GetUserInfo(int userId)
    {

        Dictionary<string, string> aDictionary = new Dictionary<string, string>();

        string query = "select * from userinfo where userinfo.UserID='"+userId+"';";
        DataSet aSet = aGateway.Select(query);
        List<string> cols = new List<string>();

        foreach (DataColumn column in aSet.Tables[0].Columns)
        {
            cols.Add(column.ColumnName);
        }

        foreach (DataRow dataRow in aSet.Tables[0].Rows)
        {
            foreach (string col in cols)
            {
                aDictionary.Add(col,dataRow[col].ToString());
            }
        }


        return aDictionary;
    }
}





