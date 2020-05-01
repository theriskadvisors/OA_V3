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
    public class AspNetStudent_PaymentDetailController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetStudent_PaymentDetail
        public ActionResult Index(int STAID)
        {
            var aspNetStudent_PaymentDetail = db.AspNetStudent_PaymentDetail.Include(a => a.AspNetStudent_Payment).Where(x => x.Student_PaymentID == STAID);
            return View(aspNetStudent_PaymentDetail.ToList());
        }

        // GET: AspNetStudent_PaymentDetail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_PaymentDetail aspNetStudent_PaymentDetail = db.AspNetStudent_PaymentDetail.Find(id);
            if (aspNetStudent_PaymentDetail == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudent_PaymentDetail);
        }

        // GET: AspNetStudent_PaymentDetail/Create
        public ActionResult Create()
        {
            ViewBag.Student_PaymentID = new SelectList(db.AspNetStudent_Payment, "Id", "StudentID");
            return View();
        }

        // POST: AspNetStudent_PaymentDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Student_PaymentID,FeeType,Amount")] AspNetStudent_PaymentDetail aspNetStudent_PaymentDetail)
        {
            if (ModelState.IsValid)
            {
                db.AspNetStudent_PaymentDetail.Add(aspNetStudent_PaymentDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Student_PaymentID = new SelectList(db.AspNetStudent_Payment, "Id", "StudentID", aspNetStudent_PaymentDetail.Student_PaymentID);
            return View(aspNetStudent_PaymentDetail);
        }

        // GET: AspNetStudent_PaymentDetail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_PaymentDetail aspNetStudent_PaymentDetail = db.AspNetStudent_PaymentDetail.Find(id);
            if (aspNetStudent_PaymentDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.Student_PaymentID = new SelectList(db.AspNetStudent_Payment, "Id", "StudentID", aspNetStudent_PaymentDetail.Student_PaymentID);
            return View(aspNetStudent_PaymentDetail);
        }

        // POST: AspNetStudent_PaymentDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Student_PaymentID,FeeType,Amount")] AspNetStudent_PaymentDetail aspNetStudent_PaymentDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetStudent_PaymentDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Student_PaymentID = new SelectList(db.AspNetStudent_Payment, "Id", "StudentID", aspNetStudent_PaymentDetail.Student_PaymentID);
            return View(aspNetStudent_PaymentDetail);
        }

        // GET: AspNetStudent_PaymentDetail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_PaymentDetail aspNetStudent_PaymentDetail = db.AspNetStudent_PaymentDetail.Find(id);
            if (aspNetStudent_PaymentDetail == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudent_PaymentDetail);
        }

        // POST: AspNetStudent_PaymentDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetStudent_PaymentDetail aspNetStudent_PaymentDetail = db.AspNetStudent_PaymentDetail.Find(id);
            db.AspNetStudent_PaymentDetail.Remove(aspNetStudent_PaymentDetail);
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
