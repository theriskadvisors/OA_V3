using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;  
using System.Web;
using System.Web.Mvc;

namespace SEA_Application.Controllers 
{   

    public class HomeController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        [AllowAnonymous]
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        { 
            ViewBag.Message = "Your application description page.";
                return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}