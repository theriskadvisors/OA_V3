using SEA_Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SEA_Application.Controllers
{
    public class EmployeeSalaryRecordController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        //
        // GET: /EmployeeSalaryRecord/
        public ActionResult Index()
        {
   
            return View(db.Employee_SalaryRecord.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees.Where(x => x.AspNetUser.Status != "False"), "Id", "Name");
            return View();
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee_SalaryRecord employee_salary = db.Employee_SalaryRecord.Where(x => x.EmployeeId == id).FirstOrDefault();
            if (employee_salary == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.AspNetEmployees.Where(x => x.AspNetUser.Status != "False"), "Id", "Name");

            return View(employee_salary);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,CurrentSalary,TimeStamp")] Employee_SalaryRecord employee_salary)
        {
            if (ModelState.IsValid)
            {
               Employee_SalaryRecord record =  db.Employee_SalaryRecord.Where(x => x.EmployeeId == employee_salary.EmployeeId).FirstOrDefault();
               record.StartingSalary = employee_salary.CurrentSalary;
               record.TimeStamp = DateTime.Now;
               record.CurrentSalary = employee_salary.CurrentSalary;
               db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public ActionResult AddRecord(Employee_SalaryRecord Record)
        {
            string Status = "error";
            try
            {
                if (db.Employee_SalaryRecord.Where(x => x.EmployeeId == Record.EmployeeId).Count() > 0)
                {
                    Status = "AlreadyExists";
                }
                else
                {
                    Record.CurrentSalary = Record.StartingSalary;
                    Record.TimeStamp = DateTime.Now;
                    db.Employee_SalaryRecord.Add(Record);
                    if (db.SaveChanges() > 0)
                    {
                        Status = "success";
                    }
                }
            }
              catch (Exception)
            {
                Status = "error";
            }
         
            return Content(Status);
        }
	}
}