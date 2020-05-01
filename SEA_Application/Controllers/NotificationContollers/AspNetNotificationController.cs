using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;

namespace SEA_Application.Controllers.NotificationContollers
{
    public class AspNetNotificationController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        private string AdminID;
        public AspNetNotificationController()
        {

            AdminID = Convert.ToString(System.Web.HttpContext.Current.Session["AdminID"]);
        }
        // GET: AspNetNotification
        public ActionResult Index()
        {
            var aspNetNotifications = db.AspNetNotifications.Include(a => a.AspNetUser);
            return View(aspNetNotifications.ToList());
        }

        // GET: AspNetNotification/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetNotification aspNetNotification = db.AspNetNotifications.Find(id);
            if (aspNetNotification == null)
            {
                return HttpNotFound();
            }
            return View(aspNetNotification);
        }

        // GET: AspNetNotification/Create
        public ActionResult Create()
        {
            ViewBag.SenderID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AspNetNotification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Subject,Description,Time,SenderID")] AspNetNotification aspNetNotification)
        {
            if (ModelState.IsValid)
            {
                db.AspNetNotifications.Add(aspNetNotification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SenderID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetNotification.SenderID);
            return View(aspNetNotification);
        }


        public void PTMNotification()
        {
            var transactionObj = db.Database.BeginTransaction();
            //try
            //{
            //    var parent_child = db.AspNetParent_Child.Select(x => x).ToList();
            //    int lastPTM = db.AspNetParentTeacherMeetings.Max(x => x.Id);
            //    AspNetParentTeacherMeeting parentTeacherMeeting = db.AspNetParentTeacherMeetings.Where(x => x.Id == lastPTM).FirstOrDefault();
            //    AspNetNotification aspNetNotification = new AspNetNotification();
            //    aspNetNotification.Subject = parentTeacherMeeting.Title;
            //    aspNetNotification.Description = "We cordially invite you to attend the Parent-Teacher Conference on " + parentTeacherMeeting.Date + " at " + parentTeacherMeeting.Time + ". This is an opportunity for you to learn about the academic, behavioral, and social well-being of your child and to clarify any related questions or concerns about his/her grades.";
            //    aspNetNotification.Time = Convert.ToDateTime(DateTime.Today);
            //    aspNetNotification.SenderID = AdminID;
            //    db.AspNetNotifications.Add(aspNetNotification);
            //    db.SaveChanges();
            //    int notificationID = db.AspNetNotifications.Max(x => x.Id);
            //    foreach (var item in parent_child)
            //    {
            //        AspNetNotification_User userNotification = new AspNetNotification_User();
            //        userNotification.NotificationID = notificationID;
            //        userNotification.UserID = item.ParentID;
            //        userNotification.Seen = false;
            //        db.AspNetNotification_User.Add(userNotification);
            //        db.SaveChanges();

            //    }
            //    db.SaveChanges();
            //    transactionObj.Commit();
            //}
            //catch(Exception ex)
            //{
            //    transactionObj.Dispose();
            //}


        }

        // GET: AspNetNotification/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetNotification aspNetNotification = db.AspNetNotifications.Find(id);
            if (aspNetNotification == null)
            {
                return HttpNotFound();
            }
            ViewBag.SenderID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetNotification.SenderID);
            return View(aspNetNotification);
        }

        // POST: AspNetNotification/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Subject,Description,Time,SenderID")] AspNetNotification aspNetNotification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetNotification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SenderID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetNotification.SenderID);
            return View(aspNetNotification);
        }

        // GET: AspNetNotification/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetNotification aspNetNotification = db.AspNetNotifications.Find(id);
            if (aspNetNotification == null)
            {
                return HttpNotFound();
            }
            return View(aspNetNotification);
        }

        // POST: AspNetNotification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetNotification aspNetNotification = db.AspNetNotifications.Find(id);
            db.AspNetNotifications.Remove(aspNetNotification);
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
    }
}
