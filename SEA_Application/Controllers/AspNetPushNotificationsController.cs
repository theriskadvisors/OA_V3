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

namespace SEA_Application.Controllers
{
    [Authorize]
    public class AspNetPushNotificationsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetPushNotifications
        public ActionResult Index()
        {

            var UserNameLog = User.Identity.Name;
            AspNetUser currentUser = db.AspNetUsers.First(x => x.UserName == UserNameLog);
            var aspNetPushNotifications = db.AspNetPushNotifications.Include(a => a.AspNetUser);
            return View(aspNetPushNotifications.ToList().Where(x => x.UserID == currentUser.Id));
        }

        // GET: AspNetPushNotifications/Details/5
        public ActionResult Details(int? id)
         {
            var UserNameLog = User.Identity.Name;
            AspNetUser currentUser = db.AspNetUsers.First(x => x.UserName == UserNameLog);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (this.User.IsInRole("Teacher"))
            {
                ViewBag.AchorTagText = "Reply to Student Comment";
            }
            else if (this.User.IsInRole("Student"))
            {
                ViewBag.AchorTagText = "See Teacher Reply";
            }
            else
            {
                ViewBag.AchorTagText = "";
            }

            AspNetNotification_User aspNetNotification = db.AspNetNotification_User.Where(x => x.Id == id).FirstOrDefault();

            if (aspNetNotification.UserID== currentUser.Id)
            {
                aspNetNotification.Seen = true;
                db.SaveChanges();
            }

            if (aspNetNotification == null)
            {
                return HttpNotFound();
            }         
            return View(aspNetNotification.AspNetNotification);
        }

        // GET: AspNetPushNotifications/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Name");
            return View();
        }

        // POST: AspNetPushNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Message,UserID,Time,IsOpenStudent,IsOpenParent")] AspNetPushNotification aspNetPushNotification)
        {
            if (ModelState.IsValid)
            {
                db.AspNetPushNotifications.Add(aspNetPushNotification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Name", aspNetPushNotification.UserID);
            return View(aspNetPushNotification);
        }

        // GET: AspNetPushNotifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPushNotification aspNetPushNotification = db.AspNetPushNotifications.Find(id);
            if (aspNetPushNotification == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Name", aspNetPushNotification.UserID);
            return View(aspNetPushNotification);
        }

        // POST: AspNetPushNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Message,UserID,Time,IsOpen")] AspNetPushNotification aspNetPushNotification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetPushNotification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Name", aspNetPushNotification.UserID);
            return View(aspNetPushNotification);
        }

        // GET: AspNetPushNotifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPushNotification aspNetPushNotification = db.AspNetPushNotifications.Find(id);
            if (aspNetPushNotification == null)
            {
                return HttpNotFound();
            }
            return View(aspNetPushNotification);
        }

        // POST: AspNetPushNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetPushNotification aspNetPushNotification = db.AspNetPushNotifications.Find(id);
            db.AspNetPushNotifications.Remove(aspNetPushNotification);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /***********************************************************************************************************************************************************/


        public class notifications
        {
            public int? Id { get; set; }
            public string Subject { get; set; }
            public string Description { get; set; }
            public string Message { set; get; }
            public string UserID { set; get; }

            public DateTime? Time { get; set; }
            public string SenderID { get; set; }
        }
        public ActionResult Calendar_Notify()
        {
            return View();
        }
        public ActionResult Save_Holiday(string check, string title, DateTime start, DateTime end)
        {
            AspNetHoliday_Calendar_Notification cn = new AspNetHoliday_Calendar_Notification();
            cn.Title = title;
            cn.StartDate = start;
            cn.EndDate = end;
            db.AspNetHoliday_Calendar_Notification.Add(cn);
            db.SaveChanges();
            ///////////////////////////////NOTIFICATION///////////////////////////////////

            if (check == "On")
            {
                var NotificationObj = new AspNetNotification();
                NotificationObj.Description = "";
                NotificationObj.Subject = title;
                NotificationObj.SenderID = User.Identity.GetUserId();
                NotificationObj.Time = DateTime.Now;
                NotificationObj.Url = "";
                db.AspNetNotifications.Add(NotificationObj);
                db.SaveChanges();

                var NotificationID = db.AspNetNotifications.Max(x => x.Id);
                var allusers = db.AspNetEmployees.Where(x => x.VirtualRoleId == 35).Select(x => x.UserId).ToList();

                foreach (var receiver in allusers)
                {
                    var notificationRecieve = new AspNetNotification_User();
                    notificationRecieve.NotificationID = NotificationID;
                    notificationRecieve.UserID = Convert.ToString(receiver);
                    notificationRecieve.Seen = false;
                    db.AspNetNotification_User.Add(notificationRecieve);
                    db.SaveChanges();
                }
            }
           
            return View("Calendar_Notify");
        }
        public ActionResult Holidays_Details()
        {
            var datetime = DateTime.Now;
            var date = datetime.Date;
            var holiday = db.AspNetHoliday_Calendar_Notification.Where(x => x.EndDate >= date).ToList();
            //var reslut = new { holiday };
            return Json(holiday,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNotifications()
        {
            //using (SEA_DatabaseEntities db = new SEA_DatabaseEntities()) {
            try { 
                var UserNameLog = User.Identity.Name;
            AspNetUser currentUser = db.AspNetUsers.First(x => x.UserName == UserNameLog);
                if(this.User.IsInRole("Teacher"))
                {

                    var NotificationsList = (from notification in db.AspNetNotification_User
                                             where notification.UserID == currentUser.Id && notification.Seen == false
                                             select new { notification.Id, notification.AspNetNotification.Subject, notification.AspNetNotification.Time, notification.AspNetNotification.Description, notification.AspNetNotification.SenderID }).ToList();

                  //  List<notifications> NotificationsList = new List<notifications>();
                  //  var teacher = db.AspNetEmployees.Where(x => x.UserName == currentUser.UserName).Select(x => x).ToList();
                  //  foreach (var item in teacher)
                  // {
                  //   var TeacherNotification = (from notification in db.AspNetPushNotifications
                  //                            where int.Parse(notification.UserID) == item.Id // && notification.IsOpenTeacher==false
                  //                            select new { notification.Id, notification.Message, notification.Time, notification.UserID }).ToList();
                  //    foreach (var notificationOfList in TeacherNotification)
                  //    {
                  //      notifications notification = new notifications();
                  //      notification.Id = notificationOfList.Id;
                  //      notification.Message = notificationOfList.Message;
                  //      notification.Time = notificationOfList.Time;
                  //      notification.UserID = notificationOfList.UserID;
                  //      NotificationsList.Add(notification);
                  //   }
                  //}
                }
                if (this.User.IsInRole("Parent"))
            {

                //List<notifications> NotificationsList = new List<notifications>();

                    var NotificationsList = (from notification in db.AspNetNotification_User
                                             where notification.UserID == currentUser.Id && notification.Seen == false
                                             select new { notification.Id, notification.AspNetNotification.Subject, notification.AspNetNotification.Time, notification.AspNetNotification.Description, notification.AspNetNotification.SenderID }).ToList();


                //var NotificationsList = db.AspNetPushNotifications.Where(x => x.UserID == currentUser.Id && x.IsOpen == false).ToList();
                return Json(NotificationsList, JsonRequestBehavior.AllowGet);

            }
                if (this.User.IsInRole("Student"))
                {

                    //List<notifications> NotificationsList = new List<notifications>();

                    var NotificationsList = (from notification in db.AspNetNotification_User
                                             where notification.UserID == currentUser.Id && notification.Seen == false
                                             select new { notification.Id, notification.AspNetNotification.Subject, notification.AspNetNotification.Time, notification.AspNetNotification.Description, notification.AspNetNotification.SenderID }).ToList();


                    //var NotificationsList = db.AspNetPushNotifications.Where(x => x.UserID == currentUser.Id && x.IsOpen == false).ToList();
                    return Json(NotificationsList, JsonRequestBehavior.AllowGet);

                }


                else
            {
                    //List<notifications> NotificationsList = new List<notifications>();

                //var NotificationsList = db.AspNetPushNotifications.Where(x => x.UserID == currentUser.Id && x.IsOpen == false).ToList();
                   var NotificationsList = (from notification in db.AspNetNotification_User
                                            where notification.UserID == currentUser.Id && notification.Seen == false
                                            select new { notification.Id, notification.AspNetNotification.Subject, notification.AspNetNotification.Time ,notification.AspNetNotification.Description,notification.AspNetNotification.SenderID}).ToList();

                return Json(NotificationsList, JsonRequestBehavior.AllowGet);
            }
                //}
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
    }

        /***********************************************************************************************************************************************************/


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
