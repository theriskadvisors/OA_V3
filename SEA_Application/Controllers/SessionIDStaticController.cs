using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEA_Application.Controllers
{
    public static class SessionIDStaticController 
    {
        /// <summary>
        /// Global variable storing GlobalSessionID.
        /// </summary>
        static string _GlobalSessionID;

        /// <summary>
        /// Get or set the static important data.
        /// </summary>
        public static string GlobalSessionID
        {
            get
            {
                return _GlobalSessionID;
            }
            set
            {
                _GlobalSessionID = value;
            }
        }
    }
}