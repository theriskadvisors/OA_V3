using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using SEA_Application.Controllers.NotificationContollers;
using Microsoft.AspNet.Identity;

namespace SEA_Application.Controllers
{
    public class AspNetParentTeacherMeetingsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        // GET: AspNetParentTeacherMeetings
        public ActionResult Index()
        {
            return View(db.AspNetParentTeacherMeetings.Where(x=> x.SessionID == SessionID) .ToList());
        }

        public bool CheckPTM()
        {
            try
            {
                int MeetingID = db.AspNetParentTeacherMeetings.Max(x => x.Id);
                AspNetParentTeacherMeeting meeting = db.AspNetParentTeacherMeetings.Find(MeetingID);
                if (meeting.Status == "Active")
                {
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
            
        } 

        // GET: AspNetParentTeacherMeetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetParentTeacherMeeting aspNetParentTeacherMeeting = db.AspNetParentTeacherMeetings.Find(id);
            if (aspNetParentTeacherMeeting == null)
            {
                return HttpNotFound();
            }
            return View(aspNetParentTeacherMeeting);
        }

        // GET: AspNetParentTeacherMeetings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetParentTeacherMeetings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Date,Time,Status")] AspNetParentTeacherMeeting aspNetParentTeacherMeeting, AspNetAnnouncement aspNetAnnouncement)
        {
            if (ModelState.IsValid)
            {
                var transactionObj = db.Database.BeginTransaction();
                try
                {
                    var Session = db.AspNetSessions.Where(x => x.Status == "Active").FirstOrDefault().Id;
                    aspNetParentTeacherMeeting.SessionID = Session;
                        
                    db.AspNetParentTeacherMeetings.Add(aspNetParentTeacherMeeting);
                    db.SaveChanges();
                    int MeetingID = db.AspNetParentTeacherMeetings.Max(x => x.Id);
                    List<int> SubjectIDs = db.AspNetSubjects.Where(x=> x.AspNetClass.SessionID == Session).Select(x => x.Id).ToList();
                    var parentSubject = (from parentchild in db.AspNetParent_Child
                                         join studentsubject in db.AspNetStudent_Subject on parentchild.ChildID equals studentsubject.StudentID
                                         where SubjectIDs.Contains(studentsubject.SubjectID) 
                                         select new { parentchild.ParentID, studentsubject.SubjectID }).ToList();
                    foreach (var parentsubject in parentSubject)
                    {
                        AspNetPTMAttendance PTMAttendace = new AspNetPTMAttendance();
                        PTMAttendace.ParentID = parentsubject.ParentID;
                        PTMAttendace.SubjectID = parentsubject.SubjectID;
                        PTMAttendace.Status = "Absent";
                        PTMAttendace.Rating = -1;
                        PTMAttendace.MeetingID = MeetingID;
                        db.AspNetPTMAttendances.Add(PTMAttendace);
                        db.SaveChanges();

                        int PTMAttendaceID = db.AspNetPTMAttendances.Max(x => x.Id);
                        List<int> ParentQuestionIDs = db.AspNetFeedBackForms.Where(x => x.AspNetPTMFormRole.RoleName == "Parent" && x.SessionID == Session).Select(x => x.Id).ToList();
                        foreach (var questionID in ParentQuestionIDs)
                        {
                            AspNetPTM_ParentFeedback PTM_ParentFeedback = new AspNetPTM_ParentFeedback();
                            PTM_ParentFeedback.PTMID = PTMAttendaceID;
                            PTM_ParentFeedback.HeadingID = questionID;
                            db.AspNetPTM_ParentFeedback.Add(PTM_ParentFeedback);
                            db.SaveChanges();
                        }
                        List<int> TeacherQuestionIDs = db.AspNetFeedBackForms.Where(x => x.AspNetPTMFormRole.RoleName == "Teacher" && x.SessionID == Session).Select(x => x.Id).ToList();
                        foreach (var questionID in TeacherQuestionIDs)
                        {
                            AspNetPTM_TeacherFeedback PTM_TeacherFeedback = new AspNetPTM_TeacherFeedback();
                            PTM_TeacherFeedback.PTMID = PTMAttendaceID;
                            PTM_TeacherFeedback.HeadingID = questionID;
                            db.AspNetPTM_TeacherFeedback.Add(PTM_TeacherFeedback);
                            db.SaveChanges();
                            ///////////////////////////////////////////////////////////////////////////////////////////

                        }
                       
                    }


                    var NotificationObj = new AspNetNotification();
                    NotificationObj.Description = aspNetParentTeacherMeeting.Description;
                    NotificationObj.Subject = aspNetParentTeacherMeeting.Title;
                    NotificationObj.SenderID = User.Identity.GetUserId();
                    NotificationObj.Time = DateTime.Now;
                    NotificationObj.SessionID = Session;
                    db.AspNetNotifications.Add(NotificationObj);
                            db.SaveChanges();

                    var NotificationID = db.AspNetNotifications.Max(x => x.Id);
                    var receiverId = (from teacher in db.AspNetUsers.Where(x => x.Status != "False" && x.AspNetUsers_Session.Any(y=> y.SessionID == Session))
                                      where teacher.AspNetRoles.Select(y => y.Name).Contains("Teacher") ||
                                            teacher.AspNetRoles.Select(y => y.Name).Contains("Student") ||
                                            teacher.AspNetRoles.Select(y => y.Name).Contains("Parent")

                                      select new { teacher.Id }).ToList();

                    List<string> obled = new List<string>();

                    foreach (var sender in receiverId)
                    {
                        var notificationRecieve = new AspNetNotification_User();
                        notificationRecieve.NotificationID = NotificationID;
                        notificationRecieve.UserID = sender.Id;
                        notificationRecieve.Seen = false;
                        db.AspNetNotification_User.Add(notificationRecieve);
                        db.SaveChanges();



                        obled.Add(sender.Id);
                        }

                    //Message start
                    //var classe = db.AspNetClasses.Where(p => p.Id == aspNetHomework.ClassId).FirstOrDefault();
                    Utility obj = new Utility();
                    obj.SMSToOffitialsp("Dear Principal, Parent Teacher Meeting is scheduled on" + aspNetParentTeacherMeeting.Date + " at " + aspNetParentTeacherMeeting.Time + "., Title : " + aspNetParentTeacherMeeting.Title + " IPC NGS Preschool, Aziz Avenue, Lahore.");
                    obj.SMSToOffitialsa("Dear Admin, Parent Teacher Meeting is scheduled on " + aspNetParentTeacherMeeting.Date + " at " + aspNetParentTeacherMeeting.Time + "., Title : " + aspNetParentTeacherMeeting.Title + " IPC NGS Preschool, Aziz Avenue, Lahore.");
                    AspNetMessage oob = new AspNetMessage();
                    oob.Message = "Dear Parent, The Parent Teacher Meeting is scheduled on " + aspNetParentTeacherMeeting.Date + " at " + aspNetParentTeacherMeeting.Time + ". Your regularity and punctuality will be appreciated. IPC NGS Preschool, Aziz Avenue, Lahore.";
                       //oob.Message = "Parent Teacher Meeting is arranged,  Title : " + aspNetParentTeacherMeeting.Title + "For discription login to Portal please";
                    obj.SendSMS(oob, obled);
                    //Message end



                    var PTMNOTIFICATIOn = new AspNetNotificationController();
                    PTMNOTIFICATIOn.PTMNotification();
                    db.SaveChanges();
                    transactionObj.Commit();
                }

                catch (Exception ex)
                {
                    transactionObj.Dispose();
                }
                return RedirectToAction("Index");
            }

            return View(aspNetParentTeacherMeeting);
        }

        // GET: AspNetParentTeacherMeetings/Edit/5
        public ActionResult Edit(int id)
        {
            AspNetParentTeacherMeeting aspNetParentTeacherMeeting = db.AspNetParentTeacherMeetings.Find(id);
           
            return View(aspNetParentTeacherMeeting);
        }

        // POST: AspNetParentTeacherMeetings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Date,Time,Status")] AspNetParentTeacherMeeting aspNetParentTeacherMeeting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetParentTeacherMeeting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetParentTeacherMeeting);
        }

        // GET: AspNetParentTeacherMeetings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetParentTeacherMeeting aspNetParentTeacherMeeting = db.AspNetParentTeacherMeetings.Find(id);
            if (aspNetParentTeacherMeeting == null)
            {
                return HttpNotFound();
            }
            return View(aspNetParentTeacherMeeting);
        }

        // POST: AspNetParentTeacherMeetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetParentTeacherMeeting aspNetParentTeacherMeeting = db.AspNetParentTeacherMeetings.Find(id);
            db.AspNetParentTeacherMeetings.Remove(aspNetParentTeacherMeeting);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCnfm(int id)
        {
            AspNetParentTeacherMeeting aspNetParentTeacherMeeting = db.AspNetParentTeacherMeetings.Find(id);

            try
            {
                db.AspNetParentTeacherMeetings.Remove(aspNetParentTeacherMeeting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "This meeting can't be deleted. It has relation with Assessment";
                return View("Details", aspNetParentTeacherMeeting);
            }
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
