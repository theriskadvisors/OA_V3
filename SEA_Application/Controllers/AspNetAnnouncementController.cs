using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SEA_Application.Models;

namespace SEA_Application.Controllers
{
    [Authorize(Roles = "Teacher,Principal")]
    public class AspNetAnnouncementController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        string TeacherID;
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        public AspNetAnnouncementController()
        {
            TeacherID = Convert.ToString(System.Web.HttpContext.Current.Session["TeacherID"]);
        }
        // GET: AspNetAnnouncement
        public ActionResult Index()
        {
            return View(db.AspNetAnnouncements.Where(x=> x.SessionID == SessionID).ToList());
        }

        // GET: AspNetAnnouncement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAnnouncement aspNetAnnouncement = db.AspNetAnnouncements.Find(id);
            if (aspNetAnnouncement == null)
            {
                return HttpNotFound();
            }
            return View(aspNetAnnouncement);
        }

        // GET: AspNetAnnouncement/Create
        public ActionResult Create()
        {
            string Id = User.Identity.GetUserId();
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x=> x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
            return View();
        }

        // POST: AspNetAnnouncement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description")] AspNetAnnouncement aspNetAnnouncement)
        {

            

            string subjects = Request.Form["subjects"];
            IEnumerable<string> selectedsubjects = subjects.Split(',');
            if (ModelState.IsValid)
            {   
                var dbContextTransaction = db.Database.BeginTransaction();
                try
                {

                    aspNetAnnouncement.Date = DateTime.Now;
                    db.AspNetAnnouncements.Add(aspNetAnnouncement);
                    db.SaveChanges();
                    int announcementID = db.AspNetAnnouncements.Max(item => item.Id);
                    List<int> SubjectIDs = new List<int>();
                    foreach (var item in selectedsubjects)
                    {
                        int subjectID = Convert.ToInt32(item);
                        SubjectIDs.Add(subjectID);
                    }
                    foreach (var item in SubjectIDs)
                    {
                        AspNetAnnouncement_Subject ann_sub = new AspNetAnnouncement_Subject();
                        ann_sub.SubjectID = item;
                        ann_sub.AnnouncementID = announcementID;
                        db.AspNetAnnouncement_Subject.Add(ann_sub);
                        db.SaveChanges();
                    }
                    List<string> student = db.AspNetStudent_Subject.Where(x => SubjectIDs.Contains(x.SubjectID)).Select(s => s.StudentID).ToList();
                    foreach (var item in student)
                    {

                        AspNetStudent_Announcement stu_ann = new AspNetStudent_Announcement();
                        stu_ann.StudentID = item;
                        stu_ann.AnnouncementID = announcementID;
                        stu_ann.Seen = false;
                        db.AspNetStudent_Announcement.Add(stu_ann);
                        db.SaveChanges();
                        /////////////////////////////////////////////////////Push Notifications Module////////////////////////////////////////////////////////////

                        //var pushNotificationObj = new AspNetPushNotification();
                        //pushNotificationObj.Message = "New Announcement: " + aspNetAnnouncement.Title + ", Description: " + aspNetAnnouncement.Description;
                        //pushNotificationObj.UserID = item;
                        //pushNotificationObj.IsOpenParent = false;
                        //pushNotificationObj.IsOpenStudent = false;
                        //pushNotificationObj.Time = DateTime.Now;
                        //db.AspNetPushNotifications.Add(pushNotificationObj);
                        //db.SaveChanges();

                    }
                    ////////////////////////////////////////////////NOTIFICATION///////////////////////////////////////////////////////////////////


                    var NotificationObj = new AspNetNotification();
                    NotificationObj.Description = aspNetAnnouncement.Description;
                    NotificationObj.Subject = aspNetAnnouncement.Title;
                    NotificationObj.SenderID = User.Identity.GetUserId();
                    NotificationObj.Time = DateTime.Now;
                    NotificationObj.Url = "/AspNetAnnouncement/details/" + aspNetAnnouncement.Id;
                    NotificationObj.SessionID = SessionID;
                    db.AspNetNotifications.Add(NotificationObj);
                    db.SaveChanges();

                    var NotificationID = db.AspNetNotifications.Max(x => x.Id);
                    List<string> students = db.AspNetStudent_Subject.Where(x => SubjectIDs.Contains(x.SubjectID)).Select(s => s.StudentID).ToList();


                    var users = new List<String>();

                    foreach (var item in students)
                    {
                        var parentID = db.AspNetParent_Child.Where(x => x.ChildID == item).Select(x => x.ParentID).FirstOrDefault();
                        users.Add(parentID);
                    }

                    var allusers = users.Union(students);

                    //Message start
                    //var classe = db.AspNetClasses.Where(p => p.Id == aspNetHomework.ClassId).FirstOrDefault();
                    //Utility obj = new Utility();
                    //obj.SMSToOffitialsp("Dear Principal, Announcement has been made with Title: " + aspNetAnnouncement.Title + "- IPC NGS Preschool, Aziz Avenue, Lahore.");
                    //obj.SMSToOffitialsa("Dear Admin, Announcement has been made with Title: " + aspNetAnnouncement.Title + "- IPC NGS Preschool, Aziz Avenue, Lahore.");
                    //AspNetMessage oob = new AspNetMessage();
                    //oob.Message = "Dear Parents, Announcement has been made with Title: " + aspNetAnnouncement.Title + "For discription login to Portal please  - IPC NGS Preschool, Aziz Avenue, Lahore.";
                    //obj.SendSMS(oob, users);
                    //Message end

                    foreach (var receiver in allusers)
                    {
                        var notificationRecieve = new AspNetNotification_User();
                        notificationRecieve.NotificationID = NotificationID;
                        notificationRecieve.UserID = Convert.ToString(receiver);
                        notificationRecieve.Seen = false;
                        db.AspNetNotification_User.Add(notificationRecieve);
                        db.SaveChanges();
                    }
                   
                    dbContextTransaction.Commit();
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    var UserNameLog = User.Identity.Name;
                    AspNetUser a = db.AspNetUsers.First(x => x.UserName == UserNameLog);
                    string UserIDLog = a.Id;
                    var classObjLog = Request.Form["ClassID"];
                    var logMessage = "A new Announcement Added, Topic: " + aspNetAnnouncement.Title + ", Description: " + aspNetAnnouncement.Description + ", ClassIDs: " + classObjLog;

                    var LogControllerObj = new AspNetLogsController();
                    LogControllerObj.CreateLogSave(logMessage, UserIDLog);
                    
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                }
                catch (Exception)
                {
                    dbContextTransaction.Dispose();
                }

                return RedirectToAction("Teacher_Announcement", "Teacher_Dashboard");
            }

            return View(aspNetAnnouncement);
        }


        // GET: AspNetAnnouncement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAnnouncement aspNetAnnouncement = db.AspNetAnnouncements.Find(id);
            if (aspNetAnnouncement == null)
            {
                return HttpNotFound();
            }
            return View(aspNetAnnouncement);
        }

        // POST: AspNetAnnouncement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Date")] AspNetAnnouncement aspNetAnnouncement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetAnnouncement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetAnnouncement);
        }

        // GET: AspNetAnnouncement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetAnnouncement aspNetAnnouncement = db.AspNetAnnouncements.Find(id);
            if (aspNetAnnouncement == null)
            {
                return HttpNotFound();
            }
            return View(aspNetAnnouncement);
        }

        // POST: AspNetAnnouncement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetAnnouncement aspNetAnnouncement = db.AspNetAnnouncements.Find(id);

            db.AspNetAnnouncements.Remove(aspNetAnnouncement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult SubjectsByClass(string[] bdoIds)
        {
            if(bdoIds[0] == "")
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            db.Configuration.ProxyCreationEnabled = false;
            List<int> ids = new List<int>();

            foreach (var item in bdoIds)
            {
                int a = Convert.ToInt32(item);
                ids.Add(a);
            }

            var Subjects = db.AspNetSubjects.Where(x => ids.Contains(x.AspNetClass.Id) && x.TeacherID == TeacherID).Select(x=> new { x.SubjectName , x.Id} ).ToList();
            ViewBag.Subjects = Subjects;
            return Json(Subjects, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
