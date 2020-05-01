using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEA_Application.Controllers
{
    public class Supervisor_DashboardController : Controller
    {
        // GET: Supervisor_Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View("BlankPage");
        }

        public ActionResult EmployeeAttendance()
        {
            return View();
        }
    }
}