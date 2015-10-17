using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalSystemApp.Models.User;

namespace SignalSystemApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            User aUser = new User();
            List<UserInfo> allUsers = aUser.GetUserInfo();
            ViewData["UserInfo"] = allUsers;
            return View();
        }

        [HttpPost]
        public ActionResult RegisterNewUser()
        {
            string baNumber = Request["baNumber"].ToString();
            string name = Request["name"].ToString();
            string rank = Request["rank"].ToString();
            string email = Request["email"].ToString();
            string password = Request["password"].ToString();
            string sex = string.Empty;
            if (Request["sex"] != null)
            {
                sex = Request["sex"].ToString();
            }
           
           
            string role = Request["role"].ToString();

            if (baNumber.Trim().Length == 0 || name.Trim().Length == 0 || rank.Trim().Length == 0 || email.Trim().Length == 0
                || password.Trim().Length == 0 || sex.Trim().Length == 0 || role.Trim().Length == 0)
            {
                ViewData["Message"] = "Error! Fields can not left blank";
                return View("Registration");
            }


            User aUser = new User();
            string outputMessage = string.Empty;
            bool success = aUser.CreateUser(baNumber,name,rank,email,password,sex,role,out outputMessage);

            if (!success)
            {
                ViewData["Message"] = "Error! " + outputMessage;
                return View("Registration");
            }


            ViewData["Message"] = "User has been added successfully";
            return View("Registration");
        }
        public ActionResult Registration()
        {
            return View();
        }
    }
}