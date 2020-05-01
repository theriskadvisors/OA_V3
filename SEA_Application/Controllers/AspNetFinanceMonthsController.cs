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
    public class AspNetFinanceMonthsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetFinanceMonths
        public ActionResult Index()
        {
            var aspNetFinanceMonths = db.AspNetFinanceMonths.Include(a => a.AspNetFinancePeriod);
            return View(aspNetFinanceMonths.ToList());
        }

        // GET: AspNetFinanceMonths/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFinanceMonth aspNetFinanceMonth = db.AspNetFinanceMonths.Find(id);
            if (aspNetFinanceMonth == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFinanceMonth);
        }

        // GET: AspNetFinanceMonths/Create
        public ActionResult Create()
        {
            ViewBag.PeriodId = new SelectList(db.AspNetFinancePeriods, "Id", "Year");
            return View();
        }

        // POST: AspNetFinanceMonths/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Month,StartData,EndDate,Name,PeriodId")] AspNetFinanceMonth aspNetFinanceMonth)
        {
            if (ModelState.IsValid)
            {
                db.AspNetFinanceMonths.Add(aspNetFinanceMonth);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PeriodId = new SelectList(db.AspNetFinancePeriods, "Id", "Year", aspNetFinanceMonth.PeriodId);
            return View(aspNetFinanceMonth);
        }

        // GET: AspNetFinanceMonths/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFinanceMonth aspNetFinanceMonth = db.AspNetFinanceMonths.Find(id);
            if (aspNetFinanceMonth == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeriodId = new SelectList(db.AspNetFinancePeriods, "Id", "Year", aspNetFinanceMonth.PeriodId);
            return View(aspNetFinanceMonth);
        }

        // POST: AspNetFinanceMonths/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Month,StartData,EndDate,Name,PeriodId")] AspNetFinanceMonth aspNetFinanceMonth)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetFinanceMonth).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PeriodId = new SelectList(db.AspNetFinancePeriods, "Id", "Year", aspNetFinanceMonth.PeriodId);
            return View(aspNetFinanceMonth);
        }

        // GET: AspNetFinanceMonths/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFinanceMonth aspNetFinanceMonth = db.AspNetFinanceMonths.Find(id);
            if (aspNetFinanceMonth == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFinanceMonth);
        }

        // POST: AspNetFinanceMonths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetFinanceMonth aspNetFinanceMonth = db.AspNetFinanceMonths.Find(id);
            db.AspNetFinanceMonths.Remove(aspNetFinanceMonth);
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
