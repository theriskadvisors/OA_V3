using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SEA_Application
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Attendance",
                url: "Admin_Dashboard/Auto_Attendance",
                defaults: new { controller = "Admin_Dashboard", action = "Auto_Attendance" }
            );

            routes.MapRoute(
                name: "AspNetAssessment_Question/Create",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "AspNetAssessment_Question", action = "Create", id = UrlParameter.Optional }
            );
        }
    }
}
