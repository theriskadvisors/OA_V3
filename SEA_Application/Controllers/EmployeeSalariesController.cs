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
    public class EmployeeSalariesController : Controller
    {
        private SEA_DatabaseEntities db = new  SEA_DatabaseEntities();

        // GET: EmployeeSalaries
        public ActionResult Index()
        {
            var employeeSalaries = db.EmployeeSalaries.Include(e => e.AspNetEmployee);
            return View(employeeSalaries.ToList());
        }

        // GET: EmployeeSalaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSalary employeeSalary = db.EmployeeSalaries.Find(id);
            if (employeeSalary == null)
            {
                return HttpNotFound();
            }
            return View(employeeSalary);
        }

        // GET: EmployeeSalaries/Create
        public ActionResult Increment()
        {
            ViewBag.EmpSalaryRecordId = new SelectList(db.AspNetEmployees.Where(x => x.AspNetUser.Status != "False"), "Id", "Name");
            return View();
        }
        public ActionResult IncrementHistory()
        {
            
            return View(db.Employee_SalaryIncrement.ToList());
        }
        // GET: EmployeeSalaries/Create
        public ActionResult Create()
        {
            ViewBag.SessionId = new SelectList(db.AspNetSessions, "Id", "SessionName");
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees.Where(x=>x.AspNetUser.Status != "False" ), "Id", "Name");
            return View();
        }
        public ActionResult AddIncrementDetails(Employee_SalaryIncrement Inc)
        {
               string status = "error";
               if (Inc.AfterSalary > 0)
               {
                   Employee_SalaryRecord es = db.Employee_SalaryRecord.Where(x => x.EmployeeId == Inc.EmpSalaryRecordId).FirstOrDefault();
                   es.CurrentSalary = Inc.AfterSalary;
                   es.TimeStamp = DateTime.Now;
                   if (db.SaveChanges() > 0)
                   {
                       Employee_SalaryIncrement Inc_Rec = new Employee_SalaryIncrement();
                       Inc_Rec.IncrementAmount = Inc.IncrementAmount;
                       Inc_Rec.PreviousSalary = Inc.PreviousSalary;
                       Inc_Rec.EmpSalaryRecordId = es.Id;
                       Inc_Rec.AfterSalary = Inc.AfterSalary;
                       Inc_Rec.TimeStamp = DateTime.Now;
                       db.Employee_SalaryIncrement.Add(Inc_Rec);
                       if (db.SaveChanges() > 0)
                       {
                           status = "success";
                       }
                   }
               }
            return Content(status);
        }
        public ActionResult GetEmployeeSalary(int empid)
        {
            double? salary = 0;
            var Rec = db.Employee_SalaryRecord.Where(x => x.EmployeeId == empid).Select(x=>x.CurrentSalary).FirstOrDefault();
            if (Rec != null)
            {
               salary = Rec;
             
            }
            var advance = db.EmployeeAdvanceSalaries.Where(x => x.EmployeeId == empid && x.Status == "Pending").Select(x => x.AdvanceSalary).FirstOrDefault();
            if (advance == null)
            {
                advance = 0;
            }

            var fine = db.TeacherPenaltyRecords.Where(x => x.EmployeeId == empid && x.Status == "Created").Select(x=>x.Amount).FirstOrDefault();
            if (fine == null)
            {
                fine = 0;
            }
         
            var result = new { advance, salary ,fine};
            return Json(result, JsonRequestBehavior.AllowGet);
            
        }


        // POST: EmployeeSalaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeId,Salary,Months,Date,Status,SessionId")] EmployeeSalary employeeSalary)
        {
            var month =  Request.Form["MonthName"];
          int counter =  db.EmployeeSalaries.Where(x => x.EmployeeId == employeeSalary.EmployeeId && x.Months == month).Count();
          if (counter == 0)
          {
              if (ModelState.IsValid)
              {

                  string e_salary = Request.Form["salary"];
                  var name = Request.Form["MonthName"];
                  employeeSalary.Months = name;
                  employeeSalary.Status = "Pending";
                  db.EmployeeSalaries.Add(employeeSalary);
                  db.SaveChanges();

                  Ledger ledger_exp = db.Ledgers.Where(x => x.Name == "Employee Clearing").FirstOrDefault();////Expence
                  ledger_exp.CurrentBalance += Convert.ToDecimal(e_salary);
                  db.SaveChanges();

                  Ledger ledger_lib = db.Ledgers.Where(x => x.Name == "Employee Salary").FirstOrDefault();////Liability
                  ledger_lib.CurrentBalance += Convert.ToDecimal(e_salary);
                  db.SaveChanges();

                  return RedirectToAction("Index");
              }

          }

            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees, "Id", "Name", employeeSalary.EmployeeId);
            return View(employeeSalary);
        }

        // GET: EmployeeSalaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSalary employeeSalary = db.EmployeeSalaries.Find(id);
            if (employeeSalary == null)
            {
                return HttpNotFound();
            }
            ViewBag.Error = TempData["ErrorMessage"] as string;
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees.Where(x => x.AspNetUser.Status != "False"), "Id", "Name", employeeSalary.EmployeeId);
            return View(employeeSalary);
        }

        // POST: EmployeeSalaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId,Salary,Months,Date,Status,SessionId")] EmployeeSalary employeeSalary)
        {

            string AccountType = Request.Form["AccountType"];
            if (ModelState.IsValid)
            {
                var dbTransaction = db.Database.BeginTransaction();
                employeeSalary.Status = employeeSalary.Status;
                db.Entry(employeeSalary).State = EntityState.Modified;
                db.SaveChanges();
                if (employeeSalary.Status != "Pending")
                {
                    if (AccountType == "Cash")
                    {
                        try
                        {
                            Ledger ledger_lib = db.Ledgers.Where(x => x.Name == "Employee Salary").FirstOrDefault();////Liability                     
                            ledger_lib.CurrentBalance -= employeeSalary.Salary;
                            db.SaveChanges();
                        }
                        catch
                        {
                            TempData["ErrorMessage"] = "No Cash in Teacher Salary Account.";
                            return RedirectToAction("Edit", employeeSalary);


                        }
                        try
                        {
                            Ledger ledger_Asset = db.Ledgers.Where(x => x.Name == "Petty Cash").FirstOrDefault();
                            ledger_Asset.CurrentBalance -= employeeSalary.Salary;
                            db.SaveChanges();
                        }
                        catch
                        {
                            TempData["ErrorMessage"] = "No Cash in Petty Cash.";
                            return RedirectToAction("Edit", employeeSalary);


                        }


                    }
                    else
                    {
                        try
                        {
                            Ledger ledger_lib = db.Ledgers.Where(x => x.Name == "Employee Salary").FirstOrDefault();////Liability
                            ledger_lib.CurrentBalance -= employeeSalary.Salary;
                            db.SaveChanges();
                        }
                        catch
                        {
                            TempData["ErrorMessage"] = "No Cash in Teacher Salary Account.";
                            return RedirectToAction("Edit", employeeSalary);


                        }
                        try
                        {
                            Ledger ledger_Asset = db.Ledgers.Where(x => x.Name == "Meezan Bank" && x.LedgerGroup.Name == "Bank").FirstOrDefault();
                            ledger_Asset.CurrentBalance -= employeeSalary.Salary;
                            db.SaveChanges();
                        }
                        catch
                        {
                            TempData["ErrorMessage"] = "No Cash in Meezan Bank Account.";
                            return RedirectToAction("Edit", employeeSalary);


                        }


                    }
                   
                }
                dbTransaction.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees, "Id", "Name", employeeSalary.EmployeeId);
            return View(employeeSalary);
        }
        // GET: EmployeeSalaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSalary employeeSalary = db.EmployeeSalaries.Find(id);
            if (employeeSalary == null)
            {
                return HttpNotFound();
            }
            return View(employeeSalary);
        }

        // POST: EmployeeSalaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeSalary employeeSalary = db.EmployeeSalaries.Find(id);
            db.EmployeeSalaries.Remove(employeeSalary);
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
