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
    public class EmployeeAdvanceSalariesController : Controller
    {
        private SEA_DatabaseEntities db = new  SEA_DatabaseEntities();

        // GET: EmployeeAdvanceSalaries
        public ActionResult Index()
        {
            var employeeAdvanceSalaries = db.EmployeeAdvanceSalaries.Include(e => e.AspNetEmployee);
            return View(employeeAdvanceSalaries.ToList());
        }

        // GET: EmployeeAdvanceSalaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeAdvanceSalary employeeAdvanceSalary = db.EmployeeAdvanceSalaries.Find(id);
            if (employeeAdvanceSalary == null)
            {
                return HttpNotFound();
            }
            return View(employeeAdvanceSalary);
        }

        // GET: EmployeeAdvanceSalaries/Create
        public ActionResult Create()
        {
            //ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "Year");
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees.Where(x=>x.AspNetUser.Status!="False"), "Id", "Name");
            return View();
        }

        // POST: EmployeeAdvanceSalaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeId,AdvanceSalary,Months,Date,Status,SessionId")] EmployeeAdvanceSalary employeeAdvanceSalary)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var name = Request.Form["MonthName"];
                    employeeAdvanceSalary.Status = "Pending";
                    employeeAdvanceSalary.Months = name;
                    db.EmployeeAdvanceSalaries.Add(employeeAdvanceSalary);
                    db.SaveChanges();

                    //Ledger ledger_exp = db.Ledgers.Where(x => x.Name == "Employee Clearing").FirstOrDefault();////Expence
                    //ledger_exp.StartingBalance += employeeAdvanceSalary.AdvanceSalary;
                    //ledger_exp.CurrentBalance += employeeAdvanceSalary.AdvanceSalary;
                    //db.SaveChanges();

                    //Ledger ledger_lib = db.Ledgers.Where(x => x.Name == "Employee Salary").FirstOrDefault();////Liability
                    //ledger_lib.StartingBalance += employeeAdvanceSalary.AdvanceSalary;
                    //ledger_lib.CurrentBalance += employeeAdvanceSalary.AdvanceSalary;
                    //db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
          catch(Exception e)
            {
                ViewBag.ErrorMessage= e.Message;
            }

            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "Year", employeeAdvanceSalary.SessionId);
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees, "Id", "Name", employeeAdvanceSalary.EmployeeId);
            return View(employeeAdvanceSalary);
        }
        public ActionResult GetAdvanceSalary(int empid)
        {
            var advance = db.EmployeeAdvanceSalaries.Where(x => x.EmployeeId == empid && x.Status == "Pending").Select(x => x.AdvanceSalary).FirstOrDefault();
            if(advance==null)
            {
                advance = 0;
            }
            return Json(advance,JsonRequestBehavior.AllowGet);
        }
        // GET: EmployeeAdvanceSalaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeAdvanceSalary employeeAdvanceSalary = db.EmployeeAdvanceSalaries.Find(id);
            if (employeeAdvanceSalary == null)
            {
                return HttpNotFound();
            }
            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "Year", employeeAdvanceSalary.SessionId);
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees, "Id", "Name", employeeAdvanceSalary.EmployeeId);
            return View(employeeAdvanceSalary);
        }

        // POST: EmployeeAdvanceSalaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId,AdvanceSalary,Months,Date,Status,SessionId")] EmployeeAdvanceSalary employeeAdvanceSalary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeAdvanceSalary).State = EntityState.Modified;
                db.SaveChanges();

                //if(employeeAdvanceSalary.Status!="Pending")
                //{
                //    Ledger l = db.Ledgers.Where(x => x.Name == "Employee Salary").FirstOrDefault();////Liability
                //    l.StartingBalance -= employeeAdvanceSalary.AdvanceSalary;
                //    l.CurrentBalance -= employeeAdvanceSalary.AdvanceSalary;
                //    db.SaveChanges();
                //}


                return RedirectToAction("Index");
            }
            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "Year", employeeAdvanceSalary.SessionId);
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees, "Id", "Name", employeeAdvanceSalary.EmployeeId);
            return View(employeeAdvanceSalary);
        }

        // GET: EmployeeAdvanceSalaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeAdvanceSalary employeeAdvanceSalary = db.EmployeeAdvanceSalaries.Find(id);
            if (employeeAdvanceSalary == null)
            {
                return HttpNotFound();
            }
            return View(employeeAdvanceSalary);
        }

        // POST: EmployeeAdvanceSalaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeAdvanceSalary employeeAdvanceSalary = db.EmployeeAdvanceSalaries.Find(id);
            db.EmployeeAdvanceSalaries.Remove(employeeAdvanceSalary);
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
