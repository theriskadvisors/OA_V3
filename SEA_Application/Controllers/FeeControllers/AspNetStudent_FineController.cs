using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;

namespace SEA_Application.Controllers.FeeControllers
{
    public class AspNetStudent_FineController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetStudent_Fine
        public ActionResult Index()
        {
            var aspNetStudent_Fine = db.AspNetStudent_Fine.Include(a => a.AspNetUser);
            return View(aspNetStudent_Fine.ToList());
        }

        // GET: AspNetStudent_Fine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_Fine aspNetStudent_Fine = db.AspNetStudent_Fine.Find(id);
            if (aspNetStudent_Fine == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudent_Fine);
        }

        // GET: AspNetStudent_Fine/Create
        public ActionResult Create()
        {
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AspNetStudent_Fine/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentID,FineDetail,Amount,Status,Date")] AspNetStudent_Fine aspNetStudent_Fine)
        {
            if (ModelState.IsValid)
            {
                db.AspNetStudent_Fine.Add(aspNetStudent_Fine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Fine.StudentID);
            return View(aspNetStudent_Fine);
        }

        // GET: AspNetStudent_Fine/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_Fine aspNetStudent_Fine = db.AspNetStudent_Fine.Find(id);
            if (aspNetStudent_Fine == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Fine.StudentID);
            return View(aspNetStudent_Fine);
        }

        // POST: AspNetStudent_Fine/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentID,FineDetail,Amount,Status,Date")] AspNetStudent_Fine aspNetStudent_Fine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetStudent_Fine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Fine.StudentID);
            return View(aspNetStudent_Fine);
        }

        // GET: AspNetStudent_Fine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_Fine aspNetStudent_Fine = db.AspNetStudent_Fine.Find(id);
            if (aspNetStudent_Fine == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudent_Fine);
        }

        // POST: AspNetStudent_Fine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetStudent_Fine aspNetStudent_Fine = db.AspNetStudent_Fine.Find(id);
            db.AspNetStudent_Fine.Remove(aspNetStudent_Fine);
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
