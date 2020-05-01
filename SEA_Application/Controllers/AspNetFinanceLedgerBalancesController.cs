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
    public class AspNetFinanceLedgerBalancesController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        //// GET: AspNetFinanceLedgerBalances
        //public ActionResult Index()
        //{
        //    var aspNetFinanceLedgerBalances = db.AspNetFinanceLedgerBalances.Include(a => a.AspNetFinanceLedger);
        //    return View(aspNetFinanceLedgerBalances.ToList());
        //}

        //// GET: AspNetFinanceLedgerBalances/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AspNetFinanceLedgerBalance aspNetFinanceLedgerBalance = db.AspNetFinanceLedgerBalances.Find(id);
        //    if (aspNetFinanceLedgerBalance == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(aspNetFinanceLedgerBalance);
        //}

        //// GET: AspNetFinanceLedgerBalances/Create
        //public ActionResult Create()
        //{
        //    ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Code");
        //    return View();
        //}

        //// POST: AspNetFinanceLedgerBalances/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,LedgerID,OldBalance,NewBalance")] AspNetFinanceLedgerBalance aspNetFinanceLedgerBalance)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.AspNetFinanceLedgerBalances.Add(aspNetFinanceLedgerBalance);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Code", aspNetFinanceLedgerBalance.LedgerID);
        //    return View(aspNetFinanceLedgerBalance);
        //}

        //// GET: AspNetFinanceLedgerBalances/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AspNetFinanceLedgerBalance aspNetFinanceLedgerBalance = db.AspNetFinanceLedgerBalances.Find(id);
        //    if (aspNetFinanceLedgerBalance == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Code", aspNetFinanceLedgerBalance.LedgerID);
        //    return View(aspNetFinanceLedgerBalance);
        //}

        //// POST: AspNetFinanceLedgerBalances/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,LedgerID,OldBalance,NewBalance")] AspNetFinanceLedgerBalance aspNetFinanceLedgerBalance)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(aspNetFinanceLedgerBalance).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Code", aspNetFinanceLedgerBalance.LedgerID);
        //    return View(aspNetFinanceLedgerBalance);
        //}

        //// GET: AspNetFinanceLedgerBalances/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AspNetFinanceLedgerBalance aspNetFinanceLedgerBalance = db.AspNetFinanceLedgerBalances.Find(id);
        //    if (aspNetFinanceLedgerBalance == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(aspNetFinanceLedgerBalance);
        //}

        //// POST: AspNetFinanceLedgerBalances/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    AspNetFinanceLedgerBalance aspNetFinanceLedgerBalance = db.AspNetFinanceLedgerBalances.Find(id);
        //    db.AspNetFinanceLedgerBalances.Remove(aspNetFinanceLedgerBalance);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        
    }
}
