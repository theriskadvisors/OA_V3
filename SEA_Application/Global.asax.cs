using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication;
using Microsoft.AspNet.Identity;
using Quartz;
using Quartz.Impl;
using System.Web.Caching;
using static SEA_Application.Controllers.AspNetRolesController;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Threading;
using SEA_Application.Models;

namespace SEA_Application
{
    public class MvcApplication : System.Web.HttpApplication
    {
        SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        protected void Application_Start()
        {
            //var time = db.AspNetTime_Setting.Where(x => x.Id == 1).FirstOrDefault();
            //var absentTime = time.AbsentTime;
            //var timeofday = DateTime.Now;
            //var date = timeofday.Date;
            //var day = timeofday.DayOfWeek.ToString();
            //int hour = absentTime.Hours;
            //int min = absentTime.Minutes;
            //int sec = 00;
            //var holiday = db.AspNetHoliday_Calendar_Notification.Where(x=>x.EndDate>=date).ToList();
            //if(holiday.Count!=0)
            //{                
            //    foreach (var item in holiday)
            //    {
            //        if (date <= item.StartDate && date <= item.EndDate)
            //        {
            //            if (day != "Saturday" && day != "Sunday")
            //            {
            //                SetUpTimer(new TimeSpan(hour, min, sec));
            //                SetUpTimer(absentTime);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    if (day != "Saturday" && day != "Sunday")
            //    {
            //        SetUpTimer(new TimeSpan(hour, min, sec));
            //        SetUpTimer(absentTime);
            //    }
            //}
           
           
          

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(System.Web.Http.GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebApplication.BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        private System.Threading.Timer timer;
        private void SetUpTimer(TimeSpan alertTime)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = alertTime - current.TimeOfDay;
            if (timeToGo < TimeSpan.Zero)
            {
                return;//time already passed
            }
            this.timer = new System.Threading.Timer(x =>
            {
                this.SomeMethodRunsAt1600();
            }, null, timeToGo, Timeout.InfiniteTimeSpan);
        }
        private void SomeMethodRunsAt1600()
        {           
            var datetime = DateTime.Now;
            var currentdate = datetime.Date;
            var timeout = datetime.TimeOfDay;
            var tout = db.AspNetStudent_AutoAttendance.Where(x => x.Date == currentdate && x.TimeOut == null).ToList();
            foreach (var item in tout)
            {
                AspNetStudent_AutoAttendance at = db.AspNetStudent_AutoAttendance.Where(x => x.Roll_Number == item.Roll_Number && x.Date == currentdate).FirstOrDefault();
                at.TimeOut = timeout;
                db.SaveChanges();
            }

            var pstd = db.AspNetStudent_AutoAttendance.Where(x => x.Date == currentdate).Select(x => x.Roll_Number).ToList();
            var tstd = db.AspNetStudents.Where(x => x.AspNetUser.Status != "False").Select(x => x.AspNetUser.UserName).ToList();
            var astd = tstd.Except(pstd).ToList();
            foreach (var item in astd)
            {
                AspNetAbsent_Student at = new AspNetAbsent_Student();
                var att = (from user in db.AspNetUsers
                           join std in db.AspNetStudents
                           on user.Id equals std.StudentID
                           where user.UserName == item && std.StudentID == user.Id
                           select new {user.UserName, std.AspNetClass.ClassName }).FirstOrDefault();

                at.Class = att.ClassName;
                at.Date = currentdate;
                at.Roll_Number = att.UserName;
                db.AspNetAbsent_Student.Add(at);
                db.SaveChanges();
            }


        }
        void Application_Error(object sender, EventArgs e)
        {
           
        // Code that runs when an unhandled error occurs

        // Get the exception object.
        Exception exc = Server.GetLastError();

            string message = exc.Message;
            string stacktrace = exc.StackTrace;
            var userName = User.Identity.GetUserName();
            var ipaddress = GetIP();

            string MailMessage = "<div><b>UserName:</b> " + userName + " <br /><b>IpAddress:</b> " + ipaddress + " <br /><b>Source:</b> " + exc.Source + " <br /><b>Exception:</b> " + exc.Message+ "// //" + exc.Data + " <br /><b>InnerException:</b> " + exc.InnerException+ " <br /><b>StackTrace:</b> " + exc.StackTrace+" </div>";

            var result = SendEmail("azeemazeem187@gmail.com", "exception of SEA", MailMessage);
        }

        public string GetIP()
        {
            string Str = "";
            Str = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(Str);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[addr.Length - 1].ToString();

        }

        public bool SendEmail(string toEmail, string subject, string emailBody)
            {
            try
            {
                //var senderEmail = "azeemazeem187@gmail.com";
                //var senderPassword = "*********";

                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                //string senderEmail = "azeemazeem187@gmail.com";
                //string senderPassword = "Skyprince99";
                string[] EmailList = new string[] { toEmail, "mr.asadishaq10@gmail.com"};
                foreach (var item in EmailList)
                {
                    SmtpClient client = new SmtpClient("smtpout.secureserver.net", 465);
                    client.EnableSsl = true;
                    client.Timeout = 100000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    MailMessage mailMessage = new MailMessage(senderEmail, item, subject, emailBody);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = UTF8Encoding.UTF8;

                    client.Send(mailMessage);
                }
                

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
