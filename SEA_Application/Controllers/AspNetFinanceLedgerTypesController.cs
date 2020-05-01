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
    public class AspNetFinanceLedgerTypesController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetFinanceLedgerTypes
        public ActionResult Index()
        {
            return View(db.AspNetFinanceLedgerTypes.ToList());
        }

        // GET: AspNetFinanceLedgerTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFinanceLedgerType aspNetFinanceLedgerType = db.AspNetFinanceLedgerTypes.Find(id);
            if (aspNetFinanceLedgerType == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFinanceLedgerType);
        }

        // GET: AspNetFinanceLedgerTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetFinanceLedgerTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] AspNetFinanceLedgerType aspNetFinanceLedgerType)
        {
            if (ModelState.IsValid)
            {
                db.AspNetFinanceLedgerTypes.Add(aspNetFinanceLedgerType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetFinanceLedgerType);
        }

        // GET: AspNetFinanceLedgerTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFinanceLedgerType aspNetFinanceLedgerType = db.AspNetFinanceLedgerTypes.Find(id);
            if (aspNetFinanceLedgerType == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFinanceLedgerType);
        }

        // POST: AspNetFinanceLedgerTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AspNetFinanceLedgerType aspNetFinanceLedgerType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetFinanceLedgerType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetFinanceLedgerType);
        }

        // GET: AspNetFinanceLedgerTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFinanceLedgerType aspNetFinanceLedgerType = db.AspNetFinanceLedgerTypes.Find(id);
            if (aspNetFinanceLedgerType == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFinanceLedgerType);
        }

        // POST: AspNetFinanceLedgerTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetFinanceLedgerType aspNetFinanceLedgerType = db.AspNetFinanceLedgerTypes.Find(id);
            db.AspNetFinanceLedgerTypes.Remove(aspNetFinanceLedgerType);
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
