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
    public class FeeDiscountsController : Controller
    {
        private SEA_DatabaseEntities db = new  SEA_DatabaseEntities();

        // GET: FeeDiscounts
        //public ActionResult Index()
        //{
        //    return View(db.FeeDiscounts.ToList());
        //}
        public ActionResult FeeDiscountIndex()
        {
            return View();
        }
        public ActionResult GetDiscounts()
        {

            var discountlist = db.FeeDiscounts.ToList();
            List<FeeDiscount> duratintype = new List<FeeDiscount>();
            foreach (var item in discountlist)
            {
                FeeDiscount dt = new  FeeDiscount();
                dt.Id = item.Id;
                dt.Name = item.Name;
                dt.Amount = item.Amount;
                duratintype.Add(dt);
            }
            return Json(duratintype, JsonRequestBehavior.AllowGet);
        }
        // GET: FeeDiscounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeDiscount feeDiscount = db.FeeDiscounts.Find(id);
            if (feeDiscount == null)
            {
                return HttpNotFound();
            }
            return View(feeDiscount);
        }

        // GET: FeeDiscounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeeDiscounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Amount")] FeeDiscount feeDiscount)
        {
            if (ModelState.IsValid)
            {
                db.FeeDiscounts.Add(feeDiscount);
                db.SaveChanges();
                return RedirectToAction("FeeDiscountIndex");
            }
            return View(feeDiscount);
        }

        // GET: FeeDiscounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeDiscount feeDiscount = db.FeeDiscounts.Find(id);
            if (feeDiscount == null)
            {
                return HttpNotFound();
            }
            return View(feeDiscount);
        }

        // POST: FeeDiscounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Amount")] FeeDiscount feeDiscount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feeDiscount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("FeeDiscountIndex");
            }
            return View(feeDiscount);
        }

        // GET: FeeDiscounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeDiscount feeDiscount = db.FeeDiscounts.Find(id);
            if (feeDiscount == null)
            {
                return HttpNotFound();
            }
            return View(feeDiscount);
        }

        // POST: FeeDiscounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FeeDiscount feeDiscount = db.FeeDiscounts.Find(id);
            db.FeeDiscounts.Remove(feeDiscount);
            db.SaveChanges();
            return RedirectToAction("FeeDiscountIndex");
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
