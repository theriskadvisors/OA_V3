using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SEA_Application.Controllers
{
    public class Principal_DashboardController : Controller
    {

        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: Principal_Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            var principalId = User.Identity.GetUserId();

            var AllMessages = (from a in db.AspNetMessages
                       join b in db.AspNetMessage_Receiver
                       on a.Id equals b.MessageID
                       where b.ReceiverID == principalId
                       join c in db.AspNetUsers
                       on a.SenderID equals c.Id
                       select new { a.Message,a.Time, c.Name }).ToList();
            List<Message> allMessages = new List<Message>();
            foreach(var data in AllMessages)
            {
                Message m = new Message();
                m.Name = data.Name;
                m.message = data.Message;
                string monthName = data.Time.Value.ToString("MMM", CultureInfo.InvariantCulture);
                m.date = monthName + " " + data.Time.Value.Day + "," + data.Time.Value.Year;
                
                allMessages.Add(m);
            }

            ViewBag.allPrincipalMessages = allMessages;
            ViewBag.TotalMessages = db.AspNetMessage_Receiver.Where(m => m.ReceiverID == principalId && m.Seen=="Not Seen").Count();
            ViewBag.TotalUsers = (from uid in db.AspNetUsers
                                     join sid in db.AspNetStudents
                                     on uid.Id equals sid.StudentID
                                     where uid.Status != "False"
                                     select sid.StudentID).Count();
            ViewBag.TotalNotifications = db.AspNetNotification_User.Where(m => m.UserID == principalId && m.Seen==false).Count();


            var ty = (from classid in db.AspNetHomeworks
                      join classname in db.AspNetClasses on classid.ClassId equals classname.Id
                      where classid.PrincipalApproved_status == "Created" || classid.PrincipalApproved_status=="Edited"
                      select new { classname.ClassName, classid.Date, classid.Id }).ToList().OrderByDescending(a=>a.Date);
            List<TODOLIST> kjlk = new List<TODOLIST>();
            foreach (var t in ty)
            {
                TODOLIST tyr = new TODOLIST();
                tyr.Classname = t.ClassName;
                tyr.HomeWorkId = t.Id;
                
                if (t.Date.Value.Day + "/" + t.Date.Value.Month + "/" + t.Date.Value.Year != DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year)
                {
                    tyr.isToDay = false;

                }
                else
                {
                    tyr.isToDay = true;
                }
                tyr.date = t.Date.Value.Day + "/" + t.Date.Value.Month + "/" + t.Date.Value.Year;
                if (t.Date < DateTime.Now)
                {
                    int ActualTime = (DateTime.Now - t.Date.Value.Date).Days;
                    if (ActualTime == 1)
                    {
                        tyr.Actualdate = ActualTime + " day ago";
                    }
                    else if (ActualTime >= 30)
                    {
                        int months = ActualTime / 30;
                        if (months == 1)
                        {
                            tyr.Actualdate = months + " month ago";
                        }
                        else
                        {
                            tyr.Actualdate = months + " months ago";
                        }
                    }

                    else
                    {
                        int weeks = ActualTime / 7;
                        if (weeks == 0)
                        {
                            tyr.Actualdate = ActualTime + " days ago";
                        }
                        else
                        {
                            if (weeks == 1)
                                tyr.Actualdate = weeks + " week ago";
                            else
                                tyr.Actualdate = weeks + " weeks ago";
                        }
                    }
                }
                else
                {
                    tyr.Actualdate = "Today";
                }
                tyr.date = t.Date.Value.Day + "/" + t.Date.Value.Month + "/" + t.Date.Value.Year;
                kjlk.Add(tyr);
            }
            ViewBag.ClassName = kjlk;

            return View("BlankPage");
        }
        public ActionResult CalendarNotification()
        {
            List<Event> Hollyday = new List<Event>();

            var h_list = db.AspNetHoliday_Calendar_Notification.ToList();
            foreach (var a in h_list)
            {

                Event hd = new Event();
                hd.Id = a.Id;
                hd.Name = a.Title;
                hd.StartDate = a.StartDate.Year.ToString() + "-" + a.StartDate.Month.ToString() + "-" + a.StartDate.Day.ToString();
                hd.EndDate = a.EndDate.Year.ToString() + "-" + a.EndDate.Month.ToString() + "-" + a.EndDate.Day.ToString();
                hd.StartTime = "";
                hd.EndTime = "";
                hd.Color = "#9ACD32";
                hd.Url = "";
                Hollyday.Add(hd);

            }

            // var result = new {Hollyday };
            return Json(Hollyday, JsonRequestBehavior.AllowGet);
        }
        public class Event
        {
            public int Id { set; get; }
            public string Name { set; get; }
            public string StartDate { set; get; }
            public string EndDate { set; get; }
            public string StartTime { set; get; }
            public string EndTime { set; get; }
            public string Color { set; get; }
            public string Url { set; get; }

        }
        public ActionResult EmployeeAttendance()
        {
            return View();
        }

        public JsonResult AllEmployees()
        {
            var date = DateTime.Now.Date;
            var time = DateTime.Now.TimeOfDay;
            var Attendance = new Attendance();
            Attendance.EmployeeAttendance = new List<Employee_Attendances>();

            var attendance = db.AspNetEmployeeAttendances.Where(x => x.Date == date).Select(x => x).FirstOrDefault();

            //var VID = db.AspNetVirtualRoles.Where(x => x.Name == "Non Directive Staff").Select(x => x.Id).FirstOrDefault();
            var AllEmployees = db.AspNetEmployees.OrderBy(x=> x.Name).ToList();

            if (attendance == null)
            {
                Attendance.Status = false;
               
                foreach (var item in AllEmployees)
                {
                    Employee_Attendances EmployeeAttendance = new Employee_Attendances();
                    EmployeeAttendance.Id = item.Id;
                    EmployeeAttendance.Name = item.Name;

                    Attendance.EmployeeAttendance.Add(EmployeeAttendance);
                }
            }
            else
            {
                Attendance.Status = true;
                Attendance.Date = (DateTime)attendance.Date;
                var Employees = db.AspNetEmployee_Attendance.Where(x => x.AttendanceID == attendance.Id).ToList();

                foreach (var item in Employees)
                {
                    Employee_Attendances EmployeeAttendance = new Employee_Attendances();
                    EmployeeAttendance.Id =(int)item.EmployeeID;
                    EmployeeAttendance.Name = item.AspNetEmployee.Name;
                    EmployeeAttendance.Reason = item.Reason;
                    EmployeeAttendance.Status = item.Status;

                    Attendance.EmployeeAttendance.Add(EmployeeAttendance);
                }       
            }

            return Json(Attendance, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveEmployeeAttendance(Attendance attendances)
        {
            var date = DateTime.Now.Date;
            var month = DateTime.Now.ToString("MMMM" , CultureInfo.CreateSpecificCulture("en-US"));
            var attendance = db.AspNetEmployeeAttendances.Where(x => x.Date == date).FirstOrDefault();

            if(attendance == null)
            {
                var EmployeeAttendance = new AspNetEmployeeAttendance();
                EmployeeAttendance.Date = date;

                db.AspNetEmployeeAttendances.Add(EmployeeAttendance);
                db.SaveChanges();

                foreach (var item in attendances.EmployeeAttendance)
                {
                    var Employee_Attendance = new AspNetEmployee_Attendance();
                    Employee_Attendance.EmployeeID = item.Id;
                    Employee_Attendance.AttendanceID = EmployeeAttendance.Id;
                    Employee_Attendance.Status = item.Status;
                    Employee_Attendance.Reason = item.Reason;
                    Employee_Attendance.Month = month;
                    Employee_Attendance.Time = DateTime.Now.TimeOfDay.ToString();
                    db.AspNetEmployee_Attendance.Add(Employee_Attendance);
                    db.SaveChanges();
                }
                
            }
            else
            {
                foreach (var item in attendances.EmployeeAttendance)
                {
                    var Employee_Attendance = db.AspNetEmployee_Attendance.Where(x => x.AspNetEmployeeAttendance.Date == date && x.EmployeeID == item.Id).Select(x=> x).FirstOrDefault();
                    Employee_Attendance.EmployeeID = item.Id;
                    Employee_Attendance.AttendanceID = attendance.Id;
                    if (Employee_Attendance.Status != item.Status)
                    {
                        Employee_Attendance.Time = DateTime.Now.TimeOfDay.ToString();
                    }
                    Employee_Attendance.Status = item.Status;
                    Employee_Attendance.Reason = item.Reason;
                    Employee_Attendance.Month = month;
                    db.SaveChanges();
                }
            }

            return Json("Employee attendance saved successfully", JsonRequestBehavior.AllowGet);
        }

        public class Attendance
        {
            public int Id { get; set; }
            public List<Employee_Attendances> EmployeeAttendance { get; set; }
            public bool Status { get; set; }
            public DateTime Date { get; set; }
            public string Time { get; set; }
        }

        public class Employee_Attendances
        {

            public int Id { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public string Reason { get; set; }

        }
        public class TODOLIST
        {

            public int HomeWorkId { get; set; }
            public string date { get; set; }
            public string Actualdate { get; set; }
            public string Classname { get; set; }

            public bool isToDay { get; set; }
        }
        public class Message
        {
            public string Name { get; set; }
            public string message { get; set; }
            public string date { get; set; }

        }
    }
}