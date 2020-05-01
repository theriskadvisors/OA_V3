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
    public class AspNetPTM_ParentFeedbackController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        // GET: AspNetPTM_ParentFeedback
        public ActionResult Index()
        {
            var aspNetPTM_ParentFeedback = db.AspNetPTM_ParentFeedback.Where(x=> x.AspNetFeedBackForm.SessionID == SessionID ).Include(a => a.AspNetFeedBackForm).Include(a => a.AspNetPTMAttendance);
            return View(aspNetPTM_ParentFeedback.ToList());
        }

        // GET: AspNetPTM_ParentFeedback/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPTM_ParentFeedback aspNetPTM_ParentFeedback = db.AspNetPTM_ParentFeedback.Find(id);
            if (aspNetPTM_ParentFeedback == null)
            {
                return HttpNotFound();
            }
            return View(aspNetPTM_ParentFeedback);
        }

        // GET: AspNetPTM_ParentFeedback/Create
        public ActionResult Create()
        {
            ViewBag.HeadingID = new SelectList(db.AspNetFeedBackForms.Where(x=> x.SessionID == SessionID), "Id", "Question");
            ViewBag.PTMID = new SelectList(db.AspNetPTMAttendances.Where(x=> x.AspNetParentTeacherMeeting.SessionID == SessionID), "Id", "ParentID");
            return View();
        }

        // POST: AspNetPTM_ParentFeedback/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PTMID,HeadingID,FeedBack")] AspNetPTM_ParentFeedback aspNetPTM_ParentFeedback)
        {
            if (ModelState.IsValid)
            {
                db.AspNetPTM_ParentFeedback.Add(aspNetPTM_ParentFeedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HeadingID = new SelectList(db.AspNetFeedBackForms.Where(x=> x.SessionID == SessionID), "Id", "Question", aspNetPTM_ParentFeedback.HeadingID);
            ViewBag.PTMID = new SelectList(db.AspNetPTMAttendances.Where(x=> x.AspNetParentTeacherMeeting.SessionID == SessionID), "Id", "ParentID", aspNetPTM_ParentFeedback.PTMID);
            return View(aspNetPTM_ParentFeedback);
        }

        // GET: AspNetPTM_TeacherFeedback/Edit/5
        public PartialViewResult ParentFeedback(int? SubjectID)
        {

            string ChildID = Session["ChildID"].ToString();
            string parentID = db.AspNetParent_Child.Where(x => x.ChildID == ChildID).Select(x => x.ParentID).FirstOrDefault();
            int? subjectID = SubjectID;
            int meetingID = db.AspNetParentTeacherMeetings.Max(x => x.Id);

            int PTMID = db.AspNetPTMAttendances.Where(x => x.MeetingID == meetingID && x.SubjectID == subjectID && x.ParentID == parentID).Select(x=>x.Id).FirstOrDefault();
            
            var data = db.AspNetPTM_ParentFeedback.Where(x => x.PTMID == PTMID).Select(x => x).ToList();
            foreach (var item in data)
            {
                if (item.FeedBack == null)
                {
                    item.FeedBack = "Your Kind Response";
                }
            }
                return PartialView(data);
        }
        public class feedbacks
        {
            public int Id { get; set; }
            public string feedback { get; set; }
        }


        public void ParentFeed(List<feedbacks> parentFeedback)
        {
            foreach (var item in parentFeedback)
            {
                if(item.feedback==null)
                {
                    item.feedback = "";
                }
                AspNetPTM_ParentFeedback aspnetptmparentfeedback = db.AspNetPTM_ParentFeedback.Where(x => x.Id == item.Id).FirstOrDefault();
                aspnetptmparentfeedback.FeedBack = item.feedback;
                db.SaveChanges();

            }
        }

        // GET: AspNetPTM_ParentFeedback/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPTM_ParentFeedback aspNetPTM_ParentFeedback = db.AspNetPTM_ParentFeedback.Find(id);
            if (aspNetPTM_ParentFeedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.HeadingID = new SelectList(db.AspNetFeedBackForms.Where(x=> x.SessionID == SessionID), "Id", "Question", aspNetPTM_ParentFeedback.HeadingID);
            ViewBag.PTMID = new SelectList(db.AspNetPTMAttendances.Where(x=> x.AspNetParentTeacherMeeting.SessionID == SessionID), "Id", "ParentID", aspNetPTM_ParentFeedback.PTMID);
            return View(aspNetPTM_ParentFeedback);
        }

        // POST: AspNetPTM_ParentFeedback/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PTMID,HeadingID,FeedBack")] AspNetPTM_ParentFeedback aspNetPTM_ParentFeedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetPTM_ParentFeedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HeadingID = new SelectList(db.AspNetFeedBackForms.Where(x => x.SessionID == SessionID), "Id", "Question", aspNetPTM_ParentFeedback.HeadingID);
            ViewBag.PTMID = new SelectList(db.AspNetPTMAttendances.Where(x => x.AspNetParentTeacherMeeting.SessionID == SessionID), "Id", "ParentID", aspNetPTM_ParentFeedback.PTMID);
            return View(aspNetPTM_ParentFeedback);
        }

        // GET: AspNetPTM_ParentFeedback/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPTM_ParentFeedback aspNetPTM_ParentFeedback = db.AspNetPTM_ParentFeedback.Find(id);
            if (aspNetPTM_ParentFeedback == null)
            {
                return HttpNotFound();
            }
            return View(aspNetPTM_ParentFeedback);
        }

        // POST: AspNetPTM_ParentFeedback/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetPTM_ParentFeedback aspNetPTM_ParentFeedback = db.AspNetPTM_ParentFeedback.Find(id);
            db.AspNetPTM_ParentFeedback.Remove(aspNetPTM_ParentFeedback);
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
