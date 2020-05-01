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
//    public class NonRecurringMonthsController : Controller
//    {
//        private SEA_DatabaseEntities db = new  SEA_DatabaseEntities();

//        // GET: NonRecurringMonths
//        public ActionResult Index()
//        {
//            var nonRecurringMonths = db.NonRecurringMonths.Include(n => n.AspNetStudent);
//            return View(nonRecurringMonths.ToList());
//        }

//        // GET: NonRecurringMonths/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            NonRecurringMonth nonRecurringMonth = db.NonRecurringMonths.Find(id);
//            if (nonRecurringMonth == null)
//            {
//                return HttpNotFound();
//            }
//            return View(nonRecurringMonth);
//        }

//        // GET: NonRecurringMonths/Create
//        public ActionResult Create()
//        {
//            ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name");
//            return View();
//        }

//        // POST: NonRecurringMonths/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,StudentId,Months,Status,InstalmentAmount,IssueDate")] NonRecurringMonth nonRecurringMonth)
//        {
//            if (ModelState.IsValid)
//            {
//                db.NonRecurringMonths.Add(nonRecurringMonth);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", nonRecurringMonth.StudentId);
//            return View(nonRecurringMonth);
//        }

//        // GET: NonRecurringMonths/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            NonRecurringMonth nonRecurringMonth = db.NonRecurringMonths.Find(id);
//            if (nonRecurringMonth == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", nonRecurringMonth.StudentId);
//            return View(nonRecurringMonth);
//        }

//        // POST: NonRecurringMonths/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,StudentId,Months,Status,InstalmentAmount,IssueDate")] NonRecurringMonth nonRecurringMonth)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(nonRecurringMonth).State = EntityState.Modified;
//                db.SaveChanges();

//                if (nonRecurringMonth.Status == "Clear")
//                {
//                    NonRecurringFeeMultiplier nonrec = db.NonRecurringFeeMultipliers.Where(x => x.StudentId == nonRecurringMonth.StudentId).Select(x => x).FirstOrDefault();
//                    nonrec.PaidAmount += nonRecurringMonth.InstalmentAmount;
//                    nonrec.PaidInstalments += 1;
//                    nonrec.RemainingAmount -= nonRecurringMonth.InstalmentAmount;
//                    nonrec.RemainingInstalments -= 1;
//                    db.SaveChanges();

//                    Ledger ledger = db.Ledgers.Where(x => x.Name == "Student Receivables").FirstOrDefault();
//                    ledger.CurrentBalance -= Convert.ToDecimal(nonRecurringMonth.InstalmentAmount);
//                    db.SaveChanges();
//                }

//                return RedirectToAction("Index");
//            }
//            ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", nonRecurringMonth.StudentId);
//            return View(nonRecurringMonth);
//        }

//        // GET: NonRecurringMonths/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            NonRecurringMonth nonRecurringMonth = db.NonRecurringMonths.Find(id);
//            if (nonRecurringMonth == null)
//            {
//                return HttpNotFound();
//            }
//            return View(nonRecurringMonth);
//        }

//        // POST: NonRecurringMonths/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            NonRecurringMonth nonRecurringMonth = db.NonRecurringMonths.Find(id);
//            db.NonRecurringMonths.Remove(nonRecurringMonth);
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
