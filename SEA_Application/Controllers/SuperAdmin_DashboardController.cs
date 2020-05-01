using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEA_Application.Controllers
{
    public class SuperAdmin_DashboardController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: SuperAdmin_Dashboard
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Dashboard()
        {
          
            return View("BlankPage");
        }

    }
}