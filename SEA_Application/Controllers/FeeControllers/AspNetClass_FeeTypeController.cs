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
    public class AspNetClass_FeeTypeController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetClass_FeeType
        public ActionResult Index()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }

        // GET: AspNetClass_FeeType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetClass_FeeType aspNetClass_FeeType = db.AspNetClass_FeeType.Find(id);
            if (aspNetClass_FeeType == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName", aspNetClass_FeeType.ClassID);
            ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Name", aspNetClass_FeeType.LedgerID);
            return View(aspNetClass_FeeType);
        }

        // GET: AspNetClass_FeeType/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Name");
            return View();
        }

        // POST: AspNetClass_FeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClassID,LedgerID,Amount,Type")] AspNetClass_FeeType aspNetClass_FeeType)
        {
            if (ModelState.IsValid)
            {
                db.AspNetClass_FeeType.Add(aspNetClass_FeeType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName", aspNetClass_FeeType.ClassID);
            ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Name", aspNetClass_FeeType.LedgerID);
            return View(aspNetClass_FeeType);
        }

        // GET: AspNetClass_FeeType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetClass_FeeType aspNetClass_FeeType = db.AspNetClass_FeeType.Find(id);
            if (aspNetClass_FeeType == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName", aspNetClass_FeeType.ClassID);
            ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Name", aspNetClass_FeeType.LedgerID);
            return View(aspNetClass_FeeType);
        }

        // POST: AspNetClass_FeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClassID,LedgerID,Amount,Type")] AspNetClass_FeeType aspNetClass_FeeType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetClass_FeeType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName", aspNetClass_FeeType.ClassID);
            ViewBag.LedgerID = new SelectList(db.AspNetFinanceLedgers, "Id", "Name", aspNetClass_FeeType.LedgerID);
            return View(aspNetClass_FeeType);
        }

        // GET: AspNetClass_FeeType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetClass_FeeType aspNetClass_FeeType = db.AspNetClass_FeeType.Find(id);
            if (aspNetClass_FeeType == null)
            {
                return HttpNotFound();
            }
            return View(aspNetClass_FeeType);
        }

        // POST: AspNetClass_FeeType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetClass_FeeType aspNetClass_FeeType = db.AspNetClass_FeeType.Find(id);
            db.AspNetClass_FeeType.Remove(aspNetClass_FeeType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteCnfm(int id)
        {
            AspNetClass_FeeType aspNetClass_FeeType = db.AspNetClass_FeeType.Find(id);
            try
            {
                db.AspNetClass_FeeType.Remove(aspNetClass_FeeType);
                db.SaveChanges();
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                ViewBag.Error = "It can't be deleted";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
          
        }


        [HttpGet]
        public JsonResult GetClassFeeType(int id)
        {
            return Json(db.AspNetClass_FeeType.Where(x => x.ClassID == id).Select(x => new {FeeId =x.Id , x.AspNetClass.Id,x.AspNetClass.ClassName, x.Amount, x.Type,LedgerID=x.AspNetFinanceLedger.Id,LedgerName=x.AspNetFinanceLedger.Name}).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFeeType()
        {
            return Json(db.AspNetClass_FeeType.Select(x => new { x.AspNetClass.Id, x.AspNetClass.ClassName, x.Amount, x.Type, LedgerID = x.AspNetFinanceLedger.Id, LedgerName = x.AspNetFinanceLedger.Name }).ToList(), JsonRequestBehavior.AllowGet);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
          }

        public class class_feetype
        {
            public int Id { get; set; }
            public int ClassId { get; set; }
            public int LedgerID { get; set; }
            public int Amount { get; set; }
            public string Type { get; set; }
        }
        [HttpPost]
        public JsonResult AddClass_FeeType(List<class_feetype> class_feeType)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            try
            {
                foreach (var item in class_feeType)
                {
                    if (item.Id == -1)
                    {
                        AspNetClass_FeeType class_fee = new AspNetClass_FeeType();
                        class_fee.ClassID = item.ClassId;
                        class_fee.LedgerID = item.LedgerID;
                        class_fee.Amount = item.Amount;
                        class_fee.Type = item.Type;
                        db.AspNetClass_FeeType.Add(class_fee);
                        db.SaveChanges();
                    }
                    else
                    {
                        AspNetClass_FeeType class_fee = db.AspNetClass_FeeType.Where(x => x.Id == item.Id).FirstOrDefault();
                        class_fee.ClassID = item.ClassId;
                        class_fee.LedgerID = item.LedgerID;
                        class_fee.Amount = item.Amount;
                        class_fee.Type = item.Type;
                        db.SaveChanges();
                    }

                }
                dbContextTransaction.Commit();
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                dbContextTransaction.Dispose();
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}
