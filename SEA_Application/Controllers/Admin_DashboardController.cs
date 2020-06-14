using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OfficeOpenXml;
using SEA_Application.CustomModel;
using SEA_Application.libs;
using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Web;
using System.Web.Mvc;

namespace SEA_Application.Controllers
{
    [Authorize(Roles = "Accountant,Admin,Principal")]
    public class Admin_DashboardController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Convert.ToInt32(SessionIDStaticController.GlobalSessionID);
        // GET: Admin_Dashboard
        public ActionResult Index()
        {
            return View();

        }


        public JsonResult GetEvents()
        {
            using (SEA_DatabaseEntities dc = new SEA_DatabaseEntities())
            {
                var id = User.Identity.GetUserId();
                var events = dc.Events.Where(x => x.UserId == id || x.IsPublic == true).Select(x => new { x.Description, x.End, x.EventID, x.IsFullDay, x.Subject, x.ThemeColor, x.Start, x.IsPublic }).ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            e.UserId = User.Identity.GetUserId();
            var status = false;
            using (SEA_DatabaseEntities dc = new SEA_DatabaseEntities())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.Events.Add(e);
                }

                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (SEA_DatabaseEntities dc = new SEA_DatabaseEntities())
            {
                var v = dc.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Events.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult test1()
        {
            //SendMail("talhaghaffar98@gmail.com", "Email Test", "1234321");
             SendMail44("talhaghaffar98@gmail.com", "Email Test", "1234321");
        //  SendMail44
            
            var Error = "Sent";
            return RedirectToAction("StudentIndex", "AspNetUser", new { Error
          });
            //return View();
        }

        public ActionResult test2()
        {
         //   SendEmail_z("talhaghaffar98@gmail.com", "Email Test", "1234321");
            SendMail_old("talhaghaffar98@gmail.com", "Email Test", "1234321");

            var Error = "Sent";
            return RedirectToAction("StudentIndex", "AspNetUser", new
            {
                Error
            });
            //return View();
        }

        public bool SendMail44(string toEmail, string subjeEnumerableDebugViewct, string emailBody)
        {
            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                string[] EmailList = new string[] { toEmail, "azeemazeem187@gmail.com" , "Studentids777@gmail.com" };
                foreach (var item in EmailList)
                {
                    SmtpClient client = new SmtpClient("relay-hosting.secureserver.net", 25);
                    client.EnableSsl = false;
                    client.Timeout = 100000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    MailMessage mailMessage = new MailMessage(senderEmail, item, "Officers Academy", emailBody);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = UTF8Encoding.UTF8;

                    client.Send(mailMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                var logs = new AspNetLog();
                logs.Operation = ex.Message + " -----" + ex.InnerException.Message;
                logs.UserID = User.Identity.GetUserId();
                db.AspNetLogs.Add(logs);
                db.SaveChanges();
                return false;
            }

        }

        public Admin_DashboardController()
        {

        }
        public ActionResult Auto_Attendance()
        {
            return View();
        }
        public ActionResult checkdata(string valval)
        {

            return Json(valval, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Student_AssessmentReport(string StudentId, string TermId)
        {
            ViewBag.StudentId = StudentId;
            ViewBag.TermId = TermId;
            return View();
        }

        public ActionResult GetTeachersList()
        {

            List<GetAllTeachers_Result> list = new List<GetAllTeachers_Result>();
            list = db.GetAllTeachers().ToList();

            string status = Newtonsoft.Json.JsonConvert.SerializeObject(list);

            return Content(status);
        }

        public ActionResult GetSubjectsList(int id)
        {

            List<GetAllSubjects_Result> list = new List<GetAllSubjects_Result>();
            list = db.GetAllSubjects().Where(x => x.Emp_ID == id).ToList();
            string status = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return Content(status);
        }
        public ActionResult GetChaptersList(int id)
        {

            var result = from s in db.AspNetSubjects
                         join ts in db.AspNetChapters
                         on s.Id equals ts.SubjectID
                         where (ts.SubjectID == id)
                         select new
                         {
                             Id = ts.Id,

                             ChapterName = ts.ChapterName,
                             ChapterNo = ts.ChapterNo,
                             SubjectID = ts.SubjectID
                         };

            string status = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return Content(status);
        }

        public ActionResult GetTopicsList(int id)
        {
            var result = from s in db.AspNetChapters
                         join ts in db.AspNetTopics
                         on s.Id equals ts.ChapterID
                         where (ts.ChapterID == id)
                         select new
                         {
                             Id = ts.Id,
                             TopicName = ts.TopicName,
                             TopicNo = ts.TopicNo
                         };

            string status = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return Content(status);
        }
        public ActionResult GetTopicPercentage(int id)
        {
            var TopicPercentage = db.AspNetTopics.Where(x => x.Id == id).FirstOrDefault().Percentage_Completion;


            return Json(new { TopicPercentage = TopicPercentage }, JsonRequestBehavior.AllowGet);

        }
        private static string GetOrdinalSuffix(int num)
        {
            string number = num.ToString();
            if (number.EndsWith("11")) return "th";
            if (number.EndsWith("12")) return "th";
            if (number.EndsWith("13")) return "th";
            if (number.EndsWith("1")) return "st";
            if (number.EndsWith("2")) return "nd";
            if (number.EndsWith("3")) return "rd";
            return "th";
        }
        public ActionResult CalendarNotification()
        {
            var id = User.Identity.GetUserId();
            var checkdate = DateTime.Now;
            var date = TimeZoneInfo.ConvertTime(DateTime.UtcNow.ToUniversalTime(), TimeZoneInfo.Local);
            var name = "";

            name = db.AspNetUsers.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();

            var day = date.DayOfWeek;
            var dd = date.Day;
            var mm = date.Month;
            var yy = date.Year;

            var dayInString = dd + GetOrdinalSuffix(dd);
            string[] array = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            var Date = day + ", " + dayInString + " " + array[mm - 1] + " " + yy;
            var result = new { checkdate, Date, name };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public class event1
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
        public ActionResult PrintPreviewData(string StudentId, string type, string TermId)
        {
            int TId = int.Parse(TermId);
            var subjects = db.GetStudentSubjects(StudentId).ToList();

            var comments = db.AspNetStudent_Term_Assessment.Where(x => x.StudentID == StudentId && x.TermID == TId).FirstOrDefault();
            string parentComent = "";
            string teacherComent = "";
            string prinpipalComent = "";
            if (comments != null)
            {
                parentComent = comments.ParentsComments;
                teacherComent = comments.TeacherComments;
                prinpipalComent = comments.PrincipalComments;
            }
            else
            {
                parentComent = "";
                teacherComent = "";
                prinpipalComent = "";
            }
            var tn = User.Identity.GetUserId();
            var teachername = db.AspNetUsers.Where(x => x.Id == tn).Select(x => x.Name).FirstOrDefault();
            var ClassId = db.AspNetStudents.Where(p => p.StudentID == StudentId).FirstOrDefault().ClassID;

            var subID = db.AspNetSubjects.Where(x => x.ClassID == ClassId).Select(x => x.Id).ToList();



            var secnumber = db.AspNetTerms.Where(p => p.Id == TId).FirstOrDefault().TermNo;
            var category = (from sub in db.AspNetSubjects
                            join aq in db.AspNetAssessment_Question on sub.Id equals aq.SubjectID
                            join cat in db.AspNetAssessment_Questions_Category on aq.QuestionCategory equals cat.Id
                            where sub.SubjectName == "English Language Development" && sub.ClassID == ClassId
                            select new { cat.CategoryName }).Distinct().ToList();
            var classname = db.AspNetClasses.Where(x => x.Id == ClassId).Select(x => x.Class).FirstOrDefault();
            var studentname = db.AspNetUsers.Where(x => x.Id == StudentId).Select(x => x.UserName).FirstOrDefault();
            var dinterm = db.AspNetTerms.Where(p => p.Id == TId).FirstOrDefault().TermEndDate - db.AspNetTerms.Where(p => p.Id == TId).FirstOrDefault().TermStartDate;

            var result2 = new { status = "success", subID = subID, categoryname = category, TId = secnumber, ClassId = ClassId, classname = classname, teachername = teachername, studentname = studentname, parentComent = parentComent, teacherComent = teacherComent, prinpipalComent = prinpipalComent, subValue = subjects, dinterm = dinterm };

            return Json(result2, JsonRequestBehavior.AllowGet);



        }
        public ActionResult Assessment_PrintPreview(string StudentId, int SID, string TermId)
        {
            var TId = int.Parse(TermId);
            var ClassId = db.AspNetStudents.Where(p => p.StudentID == StudentId).FirstOrDefault().ClassID;

            var sessionid = SessionID;
            var category = (from sub in db.AspNetSubjects
                            join aq in db.AspNetAssessment_Question on sub.Id equals aq.SubjectID
                            join cat in db.AspNetAssessment_Questions_Category on aq.QuestionCategory equals cat.Id
                            where sub.SubjectName == "English Language Development" && sub.ClassID == ClassId
                            select new { cat.CategoryName }).Distinct().ToList();
            var sessionassesques = db.GetStudentSessionSubjectAssessment(StudentId, sessionid, SID).ToList();
            var cgjhk = sessionassesques.Count();
            if (cgjhk > 0)
            {
                var result2 = new { Value = sessionassesques };

                return Json(result2, JsonRequestBehavior.AllowGet);
            }

            else
            {
                var Aquestions = db.GetSubjctAssessmentQuestions(SID).ToList();

                foreach (var ques in Aquestions)
                {
                    AspNetStudent_Term_Assessment obj = new AspNetStudent_Term_Assessment();

                    obj.StudentID = StudentId;
                    obj.SubjectID = SID;
                    obj.SessionId = sessionid;
                    obj.TermID = TId;

                    db.AspNetStudent_Term_Assessment.Add(obj);
                    db.SaveChanges();

                    AspNetStudent_Term_Assessments_Answers obje = new AspNetStudent_Term_Assessments_Answers();

                    obje.STAID = obj.Id;
                    obje.Question = ques.Question;
                    obje.Catageory = ques.CategoryName;
                    obje.FirstTermGrade = "";
                    obje.SecondTermGrade = "";
                    obje.ThirdTermGrade = "";
                    //obje = ;
                    db.AspNetStudent_Term_Assessments_Answers.Add(obje);
                    db.SaveChanges();
                }
                var secnumber = db.AspNetTerms.Where(p => p.Id == TId).FirstOrDefault().TermNo;

                var sessionassesrtques = db.GetStudentSessionSubjectAssessment(StudentId, sessionid, SID).ToList();
                var result1 = new { status = "success", Value = sessionassesrtques, categoryname = category, TId = secnumber, ClassId = ClassId };

                return Json(result1, JsonRequestBehavior.AllowGet);
            }
        }
        public ViewResult Class_Assessment()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            var sessiionid = SessionID;
            ViewBag.TermID = new SelectList(db.AspNetTerms.Where(x => x.SessionID == sessiionid), "Id", "TermName", "TermNo");
            return View("_ClassAssessment");
        }

        public Admin_DashboardController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public ActionResult Dashboard()
        {
            var CurrentUserId = User.Identity.GetUserId();
            var allMessages = (from a in db.AspNetMessages
                               join b in db.AspNetMessage_Receiver
                               on a.Id equals b.MessageID
                               where b.ReceiverID == CurrentUserId && b.Seen == "Not Seen"
                               join c in db.AspNetUsers
                              on a.SenderID equals c.Id
                               select new { a.Message, a.Time, c.Name }).ToList();
            List<Message> messages = new List<Message>();
            foreach (var item in allMessages)
            {
                Message m = new Message();
                m.Name = item.Name;
                m.message = item.Message;
                string monthName = item.Time.Value.ToString("MMM", CultureInfo.InvariantCulture);
                m.date = monthName + " " + item.Time.Value.Day + "," + item.Time.Value.Year;
                messages.Add(m);

            }
            ViewBag.Messages = messages;

            var ty = (from classid in db.AspNetHomeworks
                      join classname in db.AspNetClasses
                      on classid.ClassId equals classname.Id
                      where classid.PrincipalApproved_status == "Created" || classid.PrincipalApproved_status == "Edited"
                      select new { classname.ClassName, classid.Date, classid.Id }).ToList().OrderByDescending(a => a.Date);
            ViewBag.TotalStudents = (from uid in db.AspNetUsers
                                     join sid in db.AspNetStudents
                                     on uid.Id equals sid.StudentID
                                     where uid.Status != "False"
                                     select sid.StudentID).Count();


            ViewBag.TotalSubjects = db.AspNetSubjects.Count();
            ViewBag.TotalSessions = db.AspNetSessions.Count();
            ViewBag.TotalTeachers = db.AspNetEmployees.Where(x => x.PositionAppliedFor == "TEACHER" && x.PositionAppliedFor == "Teacher").Count();




            ViewBag.TotalMessages = db.AspNetMessage_Receiver.Where(m => m.ReceiverID == CurrentUserId && m.Seen == "Not Seen").Count();
            ViewBag.TotalNotifications = db.AspNetNotification_User.Where(m => m.UserID == CurrentUserId && m.Seen == false).Count();

            var Classes1 = db.AspNetSubjects.Select(x => x.SubjectName).Distinct().ToList();
            List<string> classes = new List<string>();

            foreach (var clas in Classes1)
            {
                classes.Add(clas);
            }

            //classes.Add("Not Published");
            ViewBag.AllClasses = classes;

            return View("BlankPage");
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                             //
        //                                                                                             // 
        //                                             ATTENDANCE                                      //
        //                                                                                             //
        /////////////////////////////////////////////////////////////////////////////////////////////////
        public ActionResult TheKssMissionVision()
        {
            return View();
        }


        public ActionResult View_Attendance()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }

        public ActionResult GetAttendance()
        {
            var d = DateTime.Now;
            var currentdate = d.Date;
            var att_list = db.AspNetStudent_AutoAttendance.Where(x => x.Date == currentdate).ToList();
            var length = att_list.Count;
            var nn = (from un in db.AspNetUsers
                      join r in db.AspNetStudent_AutoAttendance
                      on un.UserName equals r.Roll_Number
                      where un.UserName == r.Roll_Number && r.Date == currentdate
                      select un.Name).ToList();


            List<AutoAttendance> attendance = new List<AutoAttendance>();
            for (int i = 0; i < length; i++)
            {
                AspNetStudent_AutoAttendance at = att_list[i];
                var ss = nn[i];
                AutoAttendance att = new AutoAttendance();
                att.Class = at.Class;
                att.Date = at.Date;
                att.RollNumber = at.Roll_Number;
                att.Timein = at.TimeIn;
                att.Timeout = at.TimeOut;
                att.Name = nn[i];
                attendance.Add(att);
            }

            return Json(attendance, JsonRequestBehavior.AllowGet);
        }
        //////////////////////////////////////////////Attendacne Class Filter/////////////////////////////////////////////
        public ActionResult Att_Class(int Classid)
        {
            if (Classid != 0)
            {
                var d = DateTime.Now;
                var currentdate = d.Date;
                var cname = db.AspNetClasses.Where(x => x.Id == Classid).Select(x => x.ClassName).FirstOrDefault();
                var att_list = db.AspNetStudent_AutoAttendance.Where(x => x.Class == cname && x.Date == currentdate).ToList();
                var nn = (from un in db.AspNetUsers
                          join r in db.AspNetStudent_AutoAttendance
                          on un.UserName equals r.Roll_Number
                          where un.UserName == r.Roll_Number
                          select new { un.Name, un.UserName }).ToList().Distinct();
                var result = new { att_list, nn };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var d = DateTime.Now;
                var currentdate = d.Date;
                var att_list = db.AspNetStudent_AutoAttendance.Where(x => x.Date == currentdate).ToList();

                var nn = (from un in db.AspNetUsers
                          join r in db.AspNetStudent_AutoAttendance
                          on un.UserName equals r.Roll_Number
                          where un.UserName == r.Roll_Number
                          select new { un.Name, un.UserName }).ToList().Distinct();
                var result = new { att_list, nn };
                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }
        ////////////////////////////////////////////////////Status Filter//////////////////////////////////////
        public ActionResult Filter_Attendance(string type, int Classid)
        {
            List<AutoAttendance> attendance = new List<AutoAttendance>();
            var d = DateTime.Now;
            var currentdate = d.Date;
            if (type == "Present")
            {
                if (Classid == 0)
                {
                    var att_list = db.AspNetStudent_AutoAttendance.Where(x => x.Date == currentdate).ToList();
                    var length = att_list.Count;
                    var nn = (from un in db.AspNetUsers
                              join r in db.AspNetStudent_AutoAttendance
                              on un.UserName equals r.Roll_Number
                              where un.UserName == r.Roll_Number && r.Date == currentdate
                              select un.Name).ToList();

                    for (int i = 0; i < length; i++)
                    {
                        AspNetStudent_AutoAttendance at = att_list[i];
                        var ss = nn[i];
                        AutoAttendance att = new AutoAttendance();
                        att.Class = at.Class;
                        att.Date = at.Date;
                        att.RollNumber = at.Roll_Number;
                        att.Timein = at.TimeIn;
                        att.Timeout = at.TimeOut;
                        att.Name = nn[i];
                        attendance.Add(att);
                    }

                    return Json(attendance, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var cname = db.AspNetClasses.Where(x => x.Id == Classid).Select(x => x.ClassName).FirstOrDefault();
                    var att_list = db.AspNetStudent_AutoAttendance.Where(x => x.Date == currentdate && x.Class == cname).ToList();
                    var length = att_list.Count;


                    for (int i = 0; i < length; i++)
                    {
                        AspNetStudent_AutoAttendance at = att_list[i];

                        var nn = (from un in db.AspNetUsers
                                  join r in db.AspNetStudent_AutoAttendance
                                  on un.UserName equals r.Roll_Number
                                  where un.UserName == at.Roll_Number && r.Date == currentdate
                                  select un.Name).FirstOrDefault();

                        AutoAttendance att = new AutoAttendance();
                        att.Class = at.Class;
                        att.Date = at.Date;
                        att.RollNumber = at.Roll_Number;
                        att.Timein = at.TimeIn;
                        att.Timeout = at.TimeOut;
                        att.Name = nn;
                        attendance.Add(att);
                    }

                    return Json(attendance, JsonRequestBehavior.AllowGet);
                }

            }
            else if (type == "Absent")
            {
                if (Classid == 0)
                {
                    var pstd = db.AspNetStudent_AutoAttendance.Where(x => x.Date == currentdate).Select(x => x.Roll_Number).ToList();
                    var tstd = db.AspNetStudents.Where(x => x.AspNetUser.Status != "False").Select(x => x.AspNetUser.UserName).ToList();
                    var astd = tstd.Except(pstd).ToList();
                    foreach (var item in astd)
                    {
                        AutoAttendance at = new AutoAttendance();
                        var att = (from user in db.AspNetUsers
                                   join std in db.AspNetStudents
                                   on user.Id equals std.StudentID
                                   where user.UserName == item && std.StudentID == user.Id
                                   select new { user.Name, user.UserName, std.AspNetClass.ClassName }).FirstOrDefault();

                        at.Class = att.ClassName;
                        at.Date = currentdate;
                        at.RollNumber = att.UserName;
                        at.Timein = null;
                        at.Timeout = null;
                        at.Name = att.Name;
                        attendance.Add(at);
                    }

                    return Json(attendance, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var cname = db.AspNetClasses.Where(x => x.Id == Classid).Select(x => x.ClassName).FirstOrDefault();

                    var pstd = db.AspNetStudent_AutoAttendance.Where(x => x.Date == currentdate && x.Class == cname).Select(x => x.Roll_Number).ToList();
                    var tstd = db.AspNetStudents.Where(x => x.AspNetUser.Status != "False" && x.ClassID == Classid).Select(x => x.AspNetUser.UserName).ToList();
                    var astd = tstd.Except(pstd).ToList();
                    foreach (var item in astd)
                    {
                        AutoAttendance at = new AutoAttendance();
                        var att = (from user in db.AspNetUsers
                                   join std in db.AspNetStudents
                                   on user.Id equals std.StudentID
                                   where user.UserName == item && std.StudentID == user.Id
                                   select new { user.Name, user.UserName, std.AspNetClass.ClassName }).FirstOrDefault();

                        at.Class = att.ClassName;
                        at.Date = currentdate;
                        at.RollNumber = att.UserName;
                        at.Timein = null;
                        at.Timeout = null;
                        at.Name = att.Name;
                        attendance.Add(at);
                    }
                    return Json(attendance, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                //var timecheck = Convert.ToDateTime("10:00:00 AM");
                //var time = timecheck.TimeOfDay;
                var time = db.AspNetTime_Setting.Where(x => x.Id == 1).FirstOrDefault();
                var lateTime = time.LateTime;
                if (Classid == 0)
                {
                    var att_list = db.AspNetStudent_AutoAttendance.Where(x => x.Date == currentdate && x.TimeIn > lateTime).ToList();
                    var length = att_list.Count;
                    var nn = (from un in db.AspNetUsers
                              join r in db.AspNetStudent_AutoAttendance
                              on un.UserName equals r.Roll_Number
                              where un.UserName == r.Roll_Number && r.Date == currentdate && r.TimeIn > lateTime
                              select un.Name).ToList();

                    for (int i = 0; i < length; i++)
                    {
                        AspNetStudent_AutoAttendance at = att_list[i];
                        var ss = nn[i];
                        AutoAttendance att = new AutoAttendance();
                        att.Class = at.Class;
                        att.Date = at.Date;
                        att.RollNumber = at.Roll_Number;
                        att.Timein = at.TimeIn;
                        att.Timeout = at.TimeOut;
                        att.Name = nn[i];
                        attendance.Add(att);
                    }
                    return Json(attendance, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var cname = db.AspNetClasses.Where(x => x.Id == Classid).Select(x => x.ClassName).FirstOrDefault();
                    var att_list = db.AspNetStudent_AutoAttendance.Where(x => x.TimeIn > lateTime && x.Date == currentdate && x.Class == cname).ToList();
                    var length = att_list.Count;
                    var nn = (from un in db.AspNetUsers
                              join r in db.AspNetStudent_AutoAttendance
                              on un.UserName equals r.Roll_Number
                              where un.UserName == r.Roll_Number && r.Date == currentdate && r.TimeIn > lateTime
                              select un.Name).ToList();

                    for (int i = 0; i < length; i++)
                    {
                        AspNetStudent_AutoAttendance at = att_list[i];
                        //  var ss = nn[i];
                        AutoAttendance att = new AutoAttendance();
                        att.Class = at.Class;
                        att.Date = at.Date;
                        att.RollNumber = at.Roll_Number;
                        att.Timein = at.TimeIn;
                        att.Timeout = at.TimeOut;
                        att.Name = nn[i];
                        attendance.Add(att);

                    }
                    return Json(attendance, JsonRequestBehavior.AllowGet);
                }

            }

        }
        public ActionResult Att_Details(string userName)
        {

            ViewBag.username = userName;
            return View();
        }
        public ActionResult GetAttendance_Details(string RollNumber)
        {
            var std = db.AspNetStudent_AutoAttendance.Where(x => x.Roll_Number == RollNumber).ToList();
            var name = db.AspNetUsers.Where(x => x.UserName == RollNumber).Select(x => x.Name).FirstOrDefault();
            var result = new { std, name };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /////////////////////////////////////////////////////STUDENT STATUS_DETAILS////////////////////////////////////////////

        public ActionResult Std_Status_Details(string RollNumber, string status)
        {
            if (status == "Present")
            {
                var attendance = db.AspNetStudent_AutoAttendance.Where(x => x.Roll_Number == RollNumber).ToList();
                var name = db.AspNetUsers.Where(x => x.UserName == RollNumber).Select(x => x.Name).FirstOrDefault();
                var result = new { attendance, name };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (status == "Absent")
            {
                var attendance = db.AspNetAbsent_Student.Where(x => x.Roll_Number == RollNumber).ToList();
                var name = db.AspNetUsers.Where(x => x.UserName == RollNumber).Select(x => x.Name).FirstOrDefault();
                var result = new { attendance, name };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var time = db.AspNetTime_Setting.Where(x => x.Id == 1).FirstOrDefault();
                var lateTime = time.LateTime;
                var attendance = db.AspNetStudent_AutoAttendance.Where(x => x.Roll_Number == RollNumber && x.TimeIn > lateTime).ToList();
                var name = db.AspNetUsers.Where(x => x.UserName == RollNumber).Select(x => x.Name).FirstOrDefault();
                var result = new { attendance, name };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Start_End_Date(string RollNumber, string status, DateTime start, DateTime end)
        {
            if (status == "Present")
            {
                var attendance = db.AspNetStudent_AutoAttendance.Where(x => x.Roll_Number == RollNumber && x.Date >= start && x.Date <= end).ToList();
                var name = db.AspNetUsers.Where(x => x.UserName == RollNumber).Select(x => x.Name).FirstOrDefault();
                var result = new { attendance, name };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (status == "Absent")
            {
                var attendance = db.AspNetAbsent_Student.Where(x => x.Roll_Number == RollNumber && x.Date >= start && x.Date <= end).ToList();
                var name = db.AspNetUsers.Where(x => x.UserName == RollNumber).Select(x => x.Name).FirstOrDefault();
                var result = new { attendance, name };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var time = db.AspNetTime_Setting.Where(x => x.Id == 1).FirstOrDefault();
                var lateTime = time.LateTime;
                var attendance = db.AspNetStudent_AutoAttendance.Where(x => x.Roll_Number == RollNumber && x.Date >= start && x.Date <= end && x.TimeIn >= lateTime).ToList();
                var name = db.AspNetUsers.Where(x => x.UserName == RollNumber).Select(x => x.Name).FirstOrDefault();
                var result = new { attendance, name };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        ///////////////////////////////////////////ATTENDANCE DATE FILTER//////////////////////////////////////

        public ActionResult Att_DateFilter(string type, int Classid, DateTime start)
        {
            List<AutoAttendance> attendance = new List<AutoAttendance>();
            var time = db.AspNetTime_Setting.Where(x => x.Id == 1).FirstOrDefault();
            var lateTime = time.LateTime;
            if (Classid == 0 && type == "Present")
            {
                var att_list = db.AspNetStudent_AutoAttendance.Where(x => x.Date == start).ToList();
                var length = att_list.Count;
                var nn = (from un in db.AspNetUsers
                          join r in db.AspNetStudent_AutoAttendance
                          on un.UserName equals r.Roll_Number
                          where un.UserName == r.Roll_Number && r.Date == start
                          select un.Name).ToList();

                for (int i = 0; i < length; i++)
                {
                    AspNetStudent_AutoAttendance at = att_list[i];
                    AutoAttendance att = new AutoAttendance();
                    att.Class = at.Class;
                    att.Date = at.Date;
                    att.RollNumber = at.Roll_Number;
                    att.Timein = at.TimeIn;
                    att.Timeout = at.TimeOut;
                    att.Name = nn[i];
                    attendance.Add(att);
                }
                return Json(attendance, JsonRequestBehavior.AllowGet);
            }
            else if (Classid == 0 && type == "Absent")
            {
                var pstd = db.AspNetStudent_AutoAttendance.Where(x => x.Date == start).Select(x => x.Roll_Number).ToList();
                var tstd = db.AspNetStudents.Where(x => x.AspNetUser.Status != "False").Select(x => x.AspNetUser.UserName).ToList();
                var astd = tstd.Except(pstd).ToList();

                foreach (var item in astd)
                {
                    AutoAttendance at = new AutoAttendance();
                    var att = (from user in db.AspNetUsers
                               join std in db.AspNetStudents
                               on user.Id equals std.StudentID
                               where user.UserName == item && std.StudentID == user.Id
                               select new { user.Name, user.UserName, std.AspNetClass.ClassName }).FirstOrDefault();

                    at.Class = att.ClassName;
                    at.Date = start;
                    at.RollNumber = att.UserName;
                    at.Timein = null;
                    at.Timeout = null;
                    at.Name = att.Name;
                    attendance.Add(at);
                }
                return Json(attendance, JsonRequestBehavior.AllowGet);

            }
            else if (Classid == 0 && type == "Late")
            {
                //var timecheck = Convert.ToDateTime("10:00:00 AM");
                //var time = timecheck.TimeOfDay;
                var att_list = db.AspNetStudent_AutoAttendance.Where(x => x.Date == start && x.TimeIn > lateTime).ToList();
                var length = att_list.Count;
                var nn = (from un in db.AspNetUsers
                          join r in db.AspNetStudent_AutoAttendance
                          on un.UserName equals r.Roll_Number
                          where un.UserName == r.Roll_Number && r.Date == start && r.TimeIn > lateTime
                          select un.Name).ToList();

                for (int i = 0; i < length; i++)
                {
                    AspNetStudent_AutoAttendance at = att_list[i];
                    var ss = nn[i];
                    AutoAttendance att = new AutoAttendance();
                    att.Class = at.Class;
                    att.Date = at.Date;
                    att.RollNumber = at.Roll_Number;
                    att.Timein = at.TimeIn;
                    att.Timeout = at.TimeOut;
                    att.Name = nn[i];
                    attendance.Add(att);
                }
                return Json(attendance, JsonRequestBehavior.AllowGet);
            }
            else if (Classid != 0 && type == "Present")
            {
                var cname = db.AspNetClasses.Where(x => x.Id == Classid).Select(x => x.ClassName).FirstOrDefault();
                var att_list = db.AspNetStudent_AutoAttendance.Where(x => x.Date == start && x.Class == cname).ToList();
                var length = att_list.Count;


                for (int i = 0; i < length; i++)
                {
                    AspNetStudent_AutoAttendance at = att_list[i];

                    var nn = (from un in db.AspNetUsers
                              join r in db.AspNetStudent_AutoAttendance
                              on un.UserName equals r.Roll_Number
                              where un.UserName == at.Roll_Number && r.Date == start
                              select un.Name).FirstOrDefault();
                    AutoAttendance att = new AutoAttendance();
                    att.Class = at.Class;
                    att.Date = at.Date;
                    att.RollNumber = at.Roll_Number;
                    att.Timein = at.TimeIn;
                    att.Timeout = at.TimeOut;
                    att.Name = nn;
                    attendance.Add(att);

                }
                return Json(attendance, JsonRequestBehavior.AllowGet);
            }
            else if (Classid != 0 && type == "Absent")
            {
                var cname = db.AspNetClasses.Where(x => x.Id == Classid).Select(x => x.ClassName).FirstOrDefault();
                var pstd = db.AspNetStudent_AutoAttendance.Where(x => x.Date == start && x.Class == cname).Select(x => x.Roll_Number).ToList();
                var tstd = db.AspNetStudents.Where(x => x.AspNetUser.Status != "False" && x.ClassID == Classid).Select(x => x.AspNetUser.UserName).ToList();
                var astd = tstd.Except(pstd).ToList();

                foreach (var item in astd)
                {
                    AutoAttendance at = new AutoAttendance();
                    var att = (from user in db.AspNetUsers
                               join std in db.AspNetStudents
                               on user.Id equals std.StudentID
                               where user.UserName == item && std.StudentID == user.Id
                               select new { user.Name, user.UserName, std.AspNetClass.ClassName }).FirstOrDefault();

                    at.Class = att.ClassName;
                    at.Date = start;
                    at.RollNumber = att.UserName;
                    at.Timein = null;
                    at.Timeout = null;
                    at.Name = att.Name;
                    attendance.Add(at);
                }
                return Json(attendance, JsonRequestBehavior.AllowGet);
            }
            else if (Classid != 0 && type == "Late")
            {

                //  var timecheck = Convert.ToDateTime("10:00:00");
                // var time = timecheck.TimeOfDay;
                var cname = db.AspNetClasses.Where(x => x.Id == Classid).Select(x => x.ClassName).FirstOrDefault();
                var att_list = db.AspNetStudent_AutoAttendance.Where(x => x.TimeIn > lateTime && x.Date == start && x.Class == cname).ToList();
                var length = att_list.Count;


                for (int i = 0; i < length; i++)
                {
                    AspNetStudent_AutoAttendance at = att_list[i];
                    var nn = (from un in db.AspNetUsers
                              join r in db.AspNetStudent_AutoAttendance
                              on un.UserName equals r.Roll_Number
                              where un.UserName == at.Roll_Number && r.Date == start && r.TimeIn > lateTime
                              select un.Name).FirstOrDefault();
                    AutoAttendance att = new AutoAttendance();
                    att.Class = at.Class;
                    att.Date = at.Date;
                    att.RollNumber = at.Roll_Number;
                    att.Timein = at.TimeIn;
                    att.Timeout = at.TimeOut;
                    att.Name = nn;
                    attendance.Add(att);

                }
                return Json(attendance, JsonRequestBehavior.AllowGet);

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        ///////////////////////////////////////////////END/////////////////////////////////////////////////////////////

        public class Message
        {
            public string Name { get; set; }
            public string message { get; set; }

            public string date { get; set; }
        }
        public class AutoAttendance
        {
            public string Name { get; set; }
            public string Class { get; set; }
            public DateTime? Date { get; set; }
            public string RollNumber { get; set; }
            public TimeSpan? Timein { get; set; }
            public TimeSpan? Timeout { get; set; }

        }
        public class TODOLIST
        {

            public int HomeWorkId { get; set; }
            public string date { get; set; }
            public string Actualdate { get; set; }
            public string Classname { get; set; }

            public bool isToDay { get; set; }
        }

        /*******************************************************************************************************************/
        /*                                                                                                                 */
        /*                                    Accountant's Functions                                                       */
        /*                                                                                                                 */
        /*******************************************************************************************************************/
        public ActionResult Student_Data(string studentId)
        {
            var name = db.AspNetUsers.Where(x => x.Id == studentId).Select(x => x.Name).FirstOrDefault();
            var username = db.AspNetUsers.Where(x => x.Id == studentId).Select(x => x.UserName).FirstOrDefault();
            var cid = db.AspNetStudents.Where(x => x.StudentID == studentId).Select(x => x.ClassID).FirstOrDefault();
            var tid = db.AspNetClasses.Where(x => x.Id == cid).Select(x => x.TeacherID).FirstOrDefault();
            var tname = db.AspNetUsers.Where(x => x.Id == tid).Select(x => x.Name).FirstOrDefault();
            var resutl = new { name, username, tname };
            return Json(resutl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AccountantEdit(HttpPostedFileBase image)
        {
            string id = Request.Form["id"];

            var user = db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
            var details = db.AspNetEmployees.Where(x => x.UserId == id).FirstOrDefault();
            var transaction = db.Database.BeginTransaction();
            try
            {
                if (image != null)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var extension = Path.GetExtension(image.FileName);
                    image.SaveAs(Server.MapPath("~/Content/Images/StudentImages/") + image.FileName);
                    //    file.SaveAs(Server.MapPath("/Upload/") + file.FileName);
                }

                user.Name = Request.Form["Name"];
                user.UserName = Request.Form["UserName"];
                user.Email = Request.Form["Email"];
                user.PhoneNumber = Request.Form["cellNo"];
                user.Image = image.FileName;

                details.PositionAppliedFor = Request.Form["appliedFor"];
                details.DateAvailable = Request.Form["dateAvailable"];
                details.JoiningDate = Request.Form["JoiningDate"];
                details.BirthDate = Request.Form["birthDate"];
                details.Nationality = Request.Form["nationality"];
                details.Religion = Request.Form["religion"];
                details.Gender = Request.Form["gender"];
                details.MailingAddress = Request.Form["mailingAddress"];
                details.Email = user.Email;
                details.Name = user.Name;
                details.UserName = user.UserName;
                details.CellNo = user.PhoneNumber;
                details.Landline = Request.Form["landLine"];
                details.SpouseName = Request.Form["spouseName"];
                details.SpouseHighestDegree = Request.Form["spouseHighestDegree"];
                details.SpouseOccupation = Request.Form["spouseOccupation"];
                details.SpouseBusinessAddress = Request.Form["spouseBusinessAddress"];
                details.GrossSalary = Convert.ToInt32(Request.Form["GrossSalary"]);
                details.BasicSalary = Convert.ToInt32(Request.Form["BasicSalary"]);
                details.MedicalAllowance = Convert.ToInt32(Request.Form["MedicalAllowance"]);
                details.Accomodation = Convert.ToInt32(Request.Form["Accomodation"]);
                details.ProvidedFund = Convert.ToInt32(Request.Form["ProvidedFund"]);
                details.Tax = Convert.ToInt32(Request.Form["Tax"]);
                details.EOP = Convert.ToInt32(Request.Form["EOP"]);

                db.SaveChanges();
                transaction.Commit();
                return RedirectToAction("AccountantIndex", "AspNetUser");
            }
            catch
            {
                transaction.Dispose();
                return View("Sonething Went wrong");
            }

        }

        public ActionResult AccountantRegister()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountantRegister(RegisterViewModel model, HttpPostedFileBase image)
        {
            if (1 == 1)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Name = model.Name, PhoneNumber = Request.Form["cellNo"] };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (image != null)
                    {
                        var fileName = Path.GetFileName(image.FileName);
                        var extension = Path.GetExtension(image.FileName);
                        image.SaveAs(Server.MapPath("~/Content/Images/StudentImages/") + image.FileName);
                        //    file.SaveAs(Server.MapPath("/Upload/") + file.FileName);
                    }

                    ruffdata rd = new ruffdata();
                    rd.SessionID = SessionID;
                    rd.StudentName = model.Name;
                    rd.StudentUserName = model.UserName;
                    rd.StudentPassword = model.Password;
                    db.ruffdatas.Add(rd);
                    db.SaveChanges();
                }

                AspNetUser Accountant = new AspNetUser();
                Accountant.Name = user.Name;
                Accountant.UserName = user.UserName;
                Accountant.Email = user.Email;
                Accountant.PasswordHash = user.PasswordHash;
                Accountant.Status = "Active";
                Accountant.PhoneNumber = Request.Form["cellNo"];
                Accountant.Image = image.FileName;

                AspNetEmployee emp = new AspNetEmployee();
                emp.Email = Accountant.Email;
                emp.UserName = Accountant.UserName;
                emp.Name = Accountant.Name;
                emp.PositionAppliedFor = Request.Form["appliedFor"];
                emp.DateAvailable = Request.Form["dateAvailable"];
                emp.JoiningDate = Request.Form["JoiningDate"];
                emp.BirthDate = Request.Form["birthDate"];
                emp.Nationality = Request.Form["nationality"];
                emp.Religion = Request.Form["religion"];
                emp.Gender = Request.Form["gender"];
                emp.MailingAddress = Request.Form["mailingAddress"];
                emp.CellNo = Request.Form["cellNo"];
                emp.Landline = Request.Form["landLine"];
                emp.SpouseName = Request.Form["spouseName"];
                emp.SpouseHighestDegree = Request.Form["spouseHighestDegree"];
                emp.SpouseOccupation = Request.Form["spouseOccupation"];
                emp.SpouseBusinessAddress = Request.Form["spouseBusinessAddress"];
                emp.GrossSalary = Convert.ToInt32(Request.Form["GrossSalary"]);
                emp.BasicSalary = Convert.ToInt32(Request.Form["BasicSalary"]);
                emp.MedicalAllowance = Convert.ToInt32(Request.Form["MedicalAllowance"]);
                emp.Accomodation = Convert.ToInt32(Request.Form["Accomodation"]);
                emp.ProvidedFund = Convert.ToInt32(Request.Form["ProvidedFund"]);
                emp.Tax = Convert.ToInt32(Request.Form["Tax"]);
                emp.EOP = Convert.ToInt32(Request.Form["EOP"]);
                emp.VirtualRoleId = db.AspNetVirtualRoles.Where(x => x.Name == "Management Staff").Select(x => x.Id).FirstOrDefault();
                emp.UserId = user.Id;
                if (result.Succeeded)
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    userManager.AddToRole(user.Id, "Accountant");

                    db.AspNetEmployees.Add(emp);
                    db.SaveChanges();
                    AspNetUsers_Session US = new AspNetUsers_Session();
                    US.UserID = emp.UserId;
                    US.SessionID = SessionID;
                    db.AspNetUsers_Session.Add(US);

                    var usr = db.AspNetUsers.Where(x => x.UserName == user.UserName).FirstOrDefault();
                    usr.Image = image.FileName;
                    db.SaveChanges();

                    string Error = "Accountant Saved successfully";
                    return RedirectToAction("AccountantsIndex", "AspNetUser", new { Error });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult DisabledTeachers()
        {
            return View();
        }

        public JsonResult DisableTeachers()
        {
            var teachers = (from teacher in db.AspNetUsers.Where(x => x.Status == "False")
                            where teacher.AspNetRoles.Select(y => y.Name).Contains("Teacher")
                            select new
                            {
                                teacher.Id,
                                Class = teacher.AspNetClasses.Select(x => x.ClassName).FirstOrDefault(),
                                Subject = "-",
                                teacher.Email,
                                teacher.PhoneNumber,
                                teacher.UserName,
                                teacher.Name,
                            }).ToList();

            return Json(teachers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountantfromFile(RegisterViewModel model)
        {
            // if (ModelState.IsValid)
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["Accountants"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                }
                var AccountantList = new List<RegisterViewModel>();
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    ApplicationDbContext context = new ApplicationDbContext();
                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        var Accountant = new RegisterViewModel();
                        Accountant.Email = workSheet.Cells[rowIterator, 1].Value.ToString();
                        Accountant.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                        Accountant.UserName = workSheet.Cells[rowIterator, 3].Value.ToString();
                        Accountant.Password = workSheet.Cells[rowIterator, 4].Value.ToString();
                        Accountant.ConfirmPassword = workSheet.Cells[rowIterator, 5].Value.ToString();
                        string number = workSheet.Cells[rowIterator, 14].Value.ToString();
                        var user = new ApplicationUser { UserName = Accountant.UserName, Email = Accountant.Email, Name = Accountant.Name, PhoneNumber = number };
                        var result = await UserManager.CreateAsync(user, Accountant.Password);
                        if (result.Succeeded)
                        {
                            AspNetEmployee AccountantDetail = new AspNetEmployee();
                            AccountantDetail.Name = Accountant.Name;
                            AccountantDetail.Email = Accountant.Email;
                            AccountantDetail.UserName = Accountant.UserName;
                            AccountantDetail.UserId = user.Id;
                            AccountantDetail.CellNo = user.PhoneNumber;
                            AccountantDetail.PositionAppliedFor = workSheet.Cells[rowIterator, 6].Value.ToString();
                            AccountantDetail.DateAvailable = workSheet.Cells[rowIterator, 7].Value.ToString();
                            AccountantDetail.JoiningDate = workSheet.Cells[rowIterator, 8].Value.ToString();
                            AccountantDetail.BirthDate = workSheet.Cells[rowIterator, 9].Value.ToString();
                            AccountantDetail.Nationality = workSheet.Cells[rowIterator, 10].Value.ToString();
                            AccountantDetail.Religion = workSheet.Cells[rowIterator, 11].Value.ToString();
                            AccountantDetail.Gender = workSheet.Cells[rowIterator, 12].Value.ToString(); ;
                            AccountantDetail.MailingAddress = workSheet.Cells[rowIterator, 13].Value.ToString();
                            AccountantDetail.CellNo = workSheet.Cells[rowIterator, 14].Value.ToString();
                            AccountantDetail.Landline = workSheet.Cells[rowIterator, 15].Value.ToString();
                            AccountantDetail.SpouseName = workSheet.Cells[rowIterator, 16].Value.ToString();
                            AccountantDetail.SpouseHighestDegree = workSheet.Cells[rowIterator, 17].Value.ToString();
                            AccountantDetail.SpouseOccupation = workSheet.Cells[rowIterator, 18].Value.ToString();
                            AccountantDetail.SpouseBusinessAddress = workSheet.Cells[rowIterator, 19].Value.ToString();
                            AccountantDetail.Illness = workSheet.Cells[rowIterator, 20].Value.ToString();
                            AccountantDetail.GrossSalary = Convert.ToInt32(workSheet.Cells[rowIterator, 21].Value.ToString());
                            AccountantDetail.BasicSalary = Convert.ToInt32(workSheet.Cells[rowIterator, 22].Value.ToString());
                            AccountantDetail.MedicalAllowance = Convert.ToInt32(workSheet.Cells[rowIterator, 23].Value.ToString());
                            AccountantDetail.Accomodation = Convert.ToInt32(workSheet.Cells[rowIterator, 24].Value.ToString());
                            AccountantDetail.ProvidedFund = Convert.ToInt32(workSheet.Cells[rowIterator, 25].Value.ToString());
                            AccountantDetail.Tax = Convert.ToInt32(workSheet.Cells[rowIterator, 26].Value.ToString());
                            AccountantDetail.EOP = Convert.ToInt32(workSheet.Cells[rowIterator, 27].Value.ToString());
                            AccountantDetail.VirtualRoleId = db.AspNetVirtualRoles.Where(x => x.Name == "Management Staff").Select(x => x.Id).FirstOrDefault();
                            db.AspNetEmployees.Add(AccountantDetail);
                            db.SaveChanges();

                            var roleStore = new RoleStore<IdentityRole>(context);
                            var roleManager = new RoleManager<IdentityRole>(roleStore);
                            var userStore = new UserStore<ApplicationUser>(context);
                            var userManager = new UserManager<ApplicationUser>(userStore);
                            userManager.AddToRole(user.Id, "Accountant");
                        }
                        else
                        {
                            dbTransaction.Dispose();
                            AddErrors(result);
                            return View("AccountantRegister", model);
                        }

                    }
                    dbTransaction.Commit();
                    return RedirectToAction("AccountantIndex", "AspNetUser");
                }
            }
            catch (Exception)
            { dbTransaction.Dispose(); }

            return RedirectToAction("AccountantRegister", model);
        }

        /*******************************************************************************************************************
        * 
        *                                   Parent's Functions
        *                                    
        *******************************************************************************************************************/

        public ActionResult ParentRegister()
        {
            //    ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");

            ViewBag.ClassID = new SelectList(db.AspNetClasses.Where(x => x.SessionID == SessionID), "Id", "ClassName");
            return View();
        }

        public async Task<bool> SendMail_old(string toemail, string subject, string body)
        {
            bool IsMailSent = false;

            try
            {
                MailMessage msg = new MailMessage();
                //msg.To.Add(new MailAddress(toemail));
                msg.From = new MailAddress("azeemazeem187@gmail.com", "Officers Academy");
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;
                string ccMail = string.Empty;
                string bccMail = string.Empty;
                //ccMail = "azeemazeem187@gmail.com";
                //bccMail = "azeemazeem187@gmail.com";
                msg.To.Add(toemail);
                msg.Bcc.Add(bccMail);
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("azeemazeem187@gmail.com", "Zed#1@daimond.net");
                client.Port = 25; // You can use Port 25 if 587 is blocked (mine is!)
                client.Host = "smtp.gmail.com";
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                string userState = "call back object on success";
                await Task.Run(() => client.SendAsync(msg, userState));
                IsMailSent = true;
            }
            catch (Exception ex)
            {
                //    logAppException(ex.ToString(), "email send");
            }

            return IsMailSent;
        }

        public ActionResult ConfirmAccount(string id)
        {
            ViewBag.res = "error";
            var user = db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
            if (user != null)
            {
                if (user.Status == "True")
                {
                    ViewBag.res = "Hi " + user.UserName + "! Your Account is already activated";
                }
                else
                {
                    user.Status = "True";
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.res = "Hi " + user.UserName + "! Your Account has been activated";

                }
            }
            else
            {
                ViewBag.res = "Your Account can not be activated";

            }
            return View();
        }
        public ActionResult SendConformationEmail(string id)
        {
            string status = "error";
            try
            {
                string DomainName = Request.Url.GetLeftPart(UriPartial.Authority);
                var u = db.AspNetUsers.FirstOrDefault(x => x.Id == id);
                //  string ConfirmLink = "<a href/Security/EmailConfirmation/?ConfirmAccount={0}</a>";

                string ConfirmLink = "<a href='" + String.Format(DomainName + "/Parent_Dashboard/ConfirmAccount/?id={0}", u.Id.ToString() + "' target='_blank'>Click Here To Confirm Your Email </a>'");
                //SendMail(u.Email, "Account confirmation", "" + EmailDesign.SignupEmailTemplate(ConfirmLink, u.UserName));
                status = "Success";
            }
            catch (Exception ex)
            {
                //    logAppException(ex.StackTrace.ToString(), "Send Conformation Email");
            }
            return Content(status);
        }
        public ActionResult test()
        {
            SendConformationEmail("3e6f6b9b-e45b-4ee3-85bc-9848ba666fac");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ParentRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dbTransaction = db.Database.BeginTransaction();
                try
                {
                    ApplicationDbContext context = new ApplicationDbContext();
                    IEnumerable<string> selectedstudents = Request.Form["StudentID"].Split(',');
                    var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Name = model.Name, PhoneNumber = Request.Form["fatherCell"] };
                    //                    SendConformationEmail(user);

                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {

                        ruffdata rd = new ruffdata();
                        rd.SessionID = SessionID;
                        rd.StudentName = model.Name;
                        rd.StudentUserName = model.UserName;
                        rd.StudentPassword = model.Password;
                        db.ruffdatas.Add(rd);
                        db.SaveChanges();

                        AspNetParent parent = new AspNetParent();
                        parent.FatherName = Request.Form["fatherName"];
                        parent.FatherCellNo = Request.Form["fatherCell"];
                        parent.FatherEmail = Request.Form["fatherEmail"];
                        parent.FatherOccupation = Request.Form["fatherOccupation"];
                        parent.FatherEmployer = Request.Form["fatherEmployer"];
                        parent.MotherName = Request.Form["motherName"];
                        parent.MotherCellNo = Request.Form["motherCell"];
                        parent.MotherEmail = Request.Form["motherEmail"];
                        parent.MotherOccupation = Request.Form["motherOccupation"];
                        parent.MotherEmployer = Request.Form["motherEmployer"];
                        parent.UserID = user.Id;
                        db.AspNetParents.Add(parent);

                        AspNetUsers_Session US = new AspNetUsers_Session();
                        US.UserID = user.Id;
                        US.SessionID = SessionID;
                        db.AspNetUsers_Session.Add(US);
                        db.SaveChanges();

                        foreach (var item in selectedstudents)
                        {
                            AspNetParent_Child par_stu = new AspNetParent_Child();
                            par_stu.ChildID = item;
                            par_stu.ParentID = user.Id;
                            db.AspNetParent_Child.Add(par_stu);
                            db.SaveChanges();
                        }

                        var roleStore = new RoleStore<IdentityRole>(context);
                        var roleManager = new RoleManager<IdentityRole>(roleStore);

                        var userStore = new UserStore<ApplicationUser>(context);
                        var userManager = new UserManager<ApplicationUser>(userStore);
                        userManager.AddToRole(user.Id, "Parent");

                        dbTransaction.Commit();
                        SendConformationEmail(user.Id);
                    }
                    else
                    {
                        dbTransaction.Dispose();
                        AddErrors(result);
                        return View(model);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.InnerException);
                    dbTransaction.Dispose();
                    return View(model);
                }
            }
            string Error = "Parent successfully saved";
            return RedirectToAction("ParentsIndex", "AspNetUser", new { Error });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ParentEdit()
        {
            string id = Request.Form["Id"];
            var parent = db.AspNetParents.Where(x => x.UserID == id).Select(x => x).FirstOrDefault();
            var user = db.AspNetUsers.Where(x => x.Id == id).Select(x => x).FirstOrDefault();

            IEnumerable<string> selectedstudents = Request.Form["StudentID"].Split(',');
            user.UserName = Request.Form["UserName"];
            user.Name = Request.Form["Name"];
            user.Email = Request.Form["Email"];

            parent.FatherName = Request.Form["fatherName"];
            parent.FatherCellNo = Request.Form["fatherCell"];
            parent.FatherEmail = Request.Form["fatherEmail"];
            parent.FatherOccupation = Request.Form["fatherOccupation"];
            parent.FatherEmployer = Request.Form["fatherEmployer"];
            parent.MotherName = Request.Form["motherName"];
            parent.MotherCellNo = Request.Form["motherCell"];
            parent.MotherEmail = Request.Form["motherEmail"];
            parent.MotherOccupation = Request.Form["motherOccupation"];
            parent.MotherEmployer = Request.Form["motherEmployer"];

            var childs = db.AspNetParent_Child.Where(x => x.ParentID == user.Id).ToList();
            foreach (var item in childs)
            {
                db.AspNetParent_Child.Remove(item);
            }

            db.SaveChanges();

            db.AspNetUsers.Where(p => p.Id == id).FirstOrDefault().PhoneNumber = Request.Form["fatherCell"];
            db.SaveChanges();
            foreach (var item in selectedstudents)
            {
                AspNetParent_Child par_stu = new AspNetParent_Child();
                par_stu.ChildID = item;
                par_stu.ParentID = user.Id;
                db.AspNetParent_Child.Add(par_stu);
            }

            db.SaveChanges();

            return RedirectToAction("ParentIndex", "AspNetUser");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ParentRegisterFromFile(RegisterViewModel model)
        {
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["parents"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        ApplicationDbContext context = new ApplicationDbContext();
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var parent = new RegisterViewModel();
                            parent.Email = workSheet.Cells[rowIterator, 1].Value.ToString();
                            parent.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                            parent.UserName = workSheet.Cells[rowIterator, 3].Value.ToString();
                            parent.Password = workSheet.Cells[rowIterator, 4].Value.ToString();
                            parent.ConfirmPassword = workSheet.Cells[rowIterator, 5].Value.ToString();

                            var user = new ApplicationUser { UserName = parent.UserName, Email = parent.Email, Name = parent.Name };
                            var result = await UserManager.CreateAsync(user, parent.Password);
                            if (result.Succeeded)
                            {
                                AspNetParent parentDetail = new AspNetParent();
                                parentDetail.UserID = user.Id;
                                parentDetail.FatherName = workSheet.Cells[rowIterator, 6].Value.ToString();
                                parentDetail.FatherCellNo = workSheet.Cells[rowIterator, 7].Value.ToString();
                                parentDetail.FatherEmail = workSheet.Cells[rowIterator, 8].Value.ToString();
                                parentDetail.FatherOccupation = workSheet.Cells[rowIterator, 9].Value.ToString();
                                parentDetail.FatherEmployer = workSheet.Cells[rowIterator, 10].Value.ToString();
                                parentDetail.MotherName = workSheet.Cells[rowIterator, 11].Value.ToString();
                                parentDetail.MotherCellNo = workSheet.Cells[rowIterator, 12].Value.ToString();
                                parentDetail.MotherEmail = workSheet.Cells[rowIterator, 13].Value.ToString();
                                parentDetail.MotherOccupation = workSheet.Cells[rowIterator, 14].Value.ToString();
                                parentDetail.MotherEmployer = workSheet.Cells[rowIterator, 15].Value.ToString();
                                db.AspNetParents.Add(parentDetail);
                                db.SaveChanges();

                                var childUsernames = new List<string>();
                                childUsernames.Add(workSheet.Cells[rowIterator, 16].Value.ToString());
                                childUsernames.Add(workSheet.Cells[rowIterator, 17].Value.ToString());
                                childUsernames.Add(workSheet.Cells[rowIterator, 18].Value.ToString());
                                childUsernames.Add(workSheet.Cells[rowIterator, 19].Value.ToString());

                                var childIDs = (from student in db.AspNetUsers
                                                where childUsernames.Contains(student.UserName)
                                                select student.Id).ToList();
                                foreach (var item in childIDs)
                                {
                                    AspNetParent_Child par_stu = new AspNetParent_Child();
                                    par_stu.ChildID = item;
                                    par_stu.ParentID = user.Id;
                                    db.AspNetParent_Child.Add(par_stu);
                                    db.SaveChanges();
                                }

                                var roleStore = new RoleStore<IdentityRole>(context);
                                var roleManager = new RoleManager<IdentityRole>(roleStore);

                                var userStore = new UserStore<ApplicationUser>(context);
                                var userManager = new UserManager<ApplicationUser>(userStore);
                                userManager.AddToRole(user.Id, "Parent");

                            }
                            else
                            {
                                dbTransaction.Dispose();
                                AddErrors(result);
                                return View("ParentRegister", model);
                            }
                        }
                        dbTransaction.Commit();
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.InnerException);
                dbTransaction.Dispose();
                return View("ParentRegister", model);
            }
            return RedirectToAction("ParentIndex", "AspNetUser");
        }

        /*******************************************************************************************************************
         * 
         *                                    Teacher's Functions
         *                                    
         *******************************************************************************************************************/


        public ActionResult TeacherRegister()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult TeacherEdit(string id, HttpPostedFileBase image)
        {
            var user = db.AspNetUsers.Where(x => x.Id == id).Select(x => x).FirstOrDefault();

            if (image != null)
            {
                var fileName = Path.GetFileName(image.FileName);
                var extension = Path.GetExtension(image.FileName);
                image.SaveAs(Server.MapPath("~/Content/Images/StudentImages/") + image.FileName);
                //    file.SaveAs(Server.MapPath("/Upload/") + file.FileName);
            }

            user.Name = Request.Form["Name"];
            user.UserName = Request.Form["UserName"];
            user.Email = Request.Form["Email"];
            user.PhoneNumber = Request.Form["cellNo"];
            user.Image = image.FileName;

            var emp = db.AspNetEmployees.Where(x => x.UserId == id).Select(x => x).FirstOrDefault();

            emp.Name = user.Name;
            emp.UserName = user.UserName;
            emp.Email = user.Email;
            emp.PositionAppliedFor = Request.Form["appliedFor"];
            emp.DateAvailable = Request.Form["dateAvailable"];
            emp.JoiningDate = Request.Form["JoiningDate"];
            emp.BirthDate = Request.Form["birthDate"];
            emp.Nationality = Request.Form["nationality"];
            emp.Religion = Request.Form["religion"];
            emp.Gender = Request.Form["gender"];
            emp.MailingAddress = Request.Form["mailingAddress"];
            emp.CellNo = Request.Form["cellNo"];
            emp.Landline = Request.Form["landLine"];
            emp.SpouseName = Request.Form["spouseName"];
            emp.SpouseHighestDegree = Request.Form["spouseHighestDegree"];
            emp.SpouseOccupation = Request.Form["spouseOccupation"];
            emp.SpouseBusinessAddress = Request.Form["spouseBusinessAddress"];

            //emp.GrossSalary = Convert.ToInt32(Request.Form["GrossSalary"]);
            //emp.BasicSalary = Convert.ToInt32(Request.Form["BasicSalary"]);
            //emp.MedicalAllowance = Convert.ToInt32(Request.Form["MedicalAllowance"]);
            //emp.Accomodation = Convert.ToInt32(Request.Form["Accomodation"]);
            //emp.ProvidedFund = Convert.ToInt32(Request.Form["ProvidedFund"]);
            //emp.Tax = Convert.ToInt32(Request.Form["Tax"]);
            //emp.EOP = Convert.ToInt32(Request.Form["EOP"]);

            db.SaveChanges();


            //var ClassId = Convert.ToInt32(Request.Form["ClassId"]);
            //var SessionId = db.AspNetClasses.Where(x => x.Id == ClassId).FirstOrDefault().SessionID;

            //var UserSession = db.AspNetUsers_Session.Where(x => x.UserID == id).FirstOrDefault();
            //UserSession.SessionID = SessionId;
            //db.SaveChanges();

            //  int sessionid = db.AspNetSessions.Where(x => x.Status == "Active").FirstOrDefault().Id;


            //Aspnet_Employee_Session EmployeeSession = db.Aspnet_Employee_Session.Where(x => x.Emp_Id == emp.Id).FirstOrDefault();

            //EmployeeSession.Session_Id = SessionId;

            //db.SaveChanges();


            return RedirectToAction("TeachersIndex", "AspNetUser");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TeacherRegister(RegisterViewModel model, HttpPostedFileBase image)
        {
            var SessionIdOfSelectedStudent = db.AspNetSessions.Max(x=> x.Id);

            if (1 == 1)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Name = model.Name, PhoneNumber = Request.Form["cellNo"] };
                var result = await UserManager.CreateAsync(user, model.Password);

                var Field = Request.Form["Field"];
                var Uni = Request.Form["University"];
                var Ocu = Request.Form["Occupation"];
                var Deg = Request.Form["Degree"];
                var Ind = Request.Form["Industry"];
                ruffdata rd = new ruffdata();
                rd.SessionID = SessionIdOfSelectedStudent;
                rd.StudentName = model.Name;
                rd.StudentUserName = model.UserName;
                rd.StudentPassword = model.Password;

                AspNetUser Teacher = new AspNetUser();
                Teacher.Name = user.Name;
                Teacher.UserName = user.UserName;
                Teacher.Email = user.Email;
                Teacher.PasswordHash = user.PasswordHash;
                Teacher.Status = "Active";
                Teacher.PhoneNumber = Request.Form["cellNo"];

                AspNetEmployee emp = new AspNetEmployee();
                emp.Name = Teacher.Name;
                emp.UserName = Teacher.UserName;
                emp.Email = Teacher.Email;
                emp.PositionAppliedFor = Request.Form["appliedFor"];
                emp.DateAvailable = Request.Form["dateAvailable"];
                emp.JoiningDate = Request.Form["JoiningDate"];
                emp.BirthDate = Request.Form["birthDate"];
                emp.Nationality = Request.Form["nationality"];
                emp.Religion = Request.Form["religion"];
                emp.Gender = Request.Form["gender"];
                emp.MailingAddress = Request.Form["mailingAddress"];
                emp.CellNo = Request.Form["cellNo"];
                emp.Landline = Request.Form["landLine"];
                emp.SpouseName = Request.Form["spouseName"];
                emp.SpouseHighestDegree = Request.Form["spouseHighestDegree"];
                emp.SpouseOccupation = Request.Form["spouseOccupation"];
                emp.SpouseBusinessAddress = Request.Form["spouseBusinessAddress"];

                //emp.GrossSalary = Convert.ToInt32(Request.Form["GrossSalary"]);
                //emp.BasicSalary = Convert.ToInt32(Request.Form["BasicSalary"]);
                //emp.MedicalAllowance = Convert.ToInt32(Request.Form["MedicalAllowance"]);
                //emp.Accomodation = Convert.ToInt32(Request.Form["Accomodation"]);
                //emp.ProvidedFund = Convert.ToInt32(Request.Form["ProvidedFund"]);
                //emp.Tax = Convert.ToInt32(Request.Form["Tax"]);
                //emp.EOP = Convert.ToInt32(Request.Form["EOP"]);

                emp.VirtualRoleId = db.AspNetVirtualRoles.Where(x => x.Name == "Teaching Staff").Select(x => x.Id).FirstOrDefault();
                emp.UserId = user.Id;
                if (result.Succeeded)
                {

                    if (image != null)
                    {
                        var fileName = Path.GetFileName(image.FileName);
                        var extension = Path.GetExtension(image.FileName);
                        image.SaveAs(Server.MapPath("~/Content/Images/StudentImages/") + image.FileName);
                        //    file.SaveAs(Server.MapPath("/Upload/") + file.FileName);
                    }

                    db.ruffdatas.Add(rd);
                    db.SaveChanges();

                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);


                    userManager.AddToRole(user.Id, "Teacher");

                    db.AspNetEmployees.Add(emp);
                    if (db.SaveChanges() > 0)
                    {

                        AspNetUser usr = db.AspNetUsers.Find(emp.UserId);
                        usr.Industry = Ind;
                        usr.Occupation = Ocu;
                        usr.University = Uni;
                        usr.Highest_Degree = Deg;
                        usr.Field_Major = Field;
                        usr.Image = image.FileName;
                        //db.AspNetUsers.Add(usr);
                        db.SaveChanges();
                    }

                    AspNetUsers_Session US = new AspNetUsers_Session();
                    US.UserID = emp.UserId;
                    //  int sessionid = db.AspNetSessions.Where(x => x.Status == "Active").FirstOrDefault().Id;
                    US.SessionID = SessionIdOfSelectedStudent;
                    db.AspNetUsers_Session.Add(US);
                    if (db.SaveChanges() > 0)
                    {
                        Aspnet_Employee_Session aes = new Aspnet_Employee_Session();
                        aes.Emp_Id = emp.Id;
                        aes.Session_Id = SessionIdOfSelectedStudent;
                        db.Aspnet_Employee_Session.Add(aes);
                        db.SaveChanges();
                    }

                    string Error = "Teacher successfully saved.";
                    return RedirectToAction("TeacherIndex", "AspNetUser", new { Error });
                }
                else
                {
                    var remove = db.AspNetUsers.Where(x => x.Id == user.Id).FirstOrDefault();
                    db.AspNetUsers.Remove(remove);
                    db.SaveChanges();
                    AddErrors(result);
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TeacherfromFile(RegisterViewModel model)
        {
            // if (ModelState.IsValid)
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["teachers"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                }
                var teacherList = new List<RegisterViewModel>();
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    ApplicationDbContext context = new ApplicationDbContext();
                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {

                        string ErrorMsg;
                        bool ErrorMsgExist = false;
                        ErrorMsg = "Error in Row " + Convert.ToString(rowIterator - 1) + Environment.NewLine;

                        var teacher = new RegisterViewModel();

                        var Name = workSheet.Cells[rowIterator, 2].Value;

                        teacher.Email = workSheet.Cells[rowIterator, 1].Value.ToString();
                        teacher.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                        teacher.UserName = workSheet.Cells[rowIterator, 3].Value.ToString();
                        teacher.Password = workSheet.Cells[rowIterator, 4].Value.ToString();

                        //  string number = workSheet.Cells[rowIterator, 14].Value.ToString();

                        //var MailingAddress = workSheet.Cells[rowIterator, 6].Value.ToString();
                        //var CellNo = workSheet.Cells[rowIterator, 7].Value.ToString();
                        //var LandLine = workSheet.Cells[rowIterator, 8].Value.ToString();

                        var user = new ApplicationUser { UserName = teacher.UserName, Email = teacher.Email, Name = teacher.Name };
                        var result = await UserManager.CreateAsync(user, teacher.Password);
                        if (result.Succeeded)
                        {

                            ruffdata rd = new ruffdata();
                            rd.SessionID = SessionID;
                            rd.StudentName = teacher.Name;
                            rd.StudentUserName = teacher.UserName;
                            rd.StudentPassword = workSheet.Cells[rowIterator, 4].Value.ToString();
                            db.ruffdatas.Add(rd);
                            db.SaveChanges();


                            AspNetEmployee teacherDetail = new AspNetEmployee();
                            teacherDetail.Name = teacher.Name;
                            teacherDetail.Email = teacher.Email;
                            teacherDetail.UserName = teacher.UserName;
                            teacherDetail.UserId = user.Id;



                            var MailingAddress = workSheet.Cells[rowIterator, 5].Value;

                            if (MailingAddress == null)
                            {
                                teacherDetail.MailingAddress = "-";
                            }
                            else
                            {
                                teacherDetail.MailingAddress = workSheet.Cells[rowIterator, 5].Value.ToString();
                            }

                            var CellNo = workSheet.Cells[rowIterator, 6].Value;

                            if (CellNo == null)
                            {
                                teacherDetail.CellNo = "-";
                            }
                            else
                            {
                                teacherDetail.CellNo = workSheet.Cells[rowIterator, 6].Value.ToString();
                            }


                            // teacherDetail.Landline = workSheet.Cells[rowIterator, 8].Value.ToString();

                            var LandLine = workSheet.Cells[rowIterator, 7].Value;

                            if (LandLine == null)
                            {
                                teacherDetail.Landline = "-";
                            }
                            else
                            {
                                teacherDetail.Landline = workSheet.Cells[rowIterator, 7].Value.ToString();
                            }


                            teacherDetail.VirtualRoleId = db.AspNetVirtualRoles.Where(x => x.Name == "Teaching Staff").Select(x => x.Id).FirstOrDefault();
                            db.AspNetEmployees.Add(teacherDetail);
                            db.SaveChanges();

                            var roleStore = new RoleStore<IdentityRole>(context);
                            var roleManager = new RoleManager<IdentityRole>(roleStore);
                            var userStore = new UserStore<ApplicationUser>(context);
                            var userManager = new UserManager<ApplicationUser>(userStore);
                            userManager.AddToRole(user.Id, "Teacher");


                            if (db.SaveChanges() > 0)
                            {

                                AspNetUser usr = db.AspNetUsers.Find(user.Id);

                                //db.AspNetUsers.Add(usr);
                                db.SaveChanges();
                            }




                            AspNetUsers_Session US = new AspNetUsers_Session();
                            US.UserID = user.Id;
                            //  int sessionid = db.AspNetSessions.Where(x => x.Status == "Active").FirstOrDefault().Id;
                            US.SessionID = SessionID;
                            db.AspNetUsers_Session.Add(US);

                            if (db.SaveChanges() > 0)
                            {
                                Aspnet_Employee_Session aes = new Aspnet_Employee_Session();
                                aes.Emp_Id = teacherDetail.Id;
                                aes.Session_Id = SessionID;
                                db.Aspnet_Employee_Session.Add(aes);
                                db.SaveChanges();
                            }

                            //teacherDetail.PositionAppliedFor = workSheet.Cells[rowIterator, 6].Value.ToString();
                            //teacherDetail.DateAvailable = workSheet.Cells[rowIterator, 7].Value.ToString();
                            //teacherDetail.JoiningDate = workSheet.Cells[rowIterator, 8].Value.ToString();
                            //teacherDetail.BirthDate = workSheet.Cells[rowIterator, 9].Value.ToString();
                            //teacherDetail.Nationality = workSheet.Cells[rowIterator, 10].Value.ToString();
                            //teacherDetail.Religion = workSheet.Cells[rowIterator, 11].Value.ToString();
                            //teacherDetail.Gender = workSheet.Cells[rowIterator, 12].Value.ToString(); ;
                            //teacherDetail.SpouseName = workSheet.Cells[rowIterator, 16].Value.ToString();
                            //teacherDetail.SpouseHighestDegree = workSheet.Cells[rowIterator, 17].Value.ToString();
                            //teacherDetail.SpouseOccupation = workSheet.Cells[rowIterator, 18].Value.ToString();
                            //teacherDetail.SpouseBusinessAddress = workSheet.Cells[rowIterator, 19].Value.ToString();
                            //teacherDetail.Illness = workSheet.Cells[rowIterator, 20].Value.ToString();
                            //teacherDetail.GrossSalary = Convert.ToInt32(workSheet.Cells[rowIterator, 21].Value.ToString());
                            //teacherDetail.BasicSalary = Convert.ToInt32(workSheet.Cells[rowIterator, 22].Value.ToString());
                            //teacherDetail.MedicalAllowance = Convert.ToInt32(workSheet.Cells[rowIterator, 23].Value.ToString());
                            //teacherDetail.Accomodation = Convert.ToInt32(workSheet.Cells[rowIterator, 24].Value.ToString());
                            //teacherDetail.ProvidedFund = Convert.ToInt32(workSheet.Cells[rowIterator, 25].Value.ToString());
                            //teacherDetail.Tax = Convert.ToInt32(workSheet.Cells[rowIterator, 26].Value.ToString());
                            //teacherDetail.EOP = Convert.ToInt32(workSheet.Cells[rowIterator, 27].Value.ToString());
                        }
                        else
                        {
                            dbTransaction.Dispose();
                            AddErrors(result);
                            return View("TeacherRegister", model);
                        }

                    }
                    dbTransaction.Commit();
                }
            }
            catch (Exception e)
            {
                //   ModelState.AddModelError("Error", e.InnerException);
                dbTransaction.Dispose();
                return View("TeacherRegister", model);

            }
            return RedirectToAction("TeachersIndex", "AspNetUser");
        }

        public ActionResult BiometricRegistration(string RollNo, string Success)
        {
            ViewBag.RollNo = RollNo;
            ViewBag.Success = Success;

            return View();
        }

        public ActionResult CheckVerifiction(string RollNo)
        {

            AspNetUser User = db.AspNetUsers.Where(x => x.UserName == RollNo).FirstOrDefault();


            string VerifictionMsg = "";

            if (User != null)
            {

                if (User.FingerPrintCode != null)
                {
                    VerifictionMsg = "Yes";
                }
                else
                {
                    VerifictionMsg = "No";
                }

            }


            return Json(VerifictionMsg, JsonRequestBehavior.AllowGet);



            return View();
        }



        /*******************************************************************************************************************
         * 
         *                                    Student's Functions
         *                                    
         *******************************************************************************************************************/
        public ActionResult StudentRegister()
        {
            //var data = db.AspNetClasses 
            // ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SessionFee = db.AspNetSessions.Where(x => x.Id == SessionID).FirstOrDefault().Total_Fee;
            // ViewBag.ClassID2 = db.AspNetClasses.Where(x => x.SessionID == SessionID).FirstOrDefault();
            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentRegister(RegisterViewModel model, HttpPostedFileBase image)
        {
            Random rndm = new Random();
            int dom = rndm.Next(10000, 99999);

            string pass = "Oa" + "@" + dom;

            // var newpassword = 
            //string passowrd = newpassword.Substring(0, 7);
            model.Password = pass;
            model.ConfirmPassword = pass;

            // Discount
            int? SessionIdOfSelectedStudent = db.AspNetClasses.Where(x => x.Id == model.ClassID).FirstOrDefault().SessionID;

            var Discount = Request.Form["Discount"];
            double discount = 0;
            if (Request.Form["Discount"] == "")
            {
                discount = 0;
            }
            else
            {
                discount = Convert.ToDouble(Request.Form["Discount"]);
            }

            var age = 0;
            if (Request.Form["Age"] != "")
            {

                age = Convert.ToInt32(Request.Form["Age"]);

            }
            model.UserName = Request.Form["UserName"];

            if (model.Email == null)
            {

                model.Email = "oa" + model.UserName + "@gmail.com";

            }

            var dbTransaction = db.Database.BeginTransaction();
            try
            {

                if (ModelState.IsValid)
                {

                    if (image != null)
                    {
                        var fileName = Path.GetFileName(image.FileName);
                        var extension = Path.GetExtension(image.FileName);
                        image.SaveAs(Server.MapPath("~/Content/Images/StudentImages/") + image.FileName);
                        //    file.SaveAs(Server.MapPath("/Upload/") + file.FileName);
                    }


                    ApplicationDbContext context = new ApplicationDbContext();
                    List<string> selectedsubjects = new List<string>();

                    var CourseType = Request.Form["CourseType"];
                    int AllNullCSSSubjects = 0;

                    if (CourseType == "CSS")
                    {

                        for (int i = 0; i <= 7; i++)
                        {
                            var Subject = "CSSSubjects" + i;
                            if (Request.Form[Subject] != null)
                            {

                                selectedsubjects.AddRange(Request.Form[Subject].Split(',').ToList());
                            }
                            else
                            {
                                AllNullCSSSubjects = AllNullCSSSubjects + 1;
                            }

                        }

                        if (AllNullCSSSubjects == 8)
                        {
                            ViewBag.SubjectsErrorMsg = "Please Select at least one Subject";
                            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
                            ViewBag.SessionFee = db.AspNetSessions.Where(x => x.Id == SessionID).FirstOrDefault().Total_Fee;
                            return View(model);
                        }
                    }


                    else if (CourseType == "PMS")
                    {
                        for (int i = 0; i <= 5; i++)
                        {
                            var Subject = "PMSSubjects" + i;
                            if (Request.Form[Subject] != null)
                            {
                                selectedsubjects.AddRange(Request.Form[Subject].Split(',').ToList());
                            }
                            else
                            {
                                AllNullCSSSubjects = AllNullCSSSubjects + 1;
                            }

                        }

                        if (AllNullCSSSubjects == 6)
                        {
                            ViewBag.SubjectsErrorMsg = "Please Select at least one Subject";
                            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
                            ViewBag.SessionFee = db.AspNetSessions.Where(x => x.Id == SessionID).FirstOrDefault().Total_Fee;
                            return View(model);
                        }
                    }
                    else if (CourseType == "One Paper MCQs")
                    {
                        var Subject = "OnePaperSubjects0";

                        if (Request.Form[Subject] != null)
                        {
                            selectedsubjects.AddRange(Request.Form[Subject].Split(',').ToList());
                        }
                        else
                        {
                            AllNullCSSSubjects = AllNullCSSSubjects + 1;
                        }                        

                        if (AllNullCSSSubjects == 1)
                        {
                            ViewBag.SubjectsErrorMsg = "Please Select at least one Subject";
                            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
                            ViewBag.SessionFee = db.AspNetSessions.Where(x => x.Id == SessionID).FirstOrDefault().Total_Fee;
                            return View(model);
                        }
                    }

                    List<string> listofIDs = selectedsubjects.ToList();
                    List<int> myIntegersSubjectsList = listofIDs.Select(s => int.Parse(s)).ToList();

                    var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Name = model.Name, PhoneNumber = Request.Form["cellNo"] };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        SendMail44(model.Email, "Admission Confirmed", "" + EmailDesign.SignupEmailTemplate(model.Name, model.UserName, model.Password));
                        
                        ruffdata rd = new ruffdata();
                        rd.SessionID = SessionIdOfSelectedStudent;
                        rd.StudentName = model.Name;
                        rd.StudentUserName = model.UserName;
                        rd.StudentPassword = model.Password;
                        db.ruffdatas.Add(rd);
                        db.SaveChanges();

                        AspNetStudent student = new AspNetStudent();

                        student.StudentID = user.Id;

                        student.Age = age;


                        student.Address = Request.Form["Address"];

                        student.ClassTimings = Request.Form["ClassTimings"];
                        student.Fathers_Name = Request.Form["Father_Name"];
                        student.SchoolName = Request.Form["SchoolName"];
                        student.CourseType = Request.Form["CourseType"];
                        //  student.BirthDate = Request.Form["BirthDate"];
                        // student.Nationality = Request.Form["Nationality"];
                        student.Religion = Request.Form["Religion"];
                        //   student.Gender = Request.Form["Gender"];

                        student.CreationDate = DateTime.Now;
                        if (image != null)
                        {
                            student.StudentIMG = image.FileName;

                        }
                        student.ClassID = Convert.ToInt32(Request.Form["ClassID"]);
                        var Field = Request.Form["Field"];
                        var Uni = Request.Form["University"];
                        var Ocu = Request.Form["Occupation"];
                        var Deg = Request.Form["Degree"];
                        var Ind = Request.Form["Industry"];
                        db.AspNetStudents.Add(student);
                        db.SaveChanges();

                        AspNetStudent_Session_class asc = new AspNetStudent_Session_class();
                        asc.ClassID = student.ClassID;
                        Aspnet_Employee_Session ES = new Aspnet_Employee_Session();
                        //  int sessionid = db.AspNetSessions.Where(x => x.Status == "Active").FirstOrDefault().Id;
                        asc.SessionID = SessionIdOfSelectedStudent;
                        asc.StudentID = student.Id;
                        db.AspNetStudent_Session_class.Add(asc);

                        if (db.SaveChanges() > 0)
                        {

                            AspNetUsers_Session AS = new AspNetUsers_Session();
                            AS.UserID = student.StudentID;
                            AS.SessionID = SessionIdOfSelectedStudent;
                            db.AspNetUsers_Session.Add(AS);
                            db.SaveChanges();
                        }

                        // var subID = selectedsubjects.First();
                        //  var classID = db.AspNetSubjects.Where(x=> x.Id == int.Parse(subID)).Select(x=> x.ClassID).FirstOrDefault();
                        //  int id = Int32.Parse(subID);
                        //  var c_id = db.AspNetSubjects.Where(x => x.Id == id).FirstOrDefault().ClassID;
                        //var subjects = db.AspNetSubjects.Where(x => x.ClassID == c_id && x.IsManadatory == true && x.CourseType == student.CourseType).Select(x => x.Id);

                        //foreach (var item in subjects)
                        //{

                        //    AspNetStudent_Subject stu_sub = new AspNetStudent_Subject();
                        //    stu_sub.StudentID = user.Id;
                        //    stu_sub.SubjectID = Convert.ToInt32(item);
                        //    db.AspNetStudent_Subject.Add(stu_sub);
                        //    db.SaveChanges();
                        //}
                        if (selectedsubjects != null)
                        {

                            foreach (var item in selectedsubjects)
                            {
                                AspNetStudent_Subject stu_sub = new AspNetStudent_Subject();
                                stu_sub.StudentID = user.Id;
                                stu_sub.SubjectID = Convert.ToInt32(item);
                                db.AspNetStudent_Subject.Add(stu_sub);
                                db.SaveChanges();
                            }
                        }

                        if (selectedsubjects != null)
                        {
                            var AllSubjectsOfAStudent = from subject in db.AspNetSubjects
                                                        where myIntegersSubjectsList.Contains(subject.Id)
                                                        select subject;


                            foreach (var sub in AllSubjectsOfAStudent)
                            {

                                foreach (var sub1 in db.GenericSubjects.ToList())
                                {

                                    if (sub.SubjectName == sub1.SubjectName && sub.CourseType == sub1.SubjectType)
                                    {

                                        Student_GenericSubjects genericSubject = new Student_GenericSubjects();

                                        genericSubject.GenericSubjectId = sub1.Id;
                                        genericSubject.StudentId = student.StudentID;

                                        db.Student_GenericSubjects.Add(genericSubject);
                                        db.SaveChanges();
                                    }

                                }
                            }
                        }

                        var roleStore = new RoleStore<IdentityRole>(context);
                        var roleManager = new RoleManager<IdentityRole>(roleStore);

                        var userStore = new UserStore<ApplicationUser>(context);
                        var userManager = new UserManager<ApplicationUser>(userStore);
                        userManager.AddToRole(user.Id, "Student");

                        if (image != null)
                        {
                            AspNetUser usr = db.AspNetUsers.Find(user.Id);
                            usr.Image = image.FileName;
                            db.SaveChanges();
                        }

                        //db.AspNetUsers.Add(usr);
                        

                        dbTransaction.Commit();
                        string Error = "Student successfully saved.";

                        StudentFeeMonth studentFeeMonth = new StudentFeeMonth();

                        string NotesCategory = Request.Form["NotesCategory"];
                        string FeeType = Request.Form["FeeType"];
                        double NotesFee = 0;
                        double Total = Convert.ToDouble(Request.Form["SessionFee"]);
                        double studentFee = Convert.ToDouble(Request.Form["SessionFee"]);


                        if (FeeType == "Installment")
                        {

                            Total = Total + 6000;
                            studentFee = studentFee + 6000;
                        }
                        else if (FeeType == "PerMonth")
                        {
                            Total = Total + 12000;

                            studentFee = studentFee + 12000;
                        }
                        else
                        {
                            Total = Total + 0;
                            studentFee = studentFee + 0;
                        }

                        if (NotesCategory == "WithNotes")
                        {

                            NotesFee = Convert.ToDouble(Request.Form["NotesAmount"]);
                            studentFeeMonth.TotalFee = Total + Convert.ToDouble(Request.Form["NotesAmount"]);
                            studentFeeMonth.NotesFee = Convert.ToDouble(Request.Form["NotesAmount"]);

                        }

                        else
                        {
                            studentFeeMonth.TotalFee = Total;
                        }




                        //DateTime ConvertIssueDate = Convert.ToDateTime(Request.Form["IssueDate"]);
                        //var Month = ConvertIssueDate.ToString("MMMM");



                        //  studentFeeMonth.Months = Month;
                        // studentFeeMonth.IssueDate = Convert.ToDateTime(Request.Form["IssueDate"]);

                        studentFeeMonth.IssueDate = DateTime.Now;
                        var Month = DateTime.Now.ToString("MMMM");
                        studentFeeMonth.FeePayable = Convert.ToDouble(Request.Form["TotalFee"]);
                        studentFeeMonth.Discount = discount;
                        studentFeeMonth.FeeType = Request.Form["FeeType"];
                        studentFeeMonth.SessionId = SessionIdOfSelectedStudent;
                        studentFeeMonth.StudentId = student.Id;
                        studentFeeMonth.Status = "Pending";
                        studentFeeMonth.DueDate = Convert.ToDateTime(Request.Form["DueDate"]);

                        db.StudentFeeMonths.Add(studentFeeMonth);
                        db.SaveChanges();

                        var id = User.Identity.GetUserId();
                        var username = db.AspNetUsers.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
                        Voucher voucher = new Voucher();
                        var SessionName = db.AspNetSessions.Where(x => x.Id == SessionIdOfSelectedStudent).FirstOrDefault().SessionName;
                        voucher.Name = "Student Fee Creation of student " + model.Name + " Session Name " + SessionName; ;
                        voucher.Notes = "Account Receiveable, discount, and revenue is updated";
                        voucher.Date = GetLocalDateTime.GetLocalDateTimeFunction();
                        voucher.StudentId = student.Id;

                        voucher.CreatedBy = username;
                        voucher.SessionID = SessionIdOfSelectedStudent;
                        int? VoucherObj = db.Vouchers.Max(x => x.VoucherNo);

                        voucher.VoucherNo = Convert.ToInt32(VoucherObj) + 1;
                        db.Vouchers.Add(voucher);

                        db.SaveChanges();

                        var Leadger = db.Ledgers.Where(x => x.Name == "Account Receiveable").FirstOrDefault();
                        int AdminDrawerId = Leadger.Id;
                        decimal? CurrentBalance = Leadger.CurrentBalance;

                        VoucherRecord voucherRecord = new VoucherRecord();
                        decimal? AfterBalance = CurrentBalance + Convert.ToDecimal(studentFeeMonth.FeePayable);
                        voucherRecord.LedgerId = AdminDrawerId;
                        voucherRecord.Type = "Dr";
                        voucherRecord.Amount = Convert.ToDecimal(studentFeeMonth.FeePayable);
                        voucherRecord.CurrentBalance = CurrentBalance;

                        voucherRecord.AfterBalance = AfterBalance;
                        voucherRecord.VoucherId = voucher.Id;

                        voucherRecord.Description = "Fee added of student (" + model.Name + ") (" + SessionName + ")";
                        Leadger.CurrentBalance = AfterBalance;
                        db.VoucherRecords.Add(voucherRecord);
                        db.SaveChanges();

                        if (NotesCategory == "WithNotes")
                        {
                            VoucherRecord voucherRecord1 = new VoucherRecord();

                            var LeadgerNotes = db.Ledgers.Where(x => x.Name == "Notes").FirstOrDefault();

                            decimal? CurrentBalanceOfNotes = LeadgerNotes.CurrentBalance;
                            decimal? AfterBalanceOfNotes = CurrentBalanceOfNotes + Convert.ToDecimal(NotesFee);
                            voucherRecord1.LedgerId = LeadgerNotes.Id;
                            voucherRecord1.Type = "Cr";
                            voucherRecord1.Amount = Convert.ToDecimal(NotesFee);
                            voucherRecord1.CurrentBalance = CurrentBalanceOfNotes;
                            voucherRecord1.AfterBalance = AfterBalanceOfNotes;
                            voucherRecord1.VoucherId = voucher.Id;
                            voucherRecord1.Description = "Notes against student with notes of (" + model.Name + ") (" + SessionName + ")";
                            LeadgerNotes.CurrentBalance = AfterBalanceOfNotes;

                            db.VoucherRecords.Add(voucherRecord1);
                            db.SaveChanges();
                        }

                        VoucherRecord voucherRecord2 = new VoucherRecord();

                        var IdofLedger = from Ledger in db.Ledgers
                                         join LedgerHd in db.LedgerHeads on Ledger.LedgerHeadId equals LedgerHd.Id
                                         where LedgerHd.Name == "Income" && Ledger.Name == "Student Fee"
                                         select new
                                         {
                                             Ledger.Id

                                         };

                        int studentFeeId = Convert.ToInt32(IdofLedger.FirstOrDefault().Id);
                        var studentFeeL = db.Ledgers.Where(x => x.Id == studentFeeId).FirstOrDefault();

                        decimal? CurrentBalanceOfStudentFee = studentFeeL.CurrentBalance;
                        decimal? AfterBalanceOfStudentFee = CurrentBalanceOfStudentFee + Convert.ToDecimal(studentFee);
                        voucherRecord2.LedgerId = studentFeeL.Id;
                        voucherRecord2.Type = "Cr";
                        voucherRecord2.Amount = Convert.ToDecimal(studentFee);
                        voucherRecord2.CurrentBalance = CurrentBalanceOfStudentFee;
                        voucherRecord2.AfterBalance = AfterBalanceOfStudentFee;
                        voucherRecord2.VoucherId = voucher.Id;
                        // voucherRecord2.Description = "Credit in Student Fee(income)";
                        voucherRecord2.Description = "Fee added of student (" + model.Name + ") (" + SessionName + ")";
                        studentFeeL.CurrentBalance = AfterBalanceOfStudentFee;
                        db.VoucherRecords.Add(voucherRecord2);

                        db.SaveChanges();
                        if (discount != 0)
                        {

                            VoucherRecord voucherRecord3 = new VoucherRecord();

                            var LeadgerDiscount = db.Ledgers.Where(x => x.Name == "Discount").FirstOrDefault();

                            decimal? CurrentBalanceOfDiscount = LeadgerDiscount.CurrentBalance;
                            decimal? AfterBalanceOfDiscount = CurrentBalanceOfDiscount + Convert.ToDecimal(discount);
                            voucherRecord3.LedgerId = LeadgerDiscount.Id;
                            voucherRecord3.Type = "Dr";
                            voucherRecord3.Amount = Convert.ToDecimal(discount);
                            voucherRecord3.CurrentBalance = CurrentBalanceOfDiscount;
                            voucherRecord3.AfterBalance = AfterBalanceOfDiscount;
                            voucherRecord3.VoucherId = voucher.Id;
                            voucherRecord3.Description = "Discount given to student  (" + model.Name + ") (" + SessionName + ") on payable fee " + Convert.ToDouble(Request.Form["TotalFee"]);
                            LeadgerDiscount.CurrentBalance = AfterBalanceOfDiscount;

                            db.VoucherRecords.Add(voucherRecord3);
                            db.SaveChanges();
                        }

                        //    return RedirectToAction("BiometricRegistration", "Admin_Dashboard", new { RollNo = model.UserName, Success = Error });

                        return RedirectToAction("StudentIndex", "AspNetUser", new { Error });

                    }
                    else
                    {
                        dbTransaction.Dispose();
                        AddErrors(result);
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = e.Message;
                dbTransaction.Dispose();
                ModelState.AddModelError("", e.Message);
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View(model);
        }
        //public ActionResult GetMandatorySubjects(int ClassId)
        //{
        //   var MandatorySubjectsList =  db.AspNetSubjects.Where(x => x.ClassID == ClassId && x.IsManadatory == true).Select(x => x.Id);
        //   var AllSubjectList = db.AspNetSubjects.Where(x=>x.ClassID == ClassId).Select(x => x.Id);



        //    return Json(new { MandatorySubjects = MandatorySubjectsList , AllSubjects = AllSubjectList }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetSessionFee(int SelectedClassId)
        {
            int? SessionId = db.AspNetClasses.Where(x => x.Id == SelectedClassId).FirstOrDefault().SessionID;

            var SessionFee = db.AspNetSessions.Where(x => x.Id == SessionId).FirstOrDefault().Total_Fee;

            if (SessionFee == null)
            {
                SessionFee = 0;
            }

            return Json(SessionFee, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentfromFile(RegisterViewModel model)
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            // if (ModelState.IsValid)



            var dbTransaction = db.Database.BeginTransaction();
            try
            {


                HttpPostedFileBase file = Request.Files["students"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                }
                var studentList = new List<RegisterViewModel>();
                using (var package = new ExcelPackage(file.InputStream))
                {
                    string ErrorMsg = "";
                    bool ErrorMsgExist = false;
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    List<ExcelSheetError> ExcelErrors = new List<ExcelSheetError>();

                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        var checktotalrows = rowIterator;
                        var student = new RegisterViewModel();
                        var Email = workSheet.Cells[rowIterator, 1].Value;
                        var RowNumber = rowIterator - 1;

                        ErrorMsg = ErrorMsg + Environment.NewLine + "Error in Row " + Convert.ToString(rowIterator - 1) + Environment.NewLine;

                        if (Email != null)
                        {
                            string EmailInString = workSheet.Cells[rowIterator, 1].Value.ToString();
                            var EmailContains = db.AspNetUsers.Where(x => x.Email == EmailInString);

                            if (EmailContains.Count() > 0)
                            {

                                // ExcelSheetError ExcelError = new ExcelSheetError();


                                ErrorMsg = ErrorMsg + "Email already Exist" + Environment.NewLine;

                                ExcelErrors.Add(new ExcelSheetError { Row = RowNumber, Message = "Error", Detail = EmailInString + " already Exist" });

                                ErrorMsgExist = true;
                                //   TempData["ErrorMsg"] = ErrorMsg;
                                //  return RedirectToAction("StudentRegister");
                            }

                            else
                            {
                                student.Email = EmailInString;

                            }

                        }
                        else
                        {
                            ErrorMsg = ErrorMsg + "Email is Required" + Environment.NewLine;
                            ExcelErrors.Add(new ExcelSheetError { Row = RowNumber, Message = "Error", Detail = "Email is Required" });

                            ErrorMsgExist = true;
                            //return RedirectToAction("StudentRegister");
                        }


                        // student.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                        var Name = workSheet.Cells[rowIterator, 2].Value;
                        if (Name != null)
                        {

                            student.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                        }
                        else
                        {
                            ExcelErrors.Add(new ExcelSheetError { Row = RowNumber, Message = "Error", Detail = "Name is Required" });

                            ErrorMsg = "Name is Required" + Environment.NewLine;
                            ErrorMsgExist = true;
                        }

                        // student.UserName = workSheet.Cells[rowIterator, 3].Value.ToString();
                        var UserName = workSheet.Cells[rowIterator, 3].Value;

                        if (UserName != null)
                        {
                            string UserNameInString = workSheet.Cells[rowIterator, 3].Value.ToString();
                            var UserContains = db.AspNetUsers.Where(x => x.UserName == UserNameInString);

                            if (UserContains.Count() > 0)
                            {
                                ErrorMsg = ErrorMsg + "UserName already Exist" + Environment.NewLine;
                                ErrorMsgExist = true;
                                ExcelErrors.Add(new ExcelSheetError { Row = RowNumber, Message = "Error", Detail = UserNameInString + "already Exist" });

                                TempData["ErrorMsg"] = ErrorMsg;

                            }

                            else
                            {
                                student.UserName = UserNameInString;

                            }
                        }
                        else
                        {
                            ErrorMsg = ErrorMsg + "UserName is Required" + Environment.NewLine;
                            ExcelErrors.Add(new ExcelSheetError { Row = RowNumber, Message = "Error", Detail = "UserName is Required" });

                            ErrorMsgExist = true;

                        }

                        //Regex regex = new Regex(@"^(?=.{6,})(?=.*[a-z])(?=.*[0-9])(?=.*[A-Z])(?=.*[@@#$%^&+=]).*$");
                        //Match match = regex.Match(email);

                        var Password = workSheet.Cells[rowIterator, 4].Value;
                        var ConfirmPassword = workSheet.Cells[rowIterator, 5].Value;

                        if (Password != null)
                        {
                            if (ConfirmPassword != null)
                            {
                                var compare = String.Equals(Password.ToString(), ConfirmPassword.ToString());

                                if (compare == false)
                                {

                                    ErrorMsg = ErrorMsg + "password and confirm password does not match" + Environment.NewLine;
                                    ErrorMsgExist = true;

                                }

                            }

                        }
                        else
                        {
                            ErrorMsg = ErrorMsg + "Password is Required" + Environment.NewLine;
                            ErrorMsgExist = true;

                        }
                        student.Password = workSheet.Cells[rowIterator, 4].Value.ToString();

                        student.ConfirmPassword = workSheet.Cells[rowIterator, 5].Value.ToString();
                        var ClassName = workSheet.Cells[rowIterator, 6].Value;


                        string Class = "";
                        bool ClassExist = false;


                        if (ClassName != null)
                        {
                            string ClassNameInString = workSheet.Cells[rowIterator, 6].Value.ToString().Trim();
                            var ClassContains = db.AspNetClasses.Where(x => x.ClassName == ClassNameInString).FirstOrDefault();


                            if (ClassContains == null)
                            {
                                ErrorMsg = ErrorMsg + "Class not Exist in database" + Environment.NewLine;
                                ErrorMsgExist = true;

                            }

                            else
                            {
                                Class = ClassNameInString;
                                ClassExist = true;
                            }



                        }
                        else
                        {
                            ErrorMsg = ErrorMsg + "ClassName is Required" + Environment.NewLine;
                            ErrorMsgExist = true;

                        }

                        var subjects = new List<string>();
                        List<object> subjectsobjects = new List<object>();
                        subjectsobjects.Add(workSheet.Cells[rowIterator, 7].Value);
                        subjectsobjects.Add(workSheet.Cells[rowIterator, 8].Value);
                        subjectsobjects.Add(workSheet.Cells[rowIterator, 9].Value);
                        subjectsobjects.Add(workSheet.Cells[rowIterator, 10].Value);
                        subjectsobjects.Add(workSheet.Cells[rowIterator, 11].Value);
                        subjectsobjects.Add(workSheet.Cells[rowIterator, 12].Value);
                        subjectsobjects.Add(workSheet.Cells[rowIterator, 13].Value);
                        subjectsobjects.Add(workSheet.Cells[rowIterator, 14].Value);
                        subjectsobjects.Add(workSheet.Cells[rowIterator, 15].Value);
                        subjectsobjects.Add(workSheet.Cells[rowIterator, 16].Value);

                        bool SubjectExistInExcelFile = false;
                        foreach (object subject in subjectsobjects)
                        {
                            if (subject != null)
                            {

                                subjects.Add(subject.ToString().ToLower());
                                SubjectExistInExcelFile = true;
                            }

                        }

                        if (!SubjectExistInExcelFile)
                        {
                            ErrorMsg = ErrorMsg + "At Least One Subject Needed" + Environment.NewLine;
                            ErrorMsgExist = true;

                        }


                        //  var user = new { Id = "001a670e-7c8d-44d3-9972-ce92160ed643" };
                        if (ClassExist == true)
                        {

                            int ClassId = db.AspNetClasses.Where(x => x.ClassName == Class).FirstOrDefault().Id;

                            List<AspNetSubject> ListSubjects = db.AspNetSubjects.Where(x => x.ClassID == ClassId).Distinct().ToList();
                            var ListSubjectLower = ListSubjects.Select(x => x.SubjectName.ToLower());

                            bool SubjectExist = true;
                            if (subjects.Count > 0)
                            {


                                foreach (var subject in subjects)
                                {
                                    foreach (var sub in ListSubjectLower)
                                    {
                                        if (sub != subject)
                                        {
                                            SubjectExist = false;


                                        }
                                        else
                                        {
                                            SubjectExist = true;

                                            break;

                                        }

                                    }
                                    if (SubjectExist == false)
                                    {
                                        ExcelErrors.Add(new ExcelSheetError { Row = RowNumber, Message = "Error", Detail = subject + " not Exist" });


                                    }
                                }


                                //if (SubjectExist == false)
                                //{
                                //    ErrorMsg = ErrorMsg + Class + "Dont have all these subjects" + Environment.NewLine;
                                //    ErrorMsgExist = true;

                                //}

                            }
                        }

                        //var ClassTimings = workSheet.Cells[rowIterator, 23];

                        //if (ClassTimings != null)
                        //{
                        //    var ClassTimingInString = ClassTimings.ToString();

                        //    if (!ClassTimingInString.Equals("Morning") || !ClassTimingInString.Equals("Evening"))
                        //    {
                        //        ErrorMsg = ErrorMsg + "Class Timing is not Correct" + Environment.NewLine;

                        //    }
                        //}



                        var CourseType = workSheet.Cells[rowIterator, 24].Value;
                        if (CourseType != null)
                        {
                            var CourseTypeInString = CourseType.ToString().ToLower();

                            if (CourseTypeInString == "css" || CourseTypeInString == "pms" || CourseTypeInString == "one paper")
                            {
                            }
                            else
                            {
                                ErrorMsg = ErrorMsg + "Course Type is not Correct " + Environment.NewLine;
                                ErrorMsgExist = true;
                            }

                        }
                        else
                        {
                            ErrorMsg = ErrorMsg + "Course Type is Required" + Environment.NewLine;
                            ErrorMsgExist = true;
                        }


                        var IsExistNotesCategory = false;
                        var NotesCategory = workSheet.Cells[rowIterator, 25].Value;

                        if (NotesCategory != null)
                        {
                            var NotesCategoryInString = NotesCategory.ToString().ToLower();

                            if (NotesCategoryInString == "with notes" || NotesCategoryInString == "without notes")
                            {

                                IsExistNotesCategory = true;

                            }
                            else
                            {
                                ErrorMsg = ErrorMsg + "Notes Category is not Correct " + Environment.NewLine;
                                ErrorMsgExist = true;


                            }

                        }
                        else
                        {
                            ErrorMsg = ErrorMsg + "Notes Category is Required" + Environment.NewLine;
                            ErrorMsgExist = true;

                        }



                        var NotesFee = workSheet.Cells[rowIterator, 30].Value;
                        if (IsExistNotesCategory == true)
                        {

                            if (NotesFee != null)
                            {
                                if (NotesCategory.ToString().ToLower() == "with notes")
                                {

                                    var NotesFeeInString = NotesFee.ToString();

                                    var isNumeric = NotesFeeInString.All(char.IsDigit);
                                    if (isNumeric == false)
                                    {
                                        ErrorMsg = ErrorMsg + "Notes Fee should be in numeric form" + Environment.NewLine;
                                        ErrorMsgExist = true;


                                    }
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                    ErrorMsg = ErrorMsg + "Note Fee is Not Required because Notes Category is Without Notes" + Environment.NewLine;
                                    ErrorMsgExist = true;

                                }

                            }
                            else
                            {

                                if (NotesCategory.ToString().ToLower() == "with notes")
                                {
                                    ErrorMsg = ErrorMsg + "Notes Category is required" + Environment.NewLine;
                                    ErrorMsgExist = true;


                                }

                            }

                        }

                        var FeeType = workSheet.Cells[rowIterator, 28].Value;

                        if (FeeType != null)
                        {
                            var FeeTypeInString = FeeType.ToString().ToLower();

                            if (FeeTypeInString == "installment" || FeeTypeInString == "per month" || FeeTypeInString == "full fee")
                            {

                            }
                            else
                            {
                                ErrorMsg = ErrorMsg + "Fee Type is not Correct " + Environment.NewLine;
                                ErrorMsgExist = true;

                            }


                        }
                        else
                        {
                            ErrorMsg = ErrorMsg + "Fee Type is Required" + Environment.NewLine;
                            ErrorMsgExist = true;

                        }


                        var SessionFee = workSheet.Cells[rowIterator, 26].Value;
                        if (SessionFee != null)
                        {

                            if (ClassExist == true)
                            {

                                var ClassId1 = db.AspNetClasses.Where(x => x.ClassName == Class).Select(x => x.Id).FirstOrDefault();
                                var SessionIdOfCurrentStudent = db.AspNetClasses.Where(x => x.Id == ClassId1).FirstOrDefault().SessionID;

                                var SessionFeeofCurrentSession = db.AspNetSessions.Where(x => x.Id == SessionIdOfCurrentStudent).FirstOrDefault().Total_Fee;
                                var SessionFeeInString = SessionFee.ToString();


                                var isNumeric = SessionFeeInString.All(char.IsDigit);
                                if (isNumeric == false)
                                {
                                    ErrorMsg = ErrorMsg + "Session Fee Should be in numeric form" + Environment.NewLine;
                                    ErrorMsgExist = true;
                                }


                                else
                                {
                                    int? SessionFeeInInt = Convert.ToInt32(SessionFee);

                                    if (SessionFeeInInt != SessionFeeofCurrentSession)
                                    {
                                        ErrorMsg = ErrorMsg + "Session Fee is incorrect" + Environment.NewLine;
                                        ErrorMsgExist = true;

                                    }
                                }

                            }

                        }
                        else
                        {
                            ErrorMsg = ErrorMsg + "Session Fee is required" + Environment.NewLine;
                            ErrorMsgExist = true;


                        }


                        var Discount = workSheet.Cells[rowIterator, 27].Value;

                        if (Discount != null)
                        {
                            var DiscountInString = Discount.ToString();

                            var isNumeric = DiscountInString.All(char.IsDigit);
                            if (isNumeric == false)
                            {
                                ErrorMsg = ErrorMsg + "Discount Should be in numeric form" + Environment.NewLine;
                                ErrorMsgExist = true;


                            }

                        }

                        var TotalFee = workSheet.Cells[rowIterator, 29].Value;

                        if (TotalFee != null)
                        {
                            var TotalFeeInString = TotalFee.ToString();

                            var isNumeric = TotalFeeInString.All(char.IsDigit);
                            if (isNumeric == false)
                            {
                                ErrorMsg = ErrorMsg + "Total Fee should be in numeric form" + Environment.NewLine;
                                ErrorMsgExist = true;

                            }

                        }
                        else
                        {
                            ErrorMsg = ErrorMsg + "Total Fee should be is Required" + Environment.NewLine;
                            ErrorMsgExist = true;


                        }

                    }//loop


                    if (ErrorMsgExist == true)
                    {


                        TempData["ErrorMsg"] = ExcelErrors;


                        //TempData["ErrorMsg"] = ErrorMsg;

                        return RedirectToAction("StudentRegister");

                    }

                    //second loop save excel column values to database
                    for (int rowIterator = 2; rowIterator < noOfRow; rowIterator++)
                    {
                        var student = new RegisterViewModel();
                        var Email = workSheet.Cells[rowIterator, 1].Value;

                        student.Email = workSheet.Cells[rowIterator, 1].Value.ToString();
                        student.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                        student.UserName = workSheet.Cells[rowIterator, 3].Value.ToString();
                        student.Password = workSheet.Cells[rowIterator, 4].Value.ToString();
                        student.ConfirmPassword = workSheet.Cells[rowIterator, 5].Value.ToString();
                        // student. = workSheet.Cells[rowIterator, 5].Value.ToString();

                        ApplicationDbContext context = new ApplicationDbContext();
                        var user = new ApplicationUser { UserName = student.UserName, Email = student.Email, Name = student.Name };
                        var result = await UserManager.CreateAsync(user, student.Password);
                        if (result.Succeeded)
                        {


                            var subjects = new List<string>();
                            var Class = workSheet.Cells[rowIterator, 6].Value.ToString();



                            subjects.Add(workSheet.Cells[rowIterator, 7].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 8].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 9].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 10].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 11].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 12].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 13].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 14].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 15].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 16].Value.ToString());


                            var subjectIDs = (from subject in db.AspNetSubjects
                                              join Classes in db.AspNetClasses on subject.ClassID equals Classes.Id
                                              where Classes.ClassName == Class && subjects.Contains(subject.SubjectName)
                                              select subject).ToList();

                            foreach (var subjectid in subjectIDs)
                            {
                                AspNetStudent_Subject stu_sub = new AspNetStudent_Subject();
                                stu_sub.StudentID = user.Id;
                                stu_sub.SubjectID = subjectid.Id;
                                db.AspNetStudent_Subject.Add(stu_sub);
                                // db.SaveChanges();
                            }

                        }



                    }

                }//using


            }//try
            catch (Exception e)
            {
                ModelState.AddModelError("", e.InnerException);
                dbTransaction.Dispose();
                return View("StudentRegister", model);
            }



            return RedirectToAction("StudentsIndex", "AspNetUser");
        }

        public class ExcelSheetError
        {
            public int Row { get; set; }
            public string Message { get; set; }
            public string Detail { get; set; }


        }

        /**********************************************************************************************************************************/
        public ActionResult GetSessionName()
        {
            int classid = db.AspNetClasses.Where(x => x.SessionID == SessionID).FirstOrDefault().Id;

            return Content(classid.ToString());
        }

        [HttpGet]
        public JsonResult SubjectsByClass(int id, string coursetype)
        {
            db.Configuration.ProxyCreationEnabled = false;
            //List<AspNetSubject> sub = db.AspNetSubjects.Where(r => r.ClassID == id && r.CourseType == coursetype).OrderByDescending(r => r.Id).ToList();
            //ViewBag.Subjects = sub;
            AspNetSubject sub = new AspNetSubject();

            if (coursetype == "CSS")
            {

                var MandatorySubjects = db.AspNetSubjects.Where(x => x.ClassID == id && x.CourseType == coursetype && x.IsManadatory == true);

                var OptionalSubjects = db.AspNetSubjects.Where(x => x.ClassID == id && x.CourseType == coursetype && x.IsManadatory == false);


                return Json(new
                {
                    MandatorySubjectsList = MandatorySubjects,
                    OptionalSubjectsList = OptionalSubjects,


                }, JsonRequestBehavior.AllowGet);
            }
            else if (coursetype == "PMS")
            {
                var MandatorySubjects = db.AspNetSubjects.Where(x => x.ClassID == id && x.CourseType == coursetype && x.IsManadatory == true);

                var OptionalSubjects = db.AspNetSubjects.Where(x => x.ClassID == id && x.CourseType == coursetype && x.IsManadatory == false);

                return Json(new
                {
                    MandatorySubjectsList = MandatorySubjects,
                    OptionalSubjectsList = OptionalSubjects,


                }, JsonRequestBehavior.AllowGet);

            }
            else if (coursetype == "One Paper MCQs")
            {
                var MandatorySubjects = db.AspNetSubjects.Where(x => x.ClassID == id && x.CourseType == coursetype && x.IsManadatory == true);

                //var OptionalSubjects = db.AspNetSubjects.Where(x => x.ClassID == id && x.CourseType == coursetype && x.IsManadatory == false);

                return Json(new
                {
                    MandatorySubjectsList = MandatorySubjects,
                    OptionalSubjectsList = "",


                }, JsonRequestBehavior.AllowGet);
            }else
            {
                return Json(new
                {
                    MandatorySubjectsList = "",
                    OptionalSubjectsList = "",


                }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public JsonResult StudentsByClassMethod(int id)
        {
            string ClassHead = db.AspNetClasses.Where(x => x.Id == id).Select(x => x.TeacherID).First();
            string currentTeacher = User.Identity.GetUserId();

            // if(String.Compare( ClassHead , currentTeacher) == 0)

            var students = (from student in db.AspNetUsers
                            join student_subject in db.AspNetStudent_Subject on student.Id equals student_subject.StudentID
                            join subject in db.AspNetSubjects on student_subject.SubjectID equals subject.Id
                            where subject.ClassID == id
                            select new { student.Id, student.UserName, student.Name }).Distinct().ToList();

            return Json(students, JsonRequestBehavior.AllowGet);



        }

        [HttpGet]
        public JsonResult StudentsByClass(string[] bdoIds)
        {
            try
            {
                List<int?> ids = new List<int?>();
                foreach (var item in bdoIds)
                {
                    int a = Convert.ToInt32(item);
                    ids.Add(a);
                }

                var aIDs = db.AspNetParent_Child.AsEnumerable().Select(r => r.ChildID);

                var students = (from student in db.AspNetUsers.AsEnumerable()
                                join student_subject in db.AspNetStudent_Subject on student.Id equals student_subject.StudentID
                                join subject in db.AspNetSubjects on student_subject.SubjectID equals subject.Id
                                where ids.Contains(subject.ClassID)
                                orderby subject.ClassID ascending
                                select new { student.Id, student.Name, student.UserName }).Distinct().OrderBy(x => x.Name).ToList();

                // var diff = aIDs.Except(students);


                return Json(students, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }



        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        public ActionResult MonthlyFeeStudent()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }


        public ActionResult GetStudentFeeMonth()
        {
            var list = (from fee in db.StudentFeeMonths
                        join std in db.AspNetStudents on fee.StudentId equals std.Id


                        select new
                        {
                            fee.Id,
                            std.AspNetUser.Name,
                            std.AspNetUser.UserName,
                            fee.IssueDate,
                            fee.Months,
                            fee.Status,
                            fee.Discount,
                            fee.FeeReceived,
                            fee.FeeType,
                            fee.FeePayable,
                            fee.InstalmentAmount
                        }).OrderBy(x => x.IssueDate).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StudentFeeByClass(int id)
        {
            int? SessionId = db.AspNetClasses.Where(x => x.Id == id).FirstOrDefault().SessionID;

            var list = (from fee in db.StudentFeeMonths
                        join std in db.AspNetStudents on fee.StudentId equals std.Id
                        where (fee.SessionId == SessionId)
                        select new
                        {
                            fee.Id,
                            std.AspNetUser.Name,
                            std.AspNetUser.UserName,
                            fee.IssueDate,
                            fee.Months,
                            fee.Status,
                            fee.Discount,
                            fee.FeeReceived,
                            fee.FeeType,
                            fee.FeePayable,
                            fee.InstalmentAmount
                        }).OrderBy(x => x.IssueDate).ToList();


            return Json(list, JsonRequestBehavior.AllowGet);





        }

        public class StudentMonthlyFee
        {
            public int Id { get; set; }
            public string Date { get; set; }
            public string Name { get; set; }
            public string Month { get; set; }
            public string Status { get; set; }
            public double? MonthlyFee { get; set; }
            public double? PayableFee { get; set; }
            public double? Multiplier { get; set; }

        }








        #endregion



    }

}