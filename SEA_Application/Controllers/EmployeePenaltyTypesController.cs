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
    public class EmployeePenaltyTypesController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: EmployeePenaltyTypes
        //public ActionResult Index()
        //{
        //    return View(db.EmployeePenaltyTypes.ToList());
        //}
        public ActionResult PenaltyTypeIndex()
        {
            return View();

        }
        public ActionResult GetPenaltyType()
        {
            var list = db.EmployeePenaltyTypes.ToList();
            List<EmployeePenaltyType> penalty = new List<EmployeePenaltyType>();
            foreach (var item in list)
            {
                EmployeePenaltyType p = new EmployeePenaltyType();
                p.Id = item.Id;
                p.PenaltyType = item.PenaltyType;
                p.Amount = item.Amount;
                penalty.Add(p);
            }
            return Json(penalty,JsonRequestBehavior.AllowGet);
        }
        // GET: EmployeePenaltyTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePenaltyType employeePenaltyType = db.EmployeePenaltyTypes.Find(id);
            if (employeePenaltyType == null)
            {
                return HttpNotFound();
            }
            return View(employeePenaltyType);
        }

        // GET: EmployeePenaltyTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeePenaltyTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PenaltyType,Amount,TimeStamp")] EmployeePenaltyType employeePenaltyType)
        {
            if (ModelState.IsValid)
            {
                var datetime = DateTime.Now;
                employeePenaltyType.TimeStamp = datetime;
                db.EmployeePenaltyTypes.Add(employeePenaltyType);
                db.SaveChanges();
                return RedirectToAction("PenaltyTypeIndex");
            }

            return View(employeePenaltyType);
        }

        // GET: EmployeePenaltyTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePenaltyType employeePenaltyType = db.EmployeePenaltyTypes.Find(id);
            if (employeePenaltyType == null)
            {
                return HttpNotFound();
            }
            return View(employeePenaltyType);
        }

        // POST: EmployeePenaltyTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PenaltyType,Amount,TimeStamp")] EmployeePenaltyType employeePenaltyType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeePenaltyType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PenaltyTypeIndex");
            }
            return View(employeePenaltyType);
        }

        // GET: EmployeePenaltyTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePenaltyType employeePenaltyType = db.EmployeePenaltyTypes.Find(id);
            if (employeePenaltyType == null)
            {
                return HttpNotFound();
            }
            return View(employeePenaltyType);
        }

        // POST: EmployeePenaltyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeePenaltyType employeePenaltyType = db.EmployeePenaltyTypes.Find(id);
            db.EmployeePenaltyTypes.Remove(employeePenaltyType);
            db.SaveChanges();
            return RedirectToAction("PenaltyTypeIndex");
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
