using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;

namespace SEA_Application.Controllers
{
    public class AspNetChapterController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);

        private string TeacherID;
        public AspNetChapterController()
        {

            TeacherID = Convert.ToString(System.Web.HttpContext.Current.Session["TeacherID"]);
        }

        // GET: AspNetChapter
        public ActionResult Index()
        {
            var aspNetChapters = db.AspNetChapters.Where(x=> x.AspNetSubject.AspNetClass.SessionID == SessionID).Include(a => a.AspNetSubject);
            return View(aspNetChapters.ToList());
        }

        // GET: AspNetChapter/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetChapter aspNetChapter = db.AspNetChapters.Find(id);
            if (aspNetChapter == null)
            {
                return HttpNotFound();
            }
            return View(aspNetChapter);
        }

        // GET: AspNetChapter/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID && x.AspNetClass.SessionID == SessionID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x=> x.AspNetClass.SessionID == SessionID), "Id", "SubjectName");
            return View();
        }

        // POST: AspNetChapter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ChapterNo,ChapterName,StartDate,EndDate,SubjectID,Status")] AspNetChapter aspNetChapter)
        {
            if (ModelState.IsValid)
            {
                db.AspNetChapters.Add(aspNetChapter);
                db.SaveChanges();
                ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
                ViewBag.CreateChapter = "Chapter created and updated successfully";
                return View("../Teacher_Dashboard/_Topics");
            }

            ViewBag.SubjectID = new SelectList(db.AspNetSubjects.Where(x=> x.AspNetClass.SessionID == SessionID), "Id", "SubjectName", aspNetChapter.SubjectID);
            
            return View(aspNetChapter);
        }

        // GET: AspNetChapter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetChapter aspNetChapter = db.AspNetChapters.Find(id);
            if (aspNetChapter == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetChapter.SubjectID);
            return View(aspNetChapter);
        }

        // POST: AspNetChapter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ChapterNo,ChapterName,StartDate,EndDate,SubjectID,Status")] AspNetChapter aspNetChapter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetChapter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Account");
            }
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetChapter.SubjectID);
            return View(aspNetChapter);
        }

        // GET: AspNetChapter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetChapter aspNetChapter = db.AspNetChapters.Find(id);
            if (aspNetChapter == null)
            {
                return HttpNotFound();
            }
            return View(aspNetChapter);
        }

        // POST: AspNetChapter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetChapter aspNetChapter = db.AspNetChapters.Find(id);
            db.AspNetChapters.Remove(aspNetChapter);
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Account");
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
