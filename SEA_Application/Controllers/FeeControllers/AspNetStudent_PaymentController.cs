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
    public class AspNetStudent_PaymentController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspNetStudent_Payment

        public ActionResult Payment_Detail(int id)
        {
            var aspNetStudent_PaymentDetail = db.AspNetStudent_PaymentDetail.Include(a => a.AspNetStudent_Payment).Where(x => x.Student_PaymentID == id);
            return View(aspNetStudent_PaymentDetail.ToList());
        }
        public ActionResult Index()
        {
            var aspNetStudent_Payment = db.AspNetStudent_Payment.Include(a => a.AspNetFeeChallan).Include(a => a.AspNetUser);
            return View(aspNetStudent_Payment.ToList());
        }

        // GET: AspNetStudent_Payment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_Payment aspNetStudent_Payment = db.AspNetStudent_Payment.Find(id);
            if (aspNetStudent_Payment == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudent_Payment);
        }

        // GET: AspNetStudent_Payment/Create
        public ActionResult Create()
        {
            ViewBag.FeeChallanID = new SelectList(db.AspNetFeeChallans, "Id", "Id");
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AspNetStudent_Payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentID,FeeChallanID,PaymentAmount,PaymentDate")] AspNetStudent_Payment aspNetStudent_Payment)
        {
            var TransactionObj = db.Database.BeginTransaction();
            try
            {
                if (ModelState.IsValid)
                {
                    db.AspNetStudent_Payment.Add(aspNetStudent_Payment);
                    db.SaveChanges();
                }
                TransactionObj.Commit();
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                var UserNameLog = User.Identity.Name;
                AspNetUser a = db.AspNetUsers.First(x => x.UserName == UserNameLog);
                string UserIDLog = a.Id;
                var classObjLog = Request.Form["ClassID"];
                var logMessage = "Student Payment Added : " + aspNetStudent_Payment.StudentID + ", FeeChallan ID: " + aspNetStudent_Payment.FeeChallanID + ", Payment Amount: " + aspNetStudent_Payment.PaymentAmount + ", Payment Date: " + aspNetStudent_Payment.PaymentDate;

                var LogControllerObj = new AspNetLogsController();
                LogControllerObj.CreateLogSave(logMessage, UserIDLog);
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                return RedirectToAction("Index");
            }


            catch (Exception)
            {
                TransactionObj.Dispose();
            }

            ViewBag.FeeChallanID = new SelectList(db.AspNetFeeChallans, "Id", "Id", aspNetStudent_Payment.FeeChallanID);
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Payment.StudentID);
            return View(aspNetStudent_Payment);
        }

        // GET: AspNetStudent_Payment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_Payment aspNetStudent_Payment = db.AspNetStudent_Payment.Find(id);
            if (aspNetStudent_Payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.FeeChallanID = new SelectList(db.AspNetFeeChallans, "Id", "Id", aspNetStudent_Payment.FeeChallanID);
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Payment.StudentID);
            return View(aspNetStudent_Payment);
        }

        // POST: AspNetStudent_Payment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentID,FeeChallanID,PaymentAmount,PaymentDate")] AspNetStudent_Payment aspNetStudent_Payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetStudent_Payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FeeChallanID = new SelectList(db.AspNetFeeChallans, "Id", "Id", aspNetStudent_Payment.FeeChallanID);
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudent_Payment.StudentID);
            return View(aspNetStudent_Payment);
        }

        // GET: AspNetStudent_Payment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudent_Payment aspNetStudent_Payment = db.AspNetStudent_Payment.Find(id);
            if (aspNetStudent_Payment == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudent_Payment);
        }

        // POST: AspNetStudent_Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetStudent_Payment aspNetStudent_Payment = db.AspNetStudent_Payment.Find(id);
            db.AspNetStudent_Payment.Remove(aspNetStudent_Payment);
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
