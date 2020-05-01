using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEA_Application.Models
{
    public static class GetLocalDateTime
    {

        public static DateTime? GetLocalDateTimeFunction()
        {
            TimeZone time2 = TimeZone.CurrentTimeZone;
            DateTime test = time2.ToUniversalTime(DateTime.Now);
            var pakistan = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            DateTime pakistantime = TimeZoneInfo.ConvertTimeFromUtc(test, pakistan);
            return pakistantime;

        }

    }
}