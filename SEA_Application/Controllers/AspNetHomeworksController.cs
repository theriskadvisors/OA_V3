using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Text;

namespace SEA_Application.Controllers
{
    public class AspNetHomeworksController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);
        private string TeacherID;

        public AspNetHomeworksController()
        {

            TeacherID = Convert.ToString(System.Web.HttpContext.Current.Session["TeacherID"]);
        }
        // GET: AspNetHomeworks
        public ActionResult Index()
        {
            if (User.IsInRole("Principal"))
            {
                ViewBag.ClassIDs = new SelectList(db.AspNetClasses.Where(x=> x.SessionID == SessionID), "Id", "ClassName");
            }
            else
            {
                ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            }
            var aspNetHomeworks = db.AspNetHomeworks.Where(x=> x.AspNetClass.SessionID == SessionID).Include(a => a.AspNetClass).OrderByDescending(p => p.Date).ToList();
            
            return View(aspNetHomeworks.ToList());
        }
        public ActionResult successmessage()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");

            ViewBag.Error = "Diary successfully assigned";
            return View("Index");
        }
        // GET: AspNetHomeworks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetHomework aspNetHomework = db.AspNetHomeworks.Find(id);
            if (aspNetHomework == null)
            {
                return HttpNotFound();
            }
            return View(aspNetHomework);
        }

        // GET: AspNetHomeworks/Create 
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            return View();
        }

        // POST: AspNetHomeworks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClassId,Date")] AspNetHomework aspNetHomework)
        {
            if (ModelState.IsValid)
            {
                db.AspNetHomeworks.Add(aspNetHomework);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            ViewBag.ClassId = new SelectList(db.AspNetClasses.Where(x=> x.SessionID == SessionID), "Id", "ClassName", aspNetHomework.ClassId);
            return View(aspNetHomework);
        }

        // GET: AspNetHomeworks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetHomework aspNetHomework = db.AspNetHomeworks.Find(id);
            if (aspNetHomework == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(db.AspNetClasses, "Id", "ClassName", aspNetHomework.ClassId);
            return View(aspNetHomework);
        }

        // POST: AspNetHomeworks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClassId,Date")] AspNetHomework aspNetHomework)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetHomework).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(db.AspNetClasses, "Id", "ClassName", aspNetHomework.ClassId);
            return View(aspNetHomework);
        }

        // GET: AspNetHomeworks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetHomework aspNetHomework = db.AspNetHomeworks.Find(id);
            if (aspNetHomework == null)
            {
                return HttpNotFound();
            }
            return View(aspNetHomework);
        }

        // POST: AspNetHomeworks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetHomework aspNetHomework = db.AspNetHomeworks.Find(id);
            db.AspNetHomeworks.Remove(aspNetHomework);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult DiaryByClass(int classId)
        {
            var Diary = (from diary in db.AspNetHomeworks
                         where diary.ClassId == classId
                         select new {diary.Id, diary.AspNetClass.ClassName, diary.Date, diary.PrincipalApproved_status }).OrderByDescending(p => p.Date).ToList();
            
            return Json(Diary, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SubjectByClass(int classId)
        {
            var Subjects = (from subjects in db.AspNetSubjects
                         where subjects.ClassID == classId && subjects.Status!="false"
                         select new { subjects.Id,subjects.SubjectName }).ToList();
            return Json(Subjects, JsonRequestBehavior.AllowGet);
        }
        public class Subject_Homework
        {
            public int SubjectID { get; set; }
            public string HomeworkDetail { get; set; }
        }
        public class Homework
        {
            public int ClassId { get; set; }
            public DateTime Date { get; set; }
            public string TeacherComment { get; set; }
            public string Reading { get; set; }
            public List<Subject_Homework> subject_Homework { get; set; }

        }


        [HttpPost]
        public JsonResult AddDiary(Homework aspNetHomework)
        {
            AspNetHomework aspNetHomeworks = new AspNetHomework();
            aspNetHomeworks.ClassId = aspNetHomework.ClassId;
            aspNetHomeworks.Date =      Convert.ToDateTime(aspNetHomework.Date);



            aspNetHomeworks.PrincipalApproved_status = "Approved";
            db.AspNetHomeworks.Add(aspNetHomeworks);
            db.SaveChanges();

            var HomeWorkID = db.AspNetHomeworks.Max(x => x.Id);

            foreach (var subject in aspNetHomework.subject_Homework)
            {
                AspNetSubject_Homework aspNetSubject_Homework = new AspNetSubject_Homework();
                aspNetSubject_Homework.HomeworkID = HomeWorkID;
                aspNetSubject_Homework.SubjectID = subject.SubjectID;
                aspNetSubject_Homework.HomeworkDetail = subject.HomeworkDetail;
                db.AspNetSubject_Homework.Add(aspNetSubject_Homework);
                db.SaveChanges();
            }
            List<string> ParentList = new List<string>();
            var StudentList = db.AspNetStudents.Where(x => x.ClassID == aspNetHomework.ClassId).Select(x => x.StudentID).ToList();
            foreach (var student in StudentList)
            {
                AspNetStudent_HomeWork aspNetStudent_Homework = new AspNetStudent_HomeWork();
                aspNetStudent_Homework.HomeworkID = HomeWorkID;
                aspNetStudent_Homework.Reading = aspNetHomework.Reading;
                aspNetStudent_Homework.TeacherComments = aspNetHomework.TeacherComment;
                aspNetStudent_Homework.Status = "Not Seen by Parents";
                aspNetStudent_Homework.StudentID = student;
                aspNetStudent_Homework.ParentComments = "";
                db.AspNetStudent_HomeWork.Add(aspNetStudent_Homework);
                db.SaveChanges();

                //var parent = db.AspNetParent_Child.Where(p => p.ChildID == student).FirstOrDefault();
                //if(parent != null)
                //{
                //    ParentList.Add(parent.ParentID);
                //}
                //else
                //{
                //    var mee = "This student id don't have parent " + student + " NGS Portal";
                //    Utility ob = new Utility(); 
                //    ob.messagetosupport(mee);
                //}


            }

            var classe = db.AspNetClasses.Where(p => p.Id == aspNetHomework.ClassId).FirstOrDefault();
            Utility obj = new Utility();
            obj.SMSToOffitialsp("Dear Principal, Homework for Class: " + classe.ClassName + " has been assigned. IPC Aziz Avenue Campus.");
            obj.SMSToOffitialsa("Dear Admin, Homework for Class: " + classe.ClassName + " has been assigned. IPC Aziz Avenue Campus.");
            //////////////////////////////////NOTIFICATION///////////////////////////////////


            var NotificationObj = new AspNetNotification();
            NotificationObj.Description = "Today's Diray is assigned";
            NotificationObj.Subject = "Diary";
            NotificationObj.SenderID = User.Identity.GetUserId();
            NotificationObj.Time = DateTime.Now;
            NotificationObj.Url = "/AspNetHomework/Details/" + aspNetHomework.ClassId;
            NotificationObj.SessionID = SessionID;
            db.AspNetNotifications.Add(NotificationObj);
            db.SaveChanges();

            var NotificationID = db.AspNetNotifications.Max(x => x.Id);

            var users = new List<String>();

            foreach (var item in StudentList)
            {
                var parentID = db.AspNetParent_Child.Where(x => x.ChildID == item).Select(x => x.ParentID).FirstOrDefault();
                users.Add(parentID);
            }

            var allusers = users.Union(StudentList);

            foreach (var receiver in allusers)
            {
                var notificationRecieve = new AspNetNotification_User();
                notificationRecieve.NotificationID = NotificationID;
                notificationRecieve.UserID = Convert.ToString(receiver);
                notificationRecieve.Seen = false;
                db.AspNetNotification_User.Add(notificationRecieve);
                db.SaveChanges();
            }
            //////////////////////////////////////////Email//////////////////////////////////////////
            var students = db.AspNetStudents.Where(x => x.ClassID == aspNetHomework.ClassId).Select(x => x).ToList();

            string[] studentRollList = new string[students.Count];
            int r = 0;
            foreach (var item in students)
            {
                studentRollList[r] = item.AspNetUser.UserName;
                r++;
            }
            //var StudentName = db.AspNetStudent_Project.Where(sp => sp.ProjectID == aspNetProject.Id).Select(x => x.AspNetUser.Name).ToList();

            string[] studentNamelist = new string[students.Count];
            int i = 0;
            foreach (var item in students)
            {
                studentNamelist[i] = item.AspNetUser.Name;
                i++;
            }

            var Users = new List<String>();
            foreach (var item in StudentList)
            {
                Users.Add(db.AspNetParent_Child.Where(x => x.ChildID == item).Select(x => x.ParentID).FirstOrDefault());
            }
            List<string> EmailList = new List<string>();
            foreach (var sender in Users)
            {
                EmailList.Add(db.AspNetUsers.Where(x => x.Id == sender).Select(x => x.Email).FirstOrDefault());
            }
            var j = 0;
            //foreach (var toEmail in EmailList)
            //{
            //    try
            //    {
            //        var Subject = studentNamelist[j] + "(" + studentRollList[j] + ") " + " Diary";
            //        j++;
            //        string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
            //        string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

            //        SmtpClient client = new SmtpClient("smtpout.secureserver.net", 25);
            //        client.EnableSsl = false;
            //        client.Timeout = 100000;
            //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //        client.UseDefaultCredentials = false;
            //        client.Credentials = new NetworkCredential(senderEmail, senderPassword);
            //        MailMessage mailMessage = new MailMessage(senderEmail, toEmail, Subject, NotificationObj.Description);
            //        mailMessage.CC.Add(new MailAddress(senderEmail));
            //        mailMessage.IsBodyHtml = true;
            //        mailMessage.BodyEncoding = UTF8Encoding.UTF8;
            //        client.Send(mailMessage);
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}
