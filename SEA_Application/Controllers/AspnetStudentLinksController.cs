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
    public class AspnetStudentLinksController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspnetStudentLinks
        public ActionResult Index()
        {
            var aspnetStudentLinks = db.AspnetStudentLinks.Include(a => a.AspnetLesson);
            return View(aspnetStudentLinks.ToList());
        }

        // GET: AspnetStudentLinks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetStudentLink aspnetStudentLink = db.AspnetStudentLinks.Find(id);
            if (aspnetStudentLink == null)
            {
                return HttpNotFound();
            }
            return View(aspnetStudentLink);
        }

        // GET: AspnetStudentLinks/Create
        public ActionResult Create()
        {
            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name");
            return View();
        }

        // POST: AspnetStudentLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,URL,LessonId,CreationDate")] AspnetStudentLink aspnetStudentLink)
        {
            if (ModelState.IsValid)
            {
                db.AspnetStudentLinks.Add(aspnetStudentLink);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name", aspnetStudentLink.LessonId);
            return View(aspnetStudentLink);
        }

        // GET: AspnetStudentLinks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetStudentLink aspnetStudentLink = db.AspnetStudentLinks.Find(id);
            if (aspnetStudentLink == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name", aspnetStudentLink.LessonId);
            return View(aspnetStudentLink);
        }

        // POST: AspnetStudentLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,URL,LessonId,CreationDate")] AspnetStudentLink aspnetStudentLink)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnetStudentLink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name", aspnetStudentLink.LessonId);
            return View(aspnetStudentLink);
        }

        // GET: AspnetStudentLinks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetStudentLink aspnetStudentLink = db.AspnetStudentLinks.Find(id);
            if (aspnetStudentLink == null)
            {
                return HttpNotFound();
            }
            return View(aspnetStudentLink);
        }

        // POST: AspnetStudentLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspnetStudentLink aspnetStudentLink = db.AspnetStudentLinks.Find(id);
            db.AspnetStudentLinks.Remove(aspnetStudentLink);
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
