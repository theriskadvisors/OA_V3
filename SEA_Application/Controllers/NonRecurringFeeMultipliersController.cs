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
    public class NonRecurringFeeMultipliersController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: NonRecurringFeeMultipliers
        public ActionResult Index()
        {
            var nonRecurringFeeMultipliers = db.NonRecurringFeeMultipliers.Include(n => n.NonRecurringCharge).Include(x => x.AspNetStudent);
            return View(nonRecurringFeeMultipliers.ToList());
        }

        // GET: NonRecurringFeeMultipliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NonRecurringFeeMultiplier nonRecurringFeeMultiplier = db.NonRecurringFeeMultipliers.Find(id);
            if (nonRecurringFeeMultiplier == null)
            {
                return HttpNotFound();
            }
            return View(nonRecurringFeeMultiplier);
        }

        // GET: NonRecurringFeeMultipliers/Create
        public ActionResult Create()
        {
            ViewBag.ClassId = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.StudentId = new SelectList(db.AspNetStudents.Where(x=>x.AspNetUser.Status!="False"), "Id", "Name");
            ViewBag.DescriptionId = new SelectList(db.NonRecurringCharges, "Id", "ExpenseType");
            return View();
        }

        // POST: NonRecurringFeeMultipliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,DescriptionId,Date,TutionFee,PaidAmount,RemainingAmount,SharePerInstalment,Instalments,PaidInstalments,RemainingInstalments,Multiplier")] NonRecurringFeeMultiplier nonRecurringFeeMultiplier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var classid = Convert.ToInt32(Request.Form["ClassId"]);
                    var Month = Request.Form["Month"];

                    var stdid = db.AspNetStudents.Where(x => x.ClassID == classid && x.AspNetUser.Status!="False").Select(x => x.Id).ToList();
                    foreach (var item in stdid)
                    {
                        nonRecurringFeeMultiplier.StudentId = item;
                        nonRecurringFeeMultiplier.Month = Month;
                        nonRecurringFeeMultiplier.Status = "Pending";
                        db.NonRecurringFeeMultipliers.Add(nonRecurringFeeMultiplier);
                        db.SaveChanges();
                    }
                    //foreach (var item in stdid)
                    //{
                    //    NonRecurringMonth stdfeemonth = new NonRecurringMonth();
                    //    stdfeemonth.StudentId = item;
                    //    stdfeemonth.Status = "Pending";
                    //    stdfeemonth.InstalmentAmount = nonRecurringFeeMultiplier.TutionFee;
                    //    var dddd = DateTime.Now;
                    //    var d = dddd.ToString("yyyy-MM-dd");
                    //    stdfeemonth.IssueDate = d;
                    //    stdfeemonth.Months = DateTime.Now.Month.ToString();
                    //    db.NonRecurringMonths.Add(stdfeemonth);
                    //    db.SaveChanges();
                    //}

                    //var stdList = db.AspNetStudents.Where(x => x.ClassID == classid).ToList().Count();
                    //for (int i = 0; i < stdList; i++)
                    //{
                    //    Ledger ledger = db.Ledgers.Where(x => x.Name == "Student Receivables").FirstOrDefault();
                    //    ledger.StartingBalance += Convert.ToDecimal(nonRecurringFeeMultiplier.TutionFee);
                    //    ledger.CurrentBalance += Convert.ToDecimal(nonRecurringFeeMultiplier.TutionFee);
                    //    db.SaveChanges();

                    //}

                    //for (int i = 0; i < stdList; i++)
                    //{
                    //    Ledger ledger = db.Ledgers.Where(x => x.Name == "Student Fee").FirstOrDefault();
                    //    ledger.StartingBalance += Convert.ToDecimal(nonRecurringFeeMultiplier.TutionFee);
                    //    ledger.CurrentBalance += Convert.ToDecimal(nonRecurringFeeMultiplier.TutionFee);
                    //    db.SaveChanges();

                    //}



                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            ViewBag.DescriptionId = new SelectList(db.NonRecurringCharges, "Id", "ExpenseType", nonRecurringFeeMultiplier.DescriptionId);
            return View(nonRecurringFeeMultiplier);
        }
        //public JsonResult EditMultiplier(int instal, int stdid)
        //{

        //    double? remaining = 0;
        //    var list = db.NonRecurringMonths.Where(x => x.StudentId == stdid && x.Status == "Pending").Select(x => x.InstalmentAmount).ToList();
        //    foreach (var item in list)
        //    {
        //        remaining += item;
        //    }
        //    var SharePerInstalment = remaining / instal;
        //    var RemainingInstalments = Convert.ToDouble(remaining / SharePerInstalment);
        //    var result = new { remaining, SharePerInstalment, RemainingInstalments };

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        // GET: NonRecurringFeeMultipliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NonRecurringFeeMultiplier nonRecurringFeeMultiplier = db.NonRecurringFeeMultipliers.Find(id);
            if (nonRecurringFeeMultiplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.DescriptionId = new SelectList(db.NonRecurringCharges, "Id", "ExpenseType", nonRecurringFeeMultiplier.DescriptionId);
            return View(nonRecurringFeeMultiplier);
        }

        // POST: NonRecurringFeeMultipliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,StudentId,DescriptionId,TutionFee,PaidAmount,RemainingAmount,SharePerInstalment,Instalments,PaidInstalments,RemainingInstalments,Multiplier")] NonRecurringFeeMultiplier nonRecurringFeeMultiplier)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var checkbox = Request.Form["MonthName"];
        //        if (checkbox == null)
        //        {
        //            ViewBag.ErrorMessage = "No Month selected, Please select Months";
        //        }
        //        else
        //        {
        //            List<string> names = new List<string>(checkbox.Split(','));

        //            if (names.Count != nonRecurringFeeMultiplier.Instalments)
        //            {
        //                ViewBag.ErrorMessage = "No. of Months and instalments are not same";
        //            }
        //            else
        //            {
        //                var tdfee = db.NonRecurringMonths.Where(x => x.StudentId == nonRecurringFeeMultiplier.StudentId && x.Status == "Pending").ToList();
        //                foreach (var item in tdfee)
        //                {
        //                    NonRecurringMonth std = db.NonRecurringMonths.Where(x => x.Id == item.Id).FirstOrDefault();
        //                    db.NonRecurringMonths.Remove(std);
        //                    db.SaveChanges();
        //                }


        //                foreach (var item in names)
        //                {
        //                    NonRecurringMonth stdfeemonth = new NonRecurringMonth();
        //                    stdfeemonth.StudentId = nonRecurringFeeMultiplier.StudentId;
        //                    stdfeemonth.Status = "Pending";
        //                    stdfeemonth.InstalmentAmount = nonRecurringFeeMultiplier.SharePerInstalment;
        //                    var dddd = DateTime.Now;
        //                    var d = dddd.ToString("yyyy-MM-dd");
        //                    stdfeemonth.IssueDate = d;
        //                    stdfeemonth.Months = item;
        //                    db.NonRecurringMonths.Add(stdfeemonth);
        //                    db.SaveChanges();

        //                }
        //                double? remaining = 0;
        //                var list = db.NonRecurringMonths.Where(x => x.StudentId == nonRecurringFeeMultiplier.StudentId && x.Status == "Pending").Select(x => x.InstalmentAmount).ToList();
        //                foreach (var item in list)
        //                {
        //                    remaining += item;
        //                }
        //                nonRecurringFeeMultiplier.RemainingAmount = remaining;
        //                nonRecurringFeeMultiplier.PaidAmount = nonRecurringFeeMultiplier.TutionFee - remaining;
        //                nonRecurringFeeMultiplier.SharePerInstalment = nonRecurringFeeMultiplier.RemainingAmount / nonRecurringFeeMultiplier.Instalments;
        //                nonRecurringFeeMultiplier.RemainingInstalments = Convert.ToDouble(nonRecurringFeeMultiplier.RemainingAmount / nonRecurringFeeMultiplier.SharePerInstalment);
        //                db.Entry(nonRecurringFeeMultiplier).State = EntityState.Modified;
        //                db.SaveChanges();
        //                return RedirectToAction("Index");
        //            }

        //        }

        //    }
        //    ViewBag.DescriptionId = new SelectList(db.NonRecurringCharges, "Id", "ExpenseType", nonRecurringFeeMultiplier.DescriptionId);
        //    return View(nonRecurringFeeMultiplier);
        //}

        // GET: NonRecurringFeeMultipliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NonRecurringFeeMultiplier nonRecurringFeeMultiplier = db.NonRecurringFeeMultipliers.Find(id);
            if (nonRecurringFeeMultiplier == null)
            {
                return HttpNotFound();
            }
            return View(nonRecurringFeeMultiplier);
        }

        // POST: NonRecurringFeeMultipliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NonRecurringFeeMultiplier nonRecurringFeeMultiplier = db.NonRecurringFeeMultipliers.Find(id);
            db.NonRecurringFeeMultipliers.Remove(nonRecurringFeeMultiplier);
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
