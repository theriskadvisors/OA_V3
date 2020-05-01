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
    public class TeacherPenaltyRecordsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: TeacherPenaltyRecords
        public ActionResult Index()
        {
            var teacherPenaltyRecords = db.TeacherPenaltyRecords.Include(t => t.AspNetEmployee);
            return View(teacherPenaltyRecords.ToList());
        }

        // GET: TeacherPenaltyRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherPenaltyRecord teacherPenaltyRecord = db.TeacherPenaltyRecords.Find(id);
            if (teacherPenaltyRecord == null)
            {
                return HttpNotFound();
            }
            return View(teacherPenaltyRecord);
        }

        // GET: TeacherPenaltyRecords/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees, "Id", "Name");
            return View();
        }

        // POST: TeacherPenaltyRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeId,PenaltyType,Description,Amount,Months,Status,Time")] TeacherPenaltyRecord teacherPenaltyRecord)
        {
            if (ModelState.IsValid)
            {
                teacherPenaltyRecord.Status = "Created";
                
                db.TeacherPenaltyRecords.Add(teacherPenaltyRecord);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees, "Id", "PositionAppliedFor", teacherPenaltyRecord.EmployeeId);
            return View(teacherPenaltyRecord);
        }

        // GET: TeacherPenaltyRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherPenaltyRecord teacherPenaltyRecord = db.TeacherPenaltyRecords.Find(id);
            if (teacherPenaltyRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees, "Id", "PositionAppliedFor", teacherPenaltyRecord.EmployeeId);
            return View(teacherPenaltyRecord);
        }

        // POST: TeacherPenaltyRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId,PenaltyType,Description,Amount,Months,Status,Time")] TeacherPenaltyRecord teacherPenaltyRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacherPenaltyRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees, "Id", "PositionAppliedFor", teacherPenaltyRecord.EmployeeId);
            return View(teacherPenaltyRecord);
        }

        // GET: TeacherPenaltyRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherPenaltyRecord teacherPenaltyRecord = db.TeacherPenaltyRecords.Find(id);
            if (teacherPenaltyRecord == null)
            {
                return HttpNotFound();
            }
            return View(teacherPenaltyRecord);
        }

        // POST: TeacherPenaltyRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TeacherPenaltyRecord teacherPenaltyRecord = db.TeacherPenaltyRecords.Find(id);
            db.TeacherPenaltyRecords.Remove(teacherPenaltyRecord);
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
