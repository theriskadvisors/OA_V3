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
    public class PenaltyFeesController : Controller
    {
        private SEA_DatabaseEntities db = new  SEA_DatabaseEntities();

        // GET: PenaltyFees
        //public ActionResult Index()
        //{
        //    return View(db.PenaltyFees.ToList());
        //}
        public ActionResult PenaltyFeeIndex()
        {
            return View();
        }
        public ActionResult GetPenalty()
        {
            var durationlist = db.PenaltyFees.ToList();
            List<PenaltyFee> duratintype = new List<PenaltyFee>();
            foreach (var item in durationlist)
            {
                PenaltyFee dt = new  PenaltyFee();
                dt.Name = item.Name;
                dt.Amount = item.Amount;
                duratintype.Add(dt);
            }
            return Json(duratintype, JsonRequestBehavior.AllowGet);
        }
        // GET: PenaltyFees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyFee penaltyFee = db.PenaltyFees.Find(id);
            if (penaltyFee == null)
            {
                return HttpNotFound();
            }
            return View(penaltyFee);
        }

        // GET: PenaltyFees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PenaltyFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Amount")] PenaltyFee penaltyFee)
        {
            if (ModelState.IsValid)
            {
                db.PenaltyFees.Add(penaltyFee);
                db.SaveChanges();
                return RedirectToAction("PenaltyFeeIndex");
            }

            return View(penaltyFee);
        }

        // GET: PenaltyFees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyFee penaltyFee = db.PenaltyFees.Find(id);
            if (penaltyFee == null)
            {
                return HttpNotFound();
            }
            return View(penaltyFee);
        }

        // POST: PenaltyFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Amount")] PenaltyFee penaltyFee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(penaltyFee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PenaltyFeeIndex");
            }
            return View(penaltyFee);
        }

        // GET: PenaltyFees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyFee penaltyFee = db.PenaltyFees.Find(id);
            if (penaltyFee == null)
            {
                return HttpNotFound();
            }
            return View(penaltyFee);
        }

        // POST: PenaltyFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PenaltyFee penaltyFee = db.PenaltyFees.Find(id);
            db.PenaltyFees.Remove(penaltyFee);
            db.SaveChanges();
            return RedirectToAction("PenaltyFeeIndex");
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
