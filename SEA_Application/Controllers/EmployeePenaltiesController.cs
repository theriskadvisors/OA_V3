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
    public class EmployeePenaltiesController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: EmployeePenalties
        //public ActionResult Index()
        //{
        //    var employeePenalties = db.EmployeePenalties.Include(e => e.AspNetEmployee).Include(e => e.EmployeePenaltyType);
        //    return View(employeePenalties.ToList());
        //}
        public ActionResult EmployeePenaltyIndex()
        {
            return View();
        }
        public ActionResult GetEmployeePenalty()
        {
            var list = db.EmployeePenalties.ToList();
            List<Employee_Penalty> penalty = new List<Employee_Penalty>();
            foreach (var item in list)
            {
                Employee_Penalty p = new Employee_Penalty();
                p.Id = item.Id;
                p.Month = item.Month;
                p.Status = item.Status;
                p.Name = item.AspNetEmployee.Name;
                p.Type = item.EmployeePenaltyType.PenaltyType;
                p.Amount = item.EmployeePenaltyType.Amount;
                penalty.Add(p);
            }
            return Json(penalty,JsonRequestBehavior.AllowGet);
        }
        public class Employee_Penalty
        {
            public int Id { get; set; }
            public string Month { get; set; }
            public string Status { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public double? Amount { get; set; }
        }
        // GET: EmployeePenalties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePenalty employeePenalty = db.EmployeePenalties.Find(id);
            if (employeePenalty == null)
            {
                return HttpNotFound();
            }
            return View(employeePenalty);
        }

        // GET: EmployeePenalties/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees.Where(x => x.AspNetUser.Status != "False"), "Id", "Name");

            ViewBag.PenaltyId = new SelectList(db.EmployeePenaltyTypes, "Id", "PenaltyType");
            return View();
        }

        // POST: EmployeePenalties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeId,PenaltyId,Month,Status,TimeStamp")] EmployeePenalty employeePenalty)
        {
            if (ModelState.IsValid)
            {
                var datetime = DateTime.Now;
                employeePenalty.TimeStamp = datetime;
                var month = Request.Form["Month"];
                employeePenalty.Month = month;
                employeePenalty.Status = "Created";
                db.EmployeePenalties.Add(employeePenalty);
                db.SaveChanges();
                return RedirectToAction("EmployeePenaltyIndex");
            }

            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees.Where(x => x.AspNetUser.Status != "False"), "Id", "Name", employeePenalty.EmployeeId);
            ViewBag.PenaltyId = new SelectList(db.EmployeePenaltyTypes, "Id", "PenaltyType", employeePenalty.PenaltyId);
            return View(employeePenalty);
        }

        // GET: EmployeePenalties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePenalty employeePenalty = db.EmployeePenalties.Find(id);
            if (employeePenalty == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees.Where(x => x.AspNetUser.Status != "False"), "Id", "Name", employeePenalty.EmployeeId);
            ViewBag.PenaltyId = new SelectList(db.EmployeePenaltyTypes, "Id", "PenaltyType", employeePenalty.PenaltyId);
            return View(employeePenalty);
        }

        // POST: EmployeePenalties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId,PenaltyId,Month,Status,TimeStamp")] EmployeePenalty employeePenalty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeePenalty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EmployeePenaltyIndex");
            }
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees.Where(x => x.AspNetUser.Status != "False"), "Id", "Name", employeePenalty.EmployeeId);
            ViewBag.PenaltyId = new SelectList(db.EmployeePenaltyTypes, "Id", "PenaltyType", employeePenalty.PenaltyId);
            return View(employeePenalty);
        }

        // GET: EmployeePenalties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePenalty employeePenalty = db.EmployeePenalties.Find(id);
            if (employeePenalty == null)
            {
                return HttpNotFound();
            }
            return View(employeePenalty);
        }

        // POST: EmployeePenalties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeePenalty employeePenalty = db.EmployeePenalties.Find(id);
            db.EmployeePenalties.Remove(employeePenalty);
            db.SaveChanges();
            return RedirectToAction("EmployeePenaltyIndex");
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
