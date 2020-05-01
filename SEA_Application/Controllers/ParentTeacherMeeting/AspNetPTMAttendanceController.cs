using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;

namespace SEA_Application.Controllers.ParentTeacherMeeting
{
    public class AspNetPTMAttendanceController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        // GET: AspNetPTMAttendance
        public ActionResult Index()
        {
            var aspNetPTMAttendances = db.AspNetPTMAttendances.Where(x=> x.AspNetSubject.AspNetClass.SessionID == SessionID).Include(a => a.AspNetParentTeacherMeeting).Include(a => a.AspNetUser).Include(a => a.AspNetSubject);
            return View(aspNetPTMAttendances.ToList());
        }

        // GET: AspNetPTMAttendance/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPTMAttendance aspNetPTMAttendance = db.AspNetPTMAttendances.Find(id);
            if (aspNetPTMAttendance == null)
            {
                return HttpNotFound();
            }
            return View(aspNetPTMAttendance);
        }

        // GET: AspNetPTMAttendance/Create
        public ActionResult Create()
        {
            ViewBag.MeetingID = new SelectList(db.AspNetParentTeacherMeetings.Where(x=> x.SessionID == SessionID), "Id", "Title");
            ViewBag.ParentID = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Parent") && x.Status != "False" && x.AspNetUsers_Session.Any(y=> y.SessionID == SessionID)), "Id", "Email");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x=> x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
            return View();
        }

        // POST: AspNetPTMAttendance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MeetingID,ParentID,SubjectID,Status,Rating")] AspNetPTMAttendance aspNetPTMAttendance)
        {
            if (ModelState.IsValid)
            {
                db.AspNetPTMAttendances.Add(aspNetPTMAttendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MeetingID = new SelectList(db.AspNetParentTeacherMeetings.Where(x=> x.SessionID == SessionID), "Id", "Title", aspNetPTMAttendance.MeetingID);
            ViewBag.ParentID = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Parent") && x.Status != "False" && x.AspNetUsers_Session.Any(y => y.SessionID == SessionID)), "Id", "Email", aspNetPTMAttendance.ParentID);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x=> x.AspNetClass.SessionID == SessionID), "Id", "SubjectName", aspNetPTMAttendance.SubjectID);
            return View(aspNetPTMAttendance);
        }

        // GET: AspNetPTMAttendance/Edit/5
        public PartialViewResult Edit(string studentId,int subjectID)
        {
            string ChildID = studentId;
            string parentID = db.AspNetParent_Child.Where(x => x.ChildID == ChildID).Select(x => x.ParentID).FirstOrDefault();
            int meetingID = db.AspNetParentTeacherMeetings.Max(x => x.Id);
        
            AspNetPTMAttendance aspNetPTMAttendance = db.AspNetPTMAttendances.Where(x => x.MeetingID == meetingID && x.SubjectID == subjectID && x.ParentID == parentID).FirstOrDefault();
            Session["PTMID"] = aspNetPTMAttendance.Id;
            ViewBag.MeetingID = new SelectList(db.AspNetParentTeacherMeetings.Where(x => x.SessionID == SessionID), "Id", "Title", aspNetPTMAttendance.MeetingID);
            ViewBag.ParentID = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Parent") && x.Status != "False" && x.AspNetUsers_Session.Any(y => y.SessionID == SessionID)), "Id", "Email", aspNetPTMAttendance.ParentID);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => x.AspNetClass.SessionID == SessionID), "Id", "SubjectName", aspNetPTMAttendance.SubjectID);
            return PartialView(aspNetPTMAttendance);
        }

        // POST: AspNetPTMAttendance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MeetingID,ParentID,SubjectID,Status,Rating")] AspNetPTMAttendance aspNetPTMAttendance)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(aspNetPTMAttendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ParentTeacherMeeting","Teacher_Dashboard");
            }
            ViewBag.MeetingID = new SelectList(db.AspNetParentTeacherMeetings.Where(x => x.SessionID == SessionID), "Id", "Title", aspNetPTMAttendance.MeetingID);
            ViewBag.ParentID = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Parent") && x.Status != "False" && x.AspNetUsers_Session.Any(y => y.SessionID == SessionID)), "Id", "Email", aspNetPTMAttendance.ParentID);
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x => x.AspNetClass.SessionID == SessionID), "Id", "SubjectName", aspNetPTMAttendance.SubjectID);
            return View(aspNetPTMAttendance);
        }

        // GET: AspNetPTMAttendance/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPTMAttendance aspNetPTMAttendance = db.AspNetPTMAttendances.Find(id);
            if (aspNetPTMAttendance == null)
            {
                return HttpNotFound();
            }
            return View(aspNetPTMAttendance);
        }

        // POST: AspNetPTMAttendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetPTMAttendance aspNetPTMAttendance = db.AspNetPTMAttendances.Find(id);
            db.AspNetPTMAttendances.Remove(aspNetPTMAttendance);
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
