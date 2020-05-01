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
    public class AspNetFeeTypeController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetFeeType
        public ActionResult Index()
        {
            var aspNetFeeTypes = db.AspNetFeeTypes.Include(a => a.AspNetFinanceLedger);
            return View(aspNetFeeTypes.ToList());
        }

        // GET: AspNetFeeType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFeeType aspNetFeeType = db.AspNetFeeTypes.Find(id);
            ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Name", aspNetFeeType.LedgerID);
            if (aspNetFeeType == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFeeType);
        }

        // GET: AspNetFeeType/Create
        public ActionResult Create()
        {
            ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Name");
            return View();
        }

        // POST: AspNetFeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LedgerID")] AspNetFeeType aspNetFeeType)
        {
            if (ModelState.IsValid)
            {
                db.AspNetFeeTypes.Add(aspNetFeeType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Name", aspNetFeeType.LedgerID);
            return View(aspNetFeeType);
        }

        // GET: AspNetFeeType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFeeType aspNetFeeType = db.AspNetFeeTypes.Find(id);
            if (aspNetFeeType == null)
            {
                return HttpNotFound();
            }
            ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Name", aspNetFeeType.LedgerID);
            return View(aspNetFeeType);
        }

        // POST: AspNetFeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LedgerID")] AspNetFeeType aspNetFeeType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetFeeType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Name", aspNetFeeType.LedgerID);
            return View(aspNetFeeType);
        }

        // GET: AspNetFeeType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFeeType aspNetFeeType = db.AspNetFeeTypes.Find(id);
            if (aspNetFeeType == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFeeType);
        }

        // POST: AspNetFeeType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetFeeType aspNetFeeType = db.AspNetFeeTypes.Find(id);
            db.AspNetFeeTypes.Remove(aspNetFeeType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCnfm(int id)
        {
            AspNetFeeType aspNetFeeType = db.AspNetFeeTypes.Find(id);
            try
            {
                db.AspNetFeeTypes.Remove(aspNetFeeType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "This fee can't be deleted.";
                return View("Details", aspNetFeeType);
            }
                       
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
