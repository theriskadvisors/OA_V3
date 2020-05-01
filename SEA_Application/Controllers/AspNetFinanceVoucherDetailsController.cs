//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using SEA_Application.Models;


//namespace SEA_Application.Controllers
//{
//    public class AspNetFinanceVoucherDetailsController : Controller
//    {
//        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

//        // GET: AspNetFinanceVoucherDetails
//        public ActionResult Index()
//        {
//            var aspNetFinanceVoucherDetails = db.AspNetFinanceVoucherDetails.Include(a => a.AspNetFinanceLedger).Include(a => a.AspNetFinanceLedgerType).Include(a => a.AspNetFinanceVoucher);
//            return View(aspNetFinanceVoucherDetails.ToList());
//        }

//        // GET: AspNetFinanceVoucherDetails/Details/5

        
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFinanceVoucherDetail aspNetFinanceVoucherDetail = db.AspNetFinanceVoucherDetails.Find(id);
//            if (aspNetFinanceVoucherDetail == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetFinanceVoucherDetail);
//        }

//        // GET: AspNetFinanceVoucherDetails/Create
//        public ActionResult Create()
//        {
//            ViewBag.LedgerId = new SelectList(db.AspNetFinanceLedgers, "Id", "Code");
//            ViewBag.LedgertypeId = new SelectList(db.AspNetFinanceLedgerTypes, "Id", "Name");
//            ViewBag.VoucherId = new SelectList(db.AspNetFinanceVouchers, "Id", "Status");
//            return View();
//        }

//        // POST: AspNetFinanceVoucherDetails/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,LedgertypeId,LedgerId,Credit,Debit,TransactionDescription,VoucherId")] AspNetFinanceVoucherDetail aspNetFinanceVoucherDetail)
//        {
//            if (ModelState.IsValid)
//            {
//                db.AspNetFinanceVoucherDetails.Add(aspNetFinanceVoucherDetail);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.LedgerId = new SelectList(db.AspNetFinanceLedgers, "Id", "Code", aspNetFinanceVoucherDetail.LedgerId);
//            ViewBag.LedgertypeId = new SelectList(db.AspNetFinanceLedgerTypes, "Id", "Name", aspNetFinanceVoucherDetail.LedgertypeId);
//            ViewBag.VoucherId = new SelectList(db.AspNetFinanceVouchers, "Id", "Status", aspNetFinanceVoucherDetail.VoucherId);
//            return View(aspNetFinanceVoucherDetail);
//        }

//        // GET: AspNetFinanceVoucherDetails/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFinanceVoucherDetail aspNetFinanceVoucherDetail = db.AspNetFinanceVoucherDetails.Find(id);
//            if (aspNetFinanceVoucherDetail == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.LedgerId = new SelectList(db.AspNetFinanceLedgers, "Id", "Code", aspNetFinanceVoucherDetail.LedgerId);
//            ViewBag.LedgertypeId = new SelectList(db.AspNetFinanceLedgerTypes, "Id", "Name", aspNetFinanceVoucherDetail.LedgertypeId);
//            ViewBag.VoucherId = new SelectList(db.AspNetFinanceVouchers, "Id", "Status", aspNetFinanceVoucherDetail.VoucherId);
//            return View(aspNetFinanceVoucherDetail);
//        }

//        // POST: AspNetFinanceVoucherDetails/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,LedgertypeId,LedgerId,Credit,Debit,TransactionDescription,VoucherId")] AspNetFinanceVoucherDetail aspNetFinanceVoucherDetail)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(aspNetFinanceVoucherDetail).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.LedgerId = new SelectList(db.AspNetFinanceLedgers, "Id", "Code", aspNetFinanceVoucherDetail.LedgerId);
//            ViewBag.LedgertypeId = new SelectList(db.AspNetFinanceLedgerTypes, "Id", "Name", aspNetFinanceVoucherDetail.LedgertypeId);
//            ViewBag.VoucherId = new SelectList(db.AspNetFinanceVouchers, "Id", "Status", aspNetFinanceVoucherDetail.VoucherId);
//            return View(aspNetFinanceVoucherDetail);
//        }

//        // GET: AspNetFinanceVoucherDetails/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetFinanceVoucherDetail aspNetFinanceVoucherDetail = db.AspNetFinanceVoucherDetails.Find(id);
//            if (aspNetFinanceVoucherDetail == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetFinanceVoucherDetail);
//        }

//        // POST: AspNetFinanceVoucherDetails/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            AspNetFinanceVoucherDetail aspNetFinanceVoucherDetail = db.AspNetFinanceVoucherDetails.Find(id);
//            db.AspNetFinanceVoucherDetails.Remove(aspNetFinanceVoucherDetail);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
