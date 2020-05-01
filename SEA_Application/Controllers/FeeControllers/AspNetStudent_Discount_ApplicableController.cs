//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using SEA_Application.Models;

//namespace SEA_Application.Controllers.FeeControllers
//{
//    public class AspNetStudent_Discount_ApplicableController : Controller
//    {
//        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

//        // GET: AspNetStudent_Discount_Applicable
//        public ActionResult Index()
//        {
//            var aspNetStudent_Discount_Applicable = db.AspNetStudent_Discount_Applicable.Include(a => a.AspNetClass_FeeType).Include(a => a.AspNetUser);
//            return View(aspNetStudent_Discount_Applicable.ToList());
//        }

//        // GET: AspNetStudent_Discount_Applicable/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetStudent_Discount_Applicable aspNetStudent_Discount_Applicable = db.AspNetStudent_Discount_Applicable.Find(id);
//            if (aspNetStudent_Discount_Applicable == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetStudent_Discount_Applicable);
//        }

//        // GET: AspNetStudent_Discount_Applicable/Create
//        public ActionResult Create()
//        {
//            ViewBag.ClassFeeTypeId = new SelectList(db.AspNetClass_FeeType, "Id", "Type");
//            ViewBag.StudentId = new SelectList(db.AspNetUsers, "Id", "Email");
//            return View();
//        }

//        // POST: AspNetStudent_Discount_Applicable/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,StudentId,ClassFeeTypeId")] AspNetStudent_Discount_Applicable aspNetStudent_Discount_Applicable)
//        {
//            if (ModelState.IsValid)
//            {
//                db.AspNetStudent_Discount_Applicable.Add(aspNetStudent_Discount_Applicable);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.ClassFeeTypeId = new SelectList(db.AspNetClass_FeeType, "Id", "Type", aspNetStudent_Discount_Applicable.ClassFeeTypeId);
//            ViewBag.StudentId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Discount_Applicable.StudentId);
//            return View(aspNetStudent_Discount_Applicable);
//        }

//        // GET: AspNetStudent_Discount_Applicable/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetStudent_Discount_Applicable aspNetStudent_Discount_Applicable = db.AspNetStudent_Discount_Applicable.Find(id);
//            if (aspNetStudent_Discount_Applicable == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.ClassFeeTypeId = new SelectList(db.AspNetClass_FeeType, "Id", "Type", aspNetStudent_Discount_Applicable.ClassFeeTypeId);
//            ViewBag.StudentId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Discount_Applicable.StudentId);
//            return View(aspNetStudent_Discount_Applicable);
//        }

//        // POST: AspNetStudent_Discount_Applicable/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,StudentId,ClassFeeTypeId")] AspNetStudent_Discount_Applicable aspNetStudent_Discount_Applicable)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(aspNetStudent_Discount_Applicable).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.ClassFeeTypeId = new SelectList(db.AspNetClass_FeeType, "Id", "Type", aspNetStudent_Discount_Applicable.ClassFeeTypeId);
//            ViewBag.StudentId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Discount_Applicable.StudentId);
//            return View(aspNetStudent_Discount_Applicable);
//        }

//        // GET: AspNetStudent_Discount_Applicable/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetStudent_Discount_Applicable aspNetStudent_Discount_Applicable = db.AspNetStudent_Discount_Applicable.Find(id);
//            if (aspNetStudent_Discount_Applicable == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetStudent_Discount_Applicable);
//        }

//        // POST: AspNetStudent_Discount_Applicable/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            AspNetStudent_Discount_Applicable aspNetStudent_Discount_Applicable = db.AspNetStudent_Discount_Applicable.Find(id);
//            db.AspNetStudent_Discount_Applicable.Remove(aspNetStudent_Discount_Applicable);
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
