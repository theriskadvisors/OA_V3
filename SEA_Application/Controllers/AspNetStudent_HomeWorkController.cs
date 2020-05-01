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
    public class AspNetStudent_HomeWorkController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetStudent_HomeWork
        public ActionResult Index(int HomeWorkId)
        {
            ViewBag.HomeWorkId = db.AspNetHomeworks.Where(x => x.Id == HomeWorkId).FirstOrDefault().PrincipalApproved_status;
            var aspNetStudent_HomeWork = db.AspNetStudent_HomeWork.Where(x=>x.HomeworkID== HomeWorkId).Include(a => a.AspNetHomework).Include(a => a.AspNetUser);
            
            return View(aspNetStudent_HomeWork.ToList());
        }

       
        // GET: AspNetStudent_HomeWork/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_HomeWork aspNetStudent_HomeWork = db.AspNetStudent_HomeWork.Find(id);
            if (aspNetStudent_HomeWork == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudent_HomeWork);
        }

        // GET: AspNetStudent_HomeWork/Create
        public ActionResult Create()
        {
            ViewBag.HomeworkID = new SelectList(db.AspNetHomeworks, "Id", "Id");
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AspNetStudent_HomeWork/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentID,HomeworkID,Reading,TeacherComments,ParentComments,Status")] AspNetStudent_HomeWork aspNetStudent_HomeWork)
        {
            if (ModelState.IsValid)
            {
                db.AspNetStudent_HomeWork.Add(aspNetStudent_HomeWork);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HomeworkID = new SelectList(db.AspNetHomeworks, "Id", "Id", aspNetStudent_HomeWork.HomeworkID);
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_HomeWork.StudentID);
            return View(aspNetStudent_HomeWork);
        }

        // GET: AspNetStudent_HomeWork/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_HomeWork aspNetStudent_HomeWork = db.AspNetStudent_HomeWork.Find(id);
            if (aspNetStudent_HomeWork == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomeworkID = new SelectList(db.AspNetHomeworks, "Id", "Id", aspNetStudent_HomeWork.HomeworkID);
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_HomeWork.StudentID);

            return View(aspNetStudent_HomeWork);
        }

        // POST: AspNetStudent_HomeWork/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentID,HomeworkID,Reading,TeacherComments,ParentComments,Status")] AspNetStudent_HomeWork aspNetStudent_HomeWork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetStudent_HomeWork).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { HomeWorkId = aspNetStudent_HomeWork.HomeworkID });
            }
            ViewBag.HomeworkID = new SelectList(db.AspNetHomeworks, "Id", "Id", aspNetStudent_HomeWork.HomeworkID);
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_HomeWork.StudentID);
            return View(aspNetStudent_HomeWork);
        }

        // GET: AspNetStudent_HomeWork/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_HomeWork aspNetStudent_HomeWork = db.AspNetStudent_HomeWork.Find(id);
            if (aspNetStudent_HomeWork == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudent_HomeWork);
        }

        // POST: AspNetStudent_HomeWork/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetStudent_HomeWork aspNetStudent_HomeWork = db.AspNetStudent_HomeWork.Find(id);
            db.AspNetStudent_HomeWork.Remove(aspNetStudent_HomeWork);
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
