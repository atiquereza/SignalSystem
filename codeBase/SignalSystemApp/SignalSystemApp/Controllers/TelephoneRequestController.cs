using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalSystemApp.Controllers
{
    public class TelephoneRequestController : Controller
    {   
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PendingRequest()
        {
            return View();
        }
        public ActionResult ResolveRequest()
        {
            return View();
        }
    }
}