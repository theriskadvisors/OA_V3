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
    public class VoucherRecordsController : Controller
    {
        private SEA_DatabaseEntities db = new  SEA_DatabaseEntities();

        // GET: VoucherRecords
        //public ActionResult Index()
        //{
        //    var voucherRecords = db.VoucherRecords.Include(v => v.Ledger).Include(v => v.Voucher);
        //    return View(voucherRecords.ToList());
        //}
        public ActionResult VoucherRecord()
        {
            return View();
        }
        public JsonResult GetCurrentBalance(int ledgerid)
        {
            var SB = db.Ledgers.Where(x => x.Id == ledgerid).Select(x => x.StartingBalance).FirstOrDefault();
            return Json(SB,JsonRequestBehavior.AllowGet);
        }
        public JsonResult AfterBalance(int amount,int ledgerid,string type)
        {
            var currentbalance = db.Ledgers.Where(x => x.Id == ledgerid).Select(x => x.CurrentBalance).FirstOrDefault();
            var afterbalance= currentbalance-amount;
            //Ledger lg = db.Ledgers.Where(x => x.Id == ledgerid).FirstOrDefault();
            //lg.CurrentBalance = afterbalance;
            //db.SaveChanges();
            return Json(afterbalance,JsonRequestBehavior.AllowGet);
        }
        // GET: VoucherRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VoucherRecord voucherRecord = db.VoucherRecords.Find(id);
            if (voucherRecord == null)
            {
                return HttpNotFound();
            }
            return View(voucherRecord);
        }

        // GET: VoucherRecords/Create
        public ActionResult Create()
        {
            ViewBag.LedgerId = new SelectList(db.Ledgers, "Id", "Name");
            ViewBag.VoucherId = new SelectList(db.Vouchers, "Id", "Name");
            return View();
        }

        // POST: VoucherRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LedgerId,Type,Amount,CurrentBalance,AfterBalance,VoucherId")] VoucherRecord voucherRecord)
        {
            if (ModelState.IsValid)
            {
                db.VoucherRecords.Add(voucherRecord);
                db.SaveChanges();
                return RedirectToAction("VoucherRecord");
            }

            ViewBag.LedgerId = new SelectList(db.Ledgers, "Id", "Name", voucherRecord.LedgerId);
            ViewBag.VoucherId = new SelectList(db.Vouchers, "Id", "Name", voucherRecord.VoucherId);
            return View(voucherRecord);
        }

        // GET: VoucherRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VoucherRecord voucherRecord = db.VoucherRecords.Find(id);
            if (voucherRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.LedgerId = new SelectList(db.Ledgers, "Id", "Name", voucherRecord.LedgerId);
            ViewBag.VoucherId = new SelectList(db.Vouchers, "Id", "Name", voucherRecord.VoucherId);
            return View(voucherRecord);
        }

        // POST: VoucherRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LedgerId,Type,Amount,CurrentBalance,AfterBalance,VoucherId")] VoucherRecord voucherRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(voucherRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("VoucherRecord");
            }
            ViewBag.LedgerId = new SelectList(db.Ledgers, "Id", "Name", voucherRecord.LedgerId);
            ViewBag.VoucherId = new SelectList(db.Vouchers, "Id", "Name", voucherRecord.VoucherId);
            return View(voucherRecord);
        }

        // GET: VoucherRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VoucherRecord voucherRecord = db.VoucherRecords.Find(id);
            if (voucherRecord == null)
            {
                return HttpNotFound();
            }
            return View(voucherRecord);
        }

        // POST: VoucherRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VoucherRecord voucherRecord = db.VoucherRecords.Find(id);
            db.VoucherRecords.Remove(voucherRecord);
            db.SaveChanges();
            return RedirectToAction("VoucherRecord");
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
