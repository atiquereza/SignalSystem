using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalSystemApp.Controllers
{
    public class TelephoneController : Controller
    {
        // GET: Telephone
        public ActionResult AddTelephone()
        {
            return View();
        }

        public ActionResult PendingComplains()
        {
            return View();
        }

        public ActionResult ResolvedComplains()
        {
            return View();
        }
    }
}