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
//    public class AspNetDiscountTypeController : Controller
//    {
//        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

//        // GET: AspNetDiscountType
//        public ActionResult Index()
//        {
//            return View(db.AspNetDiscountTypes.ToList());
//        }

//        // GET: AspNetDiscountType/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetDiscountType aspNetDiscountType = db.AspNetDiscountTypes.Find(id);
//            if (aspNetDiscountType == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetDiscountType);
//        }

//        // GET: AspNetDiscountType/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: AspNetDiscountType/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,TypeName")] AspNetDiscountType aspNetDiscountType)
//        {
//            var TransactionObj = db.Database.BeginTransaction();
//            try { 
//            if (ModelState.IsValid)
//            {
//                db.AspNetDiscountTypes.Add(aspNetDiscountType);
//                db.SaveChanges();
//            }
//            TransactionObj.Commit();
//            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//            var UserNameLog = User.Identity.Name;
//            AspNetUser a = db.AspNetUsers.First(x => x.UserName == UserNameLog);
//            string UserIDLog = a.Id;
//            var classObjLog = Request.Form["ClassID"];
//                var logMessage = "Student Discount Type Added : " + aspNetDiscountType.TypeName;
//                var LogControllerObj = new AspNetLogsController();
//            LogControllerObj.CreateLogSave(logMessage, UserIDLog);
//            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//            return RedirectToAction("Index");
//        }
            
            
//            catch(Exception)
//            {
//                TransactionObj.Dispose();
//            }

//            return View(aspNetDiscountType);
//        }

//        // GET: AspNetDiscountType/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetDiscountType aspNetDiscountType = db.AspNetDiscountTypes.Find(id);
//            if (aspNetDiscountType == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetDiscountType);
//        }

//        // POST: AspNetDiscountType/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,TypeName")] AspNetDiscountType aspNetDiscountType)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(aspNetDiscountType).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(aspNetDiscountType);
//        }

//        // GET: AspNetDiscountType/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetDiscountType aspNetDiscountType = db.AspNetDiscountTypes.Find(id);
//            if (aspNetDiscountType == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetDiscountType);
//        }

//        // POST: AspNetDiscountType/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            AspNetDiscountType aspNetDiscountType = db.AspNetDiscountTypes.Find(id);
//            db.AspNetDiscountTypes.Remove(aspNetDiscountType);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }
//        public ActionResult DeleteCnfm(int id)
//        {
//            AspNetDiscountType aspNetDiscountType = db.AspNetDiscountTypes.Find(id);
//            try
//            {
//                db.AspNetDiscountTypes.Remove(aspNetDiscountType);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                ViewBag.Error = "It can't be deleted";
//                return View("Details", aspNetDiscountType);
//            }
           
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
