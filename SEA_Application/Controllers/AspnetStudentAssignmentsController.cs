using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;

namespace SEA_Application.Controllers
{
    public class AspnetStudentAssignmentsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspnetStudentAssignments
        public ActionResult Index()
        {
            var aspnetStudentAssignments = db.AspnetStudentAssignments.Include(a => a.AspnetLesson);
            return View(aspnetStudentAssignments.ToList());
        }

        // GET: AspnetStudentAssignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetStudentAssignment aspnetStudentAssignment = db.AspnetStudentAssignments.Find(id);
            if (aspnetStudentAssignment == null)
            {
                return HttpNotFound();
            }
            return View(aspnetStudentAssignment);
        }

        // GET: AspnetStudentAssignments/Create
        public ActionResult Create()
        {
            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name");
            return View();
        }

        // POST: AspnetStudentAssignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( AspnetStudentAssignment aspnetStudentAssignment)
        {
            HttpPostedFileBase file = Request.Files["Assignment"];
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Content/StudentAssignments/") + fileName);
                    aspnetStudentAssignment.FileName = fileName;

                }else
                {
                    aspnetStudentAssignment.FileName = "-/-";
                }
                aspnetStudentAssignment.CreationDate = DateTime.Now;
                db.AspnetStudentAssignments.Add(aspnetStudentAssignment);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name", aspnetStudentAssignment.LessonId);
            return View(aspnetStudentAssignment);
        }

        // GET: AspnetStudentAssignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetStudentAssignment aspnetStudentAssignment = db.AspnetStudentAssignments.Find(id);
            if (aspnetStudentAssignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name", aspnetStudentAssignment.LessonId);
            return View(aspnetStudentAssignment);
        }

        // POST: AspnetStudentAssignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,FileName,LessonId,DueDate,Path,CreationDate")] AspnetStudentAssignment aspnetStudentAssignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnetStudentAssignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name", aspnetStudentAssignment.LessonId);
            return View(aspnetStudentAssignment);
        }

        // GET: AspnetStudentAssignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetStudentAssignment aspnetStudentAssignment = db.AspnetStudentAssignments.Find(id);
            if (aspnetStudentAssignment == null)
            {
                return HttpNotFound();
            }
            return View(aspnetStudentAssignment);
        }

        // POST: AspnetStudentAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspnetStudentAssignment aspnetStudentAssignment = db.AspnetStudentAssignments.Find(id);
            db.AspnetStudentAssignments.Remove(aspnetStudentAssignment);
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
