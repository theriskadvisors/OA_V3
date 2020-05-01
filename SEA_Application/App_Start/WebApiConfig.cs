using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SEA_Application
{
    class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });
        }
    }
}
