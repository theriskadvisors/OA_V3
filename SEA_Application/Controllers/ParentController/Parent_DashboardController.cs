using NReco.PdfGenerator;
using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Clickatell_Service;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

//string response;
//string api = "PUqo7q5gQSCdwOMEpyHw3Q==";
//Dictionary<string, string> Params = new Dictionary<string, string>();

//Params.Add("content", "this is a message");
//Params.Add("to", "+923124199766");

//response = Api.SendSMS(api, Params);


namespace SEA_Application.Controllers.ParentController
{
    public class Parent_DashboardController : Controller
    {
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);
        private string ParentID;
        private static string StudentID;
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        public Parent_DashboardController()
        {
            ParentID = Convert.ToString(System.Web.HttpContext.Current.Session["ParentID"]);
        }
        
        public ActionResult Dashboard()
        {
            return View("BlankPage");
            
        }
        [Authorize(Roles = "Parent")]
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


        public JsonResult CalendarNotification()
        {
            var datadate="";
            DateTime beforedate = DateTime.Today.AddDays(-15);
            DateTime afterdate = DateTime.Today.AddDays(15);
      ////////////////PTM List////////////////////////////
            List<Event> PTMlist = new List<Event>();
            var ptm = db.AspNetParentTeacherMeetings.Where(x => x.Date >= beforedate && x.Date <= afterdate).ToList();
            foreach (var item in ptm)
            {
                Event ptmevent = new Event();
                ptmevent.Id = item.Id;
                ptmevent.Name = item.Title;
                ptmevent.StartDate = item.Date.Value.Year.ToString() + "-" + item.Date.Value.Month.ToString() + "-" + item.Date.Value.Day.ToString();
                ptmevent.EndDate = item.Date.Value.Year.ToString() + "-" + item.Date.Value.Month.ToString() + "-" + item.Date.Value.Day.ToString();
                ptmevent.StartTime = item.Time;
                ptmevent.EndTime = item.Time;
                ptmevent.Color = "#228B22";
                ptmevent.Url = "";
                PTMlist.Add(ptmevent);
            }
            //////////////////Project List////////////
            List<Event> PROlist = new List<Event>();
            var childlist = db.AspNetParent_Child.Where(x => x.ParentID == ParentID).Select(x => x.ChildID).ToList();
            foreach (var item in childlist)
            {
                var l = db.AspNetStudent_Project.Where(x => x.StudentID == item).Select(x => x.ProjectID).ToList();
                foreach (var i in l)
                {
                    var pro = db.AspNetProjects.Where(x => x.DueDate >= beforedate && x.DueDate <= afterdate && x.Id == i).FirstOrDefault();
                    if (pro != null)
                    {
                        datadate= pro.PublishDate.Value.Year.ToString() + "-" + pro.PublishDate.Value.Month.ToString() + "-" + pro.PublishDate.Value.Day.ToString();
                        Event proevent = new Event();
                        proevent.Id = pro.Id;
                        proevent.Name = pro.Title;
                        proevent.StartDate = pro.PublishDate.Value.Year.ToString() + "-" + pro.PublishDate.Value.Month.ToString() + "-" + pro.PublishDate.Value.Day.ToString();
                        proevent.EndDate = pro.DueDate.Value.Year.ToString() + "-" + pro.DueDate.Value.Month.ToString() + "-" + pro.DueDate.Value.Day.ToString();
                        proevent.StartTime = "";
                        proevent.EndTime = "";
                        proevent.Color = "#D7BDE2";
                        proevent.Url = "";
                        PROlist.Add(proevent);
                    }

                }
            }
            /////////Attendance List////////////////
            //var time = db.AspNetTime_Setting.Where(x => x.Id == 1).FirstOrDefault();
            //var lateTime = time.LateTime;
            //List<Event> ATTlist = new List<Event>();
            //foreach (var item in childlist)
            //{
            //    var att_list = (from user in db.AspNetUsers
            //                    join att in db.AspNetStudent_AutoAttendance
            //                    on user.UserName equals att.Roll_Number
            //                    where user.Id == item && att.TimeIn != null
            //                    select new { att.Attendance_id, att.Date, att.TimeIn }).ToList();
            //    foreach (var a in att_list)
            //    {
                   
            //        Event at = new Event();
            //        at.Id = a.Attendance_id;
            //        if(a.TimeIn>lateTime)
            //        {
            //            var hh = a.TimeIn.Hours;
            //            var mm = a.TimeIn.Minutes;
            //            var tt=hh+":"+mm;
                      
            //            at.Name = "Late Marked : " +tt;
            //            at.Color = "#FFD700";
            //        }
            //        else
            //        {
            //            at.Name = "Attendance Marked";
            //            at.Color = "#A4EADA";
            //        }
                   
            //        at.StartDate = a.Date.Value.Year.ToString() + "-" + a.Date.Value.Month.ToString() + "-" + a.Date.Value.Day.ToString();
            //        at.EndDate = "";
            //        at.StartTime = "";
            //        at.EndTime = "";                    
            //        at.Url = "";
            //        ATTlist.Add(at);
            //    }

            //}
            //////////////Absent List///////////////////////
            //List<Event> Abslist = new List<Event>();
            //foreach (var item in childlist)
            //{
            //    var att_list = (from user in db.AspNetUsers
            //                    join att in db.AspNetAbsent_Student
            //                    on user.UserName equals att.Roll_Number
            //                    where user.Id == item 
            //                    select new { att.AbsentId, att.Date, }).ToList();
            //    foreach (var a in att_list)
            //    {

            //        Event at = new Event();
            //        at.Id = a.AbsentId;
            //        at.Name = "Absent";
            //        at.StartDate = a.Date.Value.Year.ToString() + "-" + a.Date.Value.Month.ToString() + "-" + a.Date.Value.Day.ToString();
            //        at.EndDate = "";
            //        at.StartTime = "";
            //        at.EndTime = "";
            //        at.Color = "#FFA07A";
            //        at.Url = "";
            //        ATTlist.Add(at);
            //    }

            //}
            ////////////Holiday Announcement/////////////
            List<Event> Hollyday = new List<Event>();
            foreach (var item in childlist)
            {
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

            }
            //////////Calendar Welcome/////////////////
            var id = User.Identity.GetUserId();
            var checkdate = DateTime.Now;
            var date = TimeZoneInfo.ConvertTime(DateTime.UtcNow.ToUniversalTime(), TimeZoneInfo.Local);
            var name = "";

            name = db.AspNetUsers.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();

            var day = date.DayOfWeek;
            var dd = date.Day;
            var mm = date.Month;
            var yy = date.Year;
            string[] array = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            var Date = day + ", " + dd + " " + array[mm - 1] + " " + yy;

            var result = new { PTMlist,PROlist,Hollyday, checkdate, Date, name };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AnnouncementDetail(int? id)
        {

            var announcementID = db.AspNetAnnouncement_Subject.Where(s => s.Id == id).Select(s => s.AnnouncementID).SingleOrDefault();
            AspNetStudent_Announcement result = db.AspNetStudent_Announcement.Where(x => x.StudentID == StudentID && x.AnnouncementID == announcementID).Select(x => x).FirstOrDefault();

            result.Seen = true;
            db.SaveChanges();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var Announcement = db.AspNetAnnouncement_Subject.Find(id);

            if (Announcement == null)
            {
                return HttpNotFound();
            }
            return View(Announcement);
        }
        public ViewResult View_Assessment()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

//            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id)), "Id", "SubjectName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id) && x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
         
            return View();
        }

        public ActionResult Assessment_Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetStudent_Assessment aspNetStudentAssessment = db.AspNetStudent_Assessment.Where(x => x.AssessmentID == id && x.StudentID == StudentID).FirstOrDefault();
            if (aspNetStudentAssessment == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudentAssessment);
        }

        public ViewResult Student_Marks()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

         //   ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id)), "Id", "SubjectName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id) && x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
         
            return View("_Student_Marks");
        }

        public ViewResult Student_Attendance()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

         //   ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id)), "Id", "SubjectName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id) && x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
         
            return View("_Student_Attendance");
        }

        public ActionResult Student_Projects()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

         //   ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id)), "Id", "SubjectName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id) && x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
         
            return View();
        }

        public JsonResult AllProjects()
        {

            var classId = db.AspNetStudents.Where(x => x.StudentID == StudentID).Select(x => x.AspNetClass.Id).FirstOrDefault();

            var projects = (from project in db.AspNetProjects
                            where project.AspNetSubject.AspNetClass.Id == classId
                            select new { project.PublishDate, project.Title, project.Description, project.Id }).ToList();
            return Json(projects, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Project_Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetStudent_Project aspNetStudentProject = db.AspNetStudent_Project.Where(x => x.ProjectID == id && x.StudentID == StudentID).FirstOrDefault();
            if (aspNetStudentProject == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudentProject);
        }

        public ViewResult Student_Announcement()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id) &&  x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
            return View("_Student_Announcement");
        }


        public ViewResult ParentTeacherMeeting()
        {
            List<int> subjectIDs = (from student_subject in db.AspNetStudent_Subject
                                    where student_subject.StudentID == StudentID
                                    select student_subject.SubjectID).ToList();

         //   ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id)), "Id", "SubjectName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => subjectIDs.Contains(x.Id) && x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
         
            return View("_ParentTeacherMeeting");
        }

        public JsonResult GetChildren()
        {
            var children = (from child in db.AspNetUsers
                            join parent_children in db.AspNetParent_Child on child.Id equals parent_children.ChildID
                            where parent_children.ParentID == ParentID && db.AspNetUsers_Session.Any(x=> x.AspNetUser.Id == child.Id && x.SessionID == SessionID)
                            select new { child.Id, child.Name }).ToList();
            
            return Json(children, JsonRequestBehavior.AllowGet);
        }

        public class Marks
        {
            public string Title { get; set; }
            public string DueDate { get; set; }
            public int? TotalMarks { get; set; }
            public Double? MarksGot { get; set; }
            public string Type { get; set; }
        }

        [HttpGet]
        public JsonResult MarksBySubject(int subjectID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var marks = (from student_assessment in db.AspNetStudent_Assessment
                         where student_assessment.AspNetAssessment.AspNetSubject_Catalog.AspNetSubject.Id == subjectID && student_assessment.StudentID == StudentID
                         group new { student_assessment.AspNetAssessment.Title, student_assessment.AspNetAssessment.DueDate, student_assessment.AspNetAssessment.TotalMarks, student_assessment.MarksGot } by student_assessment.AspNetAssessment.AspNetSubject_Catalog.AspNetCatalog.CatalogName into g
                         select new { Key = g.Key, Marks = g.ToList() }).ToList();

            return Json(marks, JsonRequestBehavior.AllowGet);
        }

        public FileResult downloadAssessmentFile(int id)
        {
            AspNetAssessment Assessment = db.AspNetAssessments.Find(id);

            var filepath = System.IO.Path.Combine(Server.MapPath("~/App_Data/Assessments/"), Assessment.FileName);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), Assessment.FileName);

        }

        public FileResult downloadStudentSubmittedFile(int id)
        {
            AspNetStudent_Assessment Student_Assessment = db.AspNetStudent_Assessment.Find(id);

            var filepath = System.IO.Path.Combine(Server.MapPath("~/App_Data/AssessmentSubmission/"), Student_Assessment.SubmittedFileName);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), Student_Assessment.SubmittedFileName);

        }

        [HttpGet]
        public JsonResult AttendanceBySubject(int subjectID)
        {
            var attendances = (from attendance in db.AspNetStudent_Attendance
                               where attendance.StudentID == StudentID && attendance.AspNetAttendance.SubjectID == subjectID
                               select new { attendance.Reason, attendance.Status, attendance.AspNetAttendance.Date }).ToList();
            return Json(attendances, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AllAttendance()
        {
            var attendances = (from attendance in db.AspNetStudent_Attendance
                               where attendance.StudentID == StudentID
                               select new { attendance.Reason, attendance.AspNetAttendance.AspNetSubject.SubjectName , attendance.Status, attendance.AspNetAttendance.Date }).ToList();
            return Json(attendances, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Student_HomeWork()
        {
            var pid = User.Identity.GetUserId();
            ViewBag.ParentId = db.AspNetParent_Child.Where(x => x.ParentID == pid).Select(x => x.ChildID).FirstOrDefault();
            return View();
        }
        public JsonResult StudentHomeWork( string id)
        {
            
           
                var homework = (from student_homework in db.AspNetStudent_HomeWork
                                join hw in db.AspNetHomeworks on student_homework.HomeworkID equals hw.Id
                                where student_homework.StudentID == id && hw.PrincipalApproved_status == "Approved"
                            select new { student_homework.TeacherComments, student_homework.AspNetHomework.Id, student_homework.Reading, student_homework.AspNetHomework.Date }).OrderByDescending(x=>x.Date).ToList();

            var rtu = homework.OrderByDescending(p => p.Id);
          
            return Json(homework, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DiaryDetails(int? id)
        {
            var aspNetSubject_Homework = db.AspNetSubject_Homework.Where(x => x.HomeworkID == id).Include(a => a.AspNetSubject).Where(y=>y.AspNetSubject.Status!="false");
            ViewBag.teachercomment = db.AspNetStudent_HomeWork.Where(x => x.HomeworkID == id && x.StudentID==StudentID).Select(x=>x.TeacherComments).FirstOrDefault();
            ViewBag.parentcomment = db.AspNetStudent_HomeWork.Where(x => x.HomeworkID == id && x.StudentID==StudentID).Select(x => x.ParentComments).FirstOrDefault();
            ViewBag.Reading = db.AspNetStudent_HomeWork.Where(x => x.HomeworkID == id && x.StudentID == StudentID).Select(x => x.Reading).FirstOrDefault();
            ViewBag.Status = db.AspNetStudent_HomeWork.Where(x => x.HomeworkID == id && x.StudentID == StudentID).Select(x => x.Status).FirstOrDefault();
            return View(aspNetSubject_Homework.ToList());
            
        }

        
             public ActionResult SeenByParent(int HomeWorkId)
        {
            var pid = User.Identity.GetUserId();
            var sid = db.AspNetParent_Child.Where(x => x.ParentID == pid).Select(x => x.ChildID).FirstOrDefault();
            var obj = db.AspNetStudent_HomeWork.Where(x => x.StudentID == sid && x.HomeworkID == HomeWorkId).FirstOrDefault();
            obj.Status = "Seen by Parents";

            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpPost]
       public JsonResult ParentComment(string status, string comment, int homeworkId)
      {
            var dbTransection = db.Database.BeginTransaction();
            db.AspNetStudent_HomeWork.Where(x => x.StudentID == StudentID && x.HomeworkID == homeworkId).FirstOrDefault().ParentComments = comment;
            db.AspNetStudent_HomeWork.Where(x => x.StudentID == StudentID && x.HomeworkID == homeworkId).FirstOrDefault().Status = status;
            //studenthomework.ParentComments = comment;
            //db.AspNetStudent_HomeWork.Add(studenthomework);
            db.SaveChanges();
            dbTransection.Commit();


            return Json("Saved", JsonRequestBehavior.AllowGet);
        }
      
        [HttpGet]
        public JsonResult AnnouncementBySubject(int subjectID)
        {
            var announcement = (from t1 in db.AspNetAnnouncement_Subject
                                join t3 in db.AspNetStudent_Subject on t1.SubjectID equals t3.SubjectID
                                join t4 in db.AspNetUsers on t3.StudentID equals t4.Id
                                join t2 in db.AspNetStudent_Announcement on t1.AnnouncementID equals t2.AnnouncementID
                                join t5 in db.AspNetAnnouncements on t2.AnnouncementID equals t5.Id
                                
                                where t2.StudentID == StudentID && t4.Id == StudentID && t3.SubjectID == subjectID && t5.SessionID == SessionID
                                select new { t1.AspNetAnnouncement.Title, t1.Id, t1.AspNetSubject.SubjectName }).ToList();


            return Json(announcement, JsonRequestBehavior.AllowGet);
        }   

        public JsonResult AllAnnouncement()
        {
            var announcement = (from t1 in db.AspNetAnnouncement_Subject
                        join t3 in db.AspNetStudent_Subject on t1.SubjectID equals t3.SubjectID
                        join t4 in db.AspNetUsers on t3.StudentID equals t4.Id
                        join t2 in db.AspNetStudent_Announcement on t1.AnnouncementID equals t2.AnnouncementID

                        where t2.StudentID == StudentID && t4.Id == StudentID
                                select new { t1.AspNetAnnouncement.Title, t1.Id , t1.AspNetSubject.SubjectName }).ToList();


            return Json(announcement, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public void SetChildID(string studentID)
        {
            StudentID = studentID;
            Session["ChildID"] = studentID;
        }

        [HttpGet]
        public JsonResult GetChildID(string studentID)
        {
            if (StudentID == null)
            {
                StudentID = studentID;
                Session["ChildID"] = studentID;
            }
          string v1=  db.AspNetParent_Child.Where(x => x.ChildID == studentID).Select(x => x.ParentID).FirstOrDefault();
          string v2=  db.AspNetParent_Child.Where(x => x.ChildID == StudentID).Select(x => x.ParentID).FirstOrDefault();
            if(v1!=v2)
            {
                return Json(studentID, JsonRequestBehavior.AllowGet);
            }
            return Json(StudentID, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult StudentAnnouncement(string studentID)
        {
            StudentID = studentID;
            Session["ChildID"] = studentID;

            db.Configuration.ProxyCreationEnabled = false;
            var ann2 = (from t1 in db.AspNetAnnouncement_Subject
                        join t3 in db.AspNetStudent_Subject on t1.SubjectID equals t3.SubjectID
                        join t4 in db.AspNetUsers on t3.StudentID equals t4.Id
                        join t2 in db.AspNetStudent_Announcement on t1.AnnouncementID equals t2.AnnouncementID

                        where t2.StudentID == StudentID && t2.Seen == false && t4.Id == StudentID
                        select new { t1.AspNetAnnouncement.Title, t1.Id }).ToList();

            ViewBag.Subjects = ann2;
            return Json(ann2, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult ParentNotification()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var parentNotification = (from parentNotifications in db.AspNetNotification_User
                                      where parentNotifications.UserID == ParentID && parentNotifications.Seen == false
                                      select new { parentNotifications.Id, parentNotifications.AspNetNotification.Subject }).ToList();
            return Json(parentNotification, JsonRequestBehavior.AllowGet);

        }

        public ActionResult NotificationDetail(int? id)
        {

            AspNetNotification_User aspNetNotificationUser = db.AspNetNotification_User.Where(x => x.Id == id).FirstOrDefault();
            aspNetNotificationUser.Seen = true;
            db.SaveChanges();

            var AspNetNotification = db.AspNetNotifications.Find(aspNetNotificationUser.NotificationID);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // List<int> student = db.Student_Subject.Where(x => SubjectIDs.Contains(x.SubjectID)).Select(s => s.StudentID).ToList();
            if (AspNetNotification == null)
            {
                return HttpNotFound();
            }
            return View(AspNetNotification);
        }

     
        public ActionResult StudentAssessment()
        {
            
            ViewBag.Session = new SelectList(db.AspNetSessions.Where(x=>x.Id == SessionID), "Id", "SessionName");
            return View();
        }
        
        public JsonResult getTerm(int SessionId, string childId)
        {

            //db.Configuration.LazyLoadingEnabled = false;
            //db.Configuration.ProxyCreationEnabled = false;

            var terms = db.AspNetTerms.Where(x => x.SessionID == SessionId).Select(x => new {x.Id , x.TermName }).ToList();


            //var aqestiones = (from termAnswers in db.AspNetStudent_Term_Assessments_Answers
            //                  where termAnswers.AspNetStudent_Term_Assessment.StudentID == childId && termAnswers.AspNetStudent_Term_Assessment.Status == "Principle_submit"
            //                  //join question in db.AspNetAssessment_Question on termAnswers.AQID equals question.Id
            //                  orderby termAnswers.AspNetStudent_Term_Assessment.AspNetTerm.Id
            //                  group termAnswers by termAnswers.AspNetStudent_Term_Assessment.AspNetSubject.SubjectName into Subject
            //                  select new
            //                  {
            //                      subject = Subject.Key,
            //                      Data = (from data in Subject.ToList()
            //                              group data by data.AspNetAssessment_Question.AspNetAssessment_Questions_Category.CategoryName into g
            //                              select new {
            //                                  categeory = g.Key,
            //                                  questions = (from DATA in g.ToList()
            //                                               group DATA by DATA.AspNetStudent_Term_Assessment.AspNetTerm.Id into H
            //                                               select new
            //                                               {
            //                                                   term = H.Key,
            //                                                   question = H.Select(x=> x.AspNetAssessment_Question.Question),
            //                                                   answer = H.Select(x=> x.Answer)
            //                                                }).ToList()
            //                              }).ToList()
            //                  }).ToList();

            return Json(terms, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAssessment(int termID, string StudentID)
        {
            bool Submited_flag;

            try
            {
                var check = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == termID && x.StudentID == StudentID && x.Status == "Principle_submit").Select(x => x.Id).ToList();

                if (check.Count == 0)
                    Submited_flag = false;
                else
                    Submited_flag = true;
            }
            catch
            {
                Submited_flag = false;
            }



            Student_data Assessment_DATA = new Student_data();
            Assessment_DATA.assessment_data = new List<subject>();

            if (Submited_flag)
            {
                var STAID = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == termID && x.StudentID == StudentID).Select(x => x.Id).ToList();
                Assessment_DATA.PrincipleComment = db.AspNetStudent_Term_Assessment.Where(x => x.TermID == termID && x.StudentID == StudentID).Select(x => x.PrincipalComments).FirstOrDefault();


                var catageory = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.AspNetStudent_Term_Assessment.TermID == termID && x.AspNetStudent_Term_Assessment.StudentID == StudentID).Select(x => x.Catageory).Distinct().ToList();

                foreach (var item in STAID)
                {
                    subject Subject = new subject();
                    Subject.SubjectName = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.STAID == item).Select(x => x.AspNetStudent_Term_Assessment.AspNetSubject.SubjectName).First();
                    Subject.TeacherComment = db.AspNetStudent_Term_Assessment.Where(x => x.Id == item).Select(x => x.TeacherComments).FirstOrDefault();
                    Subject.CatageoryList = new List<Catageory>();

                    foreach (var Catag in catageory)
                    {
                        Catageory Catageory = new Catageory();
                        Catageory.catageoryName = Catag;
                        Catageory.QuestionList = new List<Questions>();

                        var AQID = db.AspNetStudent_Term_Assessments_Answers.Where(x => x.STAID == item && x.Catageory == Catag).ToList();

                        if (AQID.Count != 0)
                        {
                            foreach (var aqid in AQID)
                            {
                                Questions question = new Questions();
                                question.Question = aqid.Question;
                                question.Answer = aqid.Answer;
                                Catageory.QuestionList.Add(question);
                            }
                            Subject.CatageoryList.Add(Catageory);
                        }
                    }

                    Assessment_DATA.assessment_data.Add(Subject);
                }

            }

            return Json(Assessment_DATA, JsonRequestBehavior.AllowGet);
        }

        public class ChildAnnouncement
        {
            public string Title { set; get; }
            public string SubjectName { set; get; }
            public int Id { set; get; }
        }

        public class ClassSubject
        {
            public string Name { set; get; }
            public List<Categeories> Categeories { set; get; }
        }

        public class Categeories
        {
            public string CName { set; get; }
            public List<TermQuestions> Questiones { set; get; }
        }

        public class TermQuestions
        {
            public string Question { set; get; }
            public string Term1 { set; get; }
            public string Term2 { set; get; }
            public string Term3 { set; get; }
        }

        public class Student_data
        {
            public List<subject> assessment_data { get; set; }
            public string PrincipleComment { set; get; }
        }
        public class subject
        {
            public string SubjectName { set; get; }
            public string TeacherComment { set; get; }
            public List<Catageory> CatageoryList { set; get; }
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
        public class Catageory
        {
            public string catageoryName { get; set; }
            public List<Questions> QuestionList { get; set; }
        }

        public class Questions
        {
            public string Question { get; set; }
            public string Answer { get; set; }
        }
    }
}