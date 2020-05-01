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
    public class AspNetPTM_TeacherFeedbackController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        // GET: AspNetPTM_TeacherFeedback
        public ActionResult Index()
        {
            var aspNetPTM_TeacherFeedback = db.AspNetPTM_TeacherFeedback.Where(x=> x.AspNetFeedBackForm.SessionID == SessionID).Include(a => a.AspNetFeedBackForm).Include(a => a.AspNetPTMAttendance);
            return View(aspNetPTM_TeacherFeedback.ToList());
        }

        // GET: AspNetPTM_TeacherFeedback/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPTM_TeacherFeedback aspNetPTM_TeacherFeedback = db.AspNetPTM_TeacherFeedback.Find(id);
            if (aspNetPTM_TeacherFeedback == null)
            {
                return HttpNotFound();
            }
            return View(aspNetPTM_TeacherFeedback);
        }

        // GET: AspNetPTM_TeacherFeedback/Create
        public ActionResult Create()
        {
            ViewBag.HeadingID = new SelectList(db.AspNetFeedBackForms.Where(x=> x.SessionID == SessionID), "Id", "Question");
            ViewBag.PTMID = new SelectList(db.AspNetPTMAttendances.Where(x=> x.AspNetParentTeacherMeeting.SessionID == SessionID), "Id", "ParentID");
            return View();
        }

        // POST: AspNetPTM_TeacherFeedback/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PTMID,HeadingID,FeedBack")] AspNetPTM_TeacherFeedback aspNetPTM_TeacherFeedback)
        {
            if (ModelState.IsValid)
            {
                db.AspNetPTM_TeacherFeedback.Add(aspNetPTM_TeacherFeedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HeadingID = new SelectList(db.AspNetFeedBackForms.Where(x=> x.SessionID == SessionID), "Id", "Question", aspNetPTM_TeacherFeedback.HeadingID);
            ViewBag.PTMID = new SelectList(db.AspNetPTMAttendances.Where(x=> x.AspNetParentTeacherMeeting.SessionID == SessionID), "Id", "ParentID", aspNetPTM_TeacherFeedback.PTMID);
            return View(aspNetPTM_TeacherFeedback);
        }

        // GET: AspNetPTM_TeacherFeedback/Edit/5
        public PartialViewResult TeacherFeedback()
        {

            int PTMID = Convert.ToInt32(Session["PTMID"].ToString());
            var data = db.AspNetPTM_TeacherFeedback.Where(x => x.PTMID == PTMID).Select(x => x).ToList();
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


        public void TeacherFeed(List<feedbacks> teacherFeedback)
        {
           foreach(var item in teacherFeedback)
            {
                if (item.feedback == null)
                {
                    item.feedback = "";
                }
                AspNetPTM_TeacherFeedback aspnetptmteacherfeedback = db.AspNetPTM_TeacherFeedback.Where(x => x.Id == item.Id).FirstOrDefault();
                aspnetptmteacherfeedback.FeedBack = item.feedback;
                db.SaveChanges();

            }
        }
        // GET: AspNetPTM_TeacherFeedback/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPTM_TeacherFeedback aspNetPTM_TeacherFeedback = db.AspNetPTM_TeacherFeedback.Find(id);
            if (aspNetPTM_TeacherFeedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.HeadingID = new SelectList(db.AspNetFeedBackForms.Where(x => x.SessionID == SessionID), "Id", "Question", aspNetPTM_TeacherFeedback.HeadingID);
            ViewBag.PTMID = new SelectList(db.AspNetPTMAttendances.Where(x => x.AspNetParentTeacherMeeting.SessionID == SessionID), "Id", "ParentID", aspNetPTM_TeacherFeedback.PTMID);
            return View(aspNetPTM_TeacherFeedback);
        }

        // POST: AspNetPTM_TeacherFeedback/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PTMID,HeadingID,FeedBack")] AspNetPTM_TeacherFeedback aspNetPTM_TeacherFeedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetPTM_TeacherFeedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HeadingID = new SelectList(db.AspNetFeedBackForms.Where(x => x.SessionID == SessionID), "Id", "Question", aspNetPTM_TeacherFeedback.HeadingID);
            ViewBag.PTMID = new SelectList(db.AspNetPTMAttendances.Where(x => x.AspNetParentTeacherMeeting.SessionID == SessionID), "Id", "ParentID", aspNetPTM_TeacherFeedback.PTMID);
            return View(aspNetPTM_TeacherFeedback);
        }

        // GET: AspNetPTM_TeacherFeedback/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetPTM_TeacherFeedback aspNetPTM_TeacherFeedback = db.AspNetPTM_TeacherFeedback.Find(id);
            if (aspNetPTM_TeacherFeedback == null)
            {
                return HttpNotFound();
            }
            return View(aspNetPTM_TeacherFeedback);
        }

        // POST: AspNetPTM_TeacherFeedback/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetPTM_TeacherFeedback aspNetPTM_TeacherFeedback = db.AspNetPTM_TeacherFeedback.Find(id);
            db.AspNetPTM_TeacherFeedback.Remove(aspNetPTM_TeacherFeedback);
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
