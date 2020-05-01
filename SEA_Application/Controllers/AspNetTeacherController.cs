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
    [Authorize(Roles = "Admin")]
    public class AspNetTeacherController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetTeacher
        public ActionResult Index()
        {
            var aspNetTeachers = db.AspNetTeachers.Include(a => a.AspNetUser);
            return View(aspNetTeachers.ToList());
        }

        // GET: AspNetTeacher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetTeacher aspNetTeacher = db.AspNetTeachers.Find(id);
            if (aspNetTeacher == null)
            {
                return HttpNotFound();
            }
            return View(aspNetTeacher);
        }

        // GET: AspNetTeacher/Create
        public ActionResult Create()
        {
            ViewBag.TeacherID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AspNetTeacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TeacherID,JoiningDate")] AspNetTeacher aspNetTeacher)
        {
            var TransactionObj = db.Database.BeginTransaction();
            try
            {
                
                if (ModelState.IsValid)
            {
                db.AspNetTeachers.Add(aspNetTeacher);
                db.SaveChanges();
            }
                TransactionObj.Commit();
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                var UserNameLog = User.Identity.Name;
                AspNetUser a = db.AspNetUsers.First(x => x.UserName == UserNameLog);
                string UserIDLog = a.Id;

                var logMessage = "New Teacher Added, TeacherID: " + aspNetTeacher.TeacherID + ", JoiningDate: " + aspNetTeacher.JoiningDate;

                var LogControllerObj = new AspNetLogsController();
                LogControllerObj.CreateLogSave(logMessage, UserIDLog);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                return RedirectToAction("Index");
            }
            catch { TransactionObj.Dispose(); }

            ViewBag.TeacherID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetTeacher.TeacherID);
            return View(aspNetTeacher);
        }

        // GET: AspNetTeacher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetTeacher aspNetTeacher = db.AspNetTeachers.Find(id);
            if (aspNetTeacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeacherID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetTeacher.TeacherID);
            return View(aspNetTeacher);
        }

        // POST: AspNetTeacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TeacherID,JoiningDate")] AspNetTeacher aspNetTeacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetTeacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeacherID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetTeacher.TeacherID);
            return View(aspNetTeacher);
        }

        // GET: AspNetTeacher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetTeacher aspNetTeacher = db.AspNetTeachers.Find(id);
            if (aspNetTeacher == null)
            {
                return HttpNotFound();
            }
            return View(aspNetTeacher);
        }

        // POST: AspNetTeacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetTeacher aspNetTeacher = db.AspNetTeachers.Find(id);
            db.AspNetTeachers.Remove(aspNetTeacher);
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
