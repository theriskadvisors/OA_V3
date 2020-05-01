using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEA_Application.Models;
using OfficeOpenXml;

namespace SEA_Application.Controllers
{
    public class AspNetEmployeesController :  Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();
        int SessionID = Int32.Parse(SessionIDStaticController.GlobalSessionID);
        // GET: AspNetEmployees
        public ActionResult Index()
        {
            if (TempData["sMessage"] != null)
            {
                ViewBag.Message = TempData["sMessage"].ToString();
            }
            
          return View(db.AspNetEmployees.Where(x => x.Aspnet_Employee_Session.Select(y => y.Session_Id).Contains(SessionID)).ToList());
           
        }

        public ActionResult Indexs()
        {
            ViewBag.Error = "Empolyee Saved successfully";
            return View("Index", db.AspNetEmployees.ToList());
        }

        
        public ActionResult EmployeeDetail(int Id)
        {
            AspNetEmployee aspNetEmployee = db.AspNetEmployees.Where(x => x.Id== Id).Select(x => x).FirstOrDefault();
            if(aspNetEmployee.UserId != null)
            {
                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetEmployee.UserId);
            }
            
            if(aspNetEmployee.Status != null)
            {
                aspNetEmployee.Status = aspNetEmployee.Status.Trim();
            }

            ViewBag.VirtualRoleId = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetEmployee.VirtualRoleId);
            return View("Details", aspNetEmployee);
        }

        // GET: AspNetEmployees/Details/5
        public ActionResult Details(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetEmployee aspNetEmployee = db.AspNetEmployees.Where(x=> x.UserName == username).Select(x=> x).FirstOrDefault();
            if (aspNetEmployee == null)
            {
                return HttpNotFound();
            }
         
            return View(aspNetEmployee);
        }

        // GET: AspNetEmployees/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AspNetEmployees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "Id,PositionAppliedFor,DateAvailable,Name,UserName,BirthDate,Nationality,Religion,Gender,MailingAddress,Email,CellNo,Landline,SpouseName,SpouseHighestDegree,SpouseOccupation,SpouseBusinessAddress,UserId,GrossSalary,BasicSalary,MedicalAllowance,Accomodation,ProvidedFund,Tax,EOP,Salary,VirtualRoleId")] AspNetEmployee aspNetEmployee)
        {
            if (ModelState.IsValid)
            {
                var VirtualId = db.AspNetVirtualRoles.Where(x => x.Name == "Non Directive Staff").Select(x => x.Id).FirstOrDefault();
                aspNetEmployee.VirtualRoleId = VirtualId;
                db.AspNetEmployees.Add(aspNetEmployee);
                db.SaveChanges();

                Aspnet_Employee_Session ES = new Aspnet_Employee_Session();
               int sessionid =  db.AspNetSessions.Where(x => x.Status == "Active").FirstOrDefault().Id;
               ES.Session_Id = sessionid;
                ES.Emp_Id = aspNetEmployee.Id;
                db.Aspnet_Employee_Session.Add(ES);
                db.SaveChanges();
             
                return RedirectToAction("Indexs");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetEmployee.UserId);
            ViewBag.VirtualRoleId = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetEmployee.VirtualRoleId);
            return View(aspNetEmployee);
        }

        // GET: AspNetEmployees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetEmployee aspNetEmployee = db.AspNetEmployees.Find(id);
            if (aspNetEmployee == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetEmployee.UserId);
            ViewBag.VirtualRoleId = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetEmployee.VirtualRoleId);
            return View(aspNetEmployee);
        }

        // POST: AspNetEmployees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PositionAppliedFor,DateAvailable,Name,UserName,BirthDate,Nationality,Religion,Gender,MailingAddress,Email,CellNo,Landline,SpouseName,SpouseHighestDegree,SpouseOccupation,SpouseBusinessAddress,UserId,GrossSalary,BasicSalary,MedicalAllowance,Accomodation,ProvidedFund,Tax,EOP,Salary,VirtualRoleId")] AspNetEmployee aspNetEmployee)
        {
            if (ModelState.IsValid)
            {

                db.Entry(aspNetEmployee).State = EntityState.Modified;
                var VirtualId = db.AspNetVirtualRoles.Where(x => x.Name == "Non Directive Staff").Select(x => x.Id).FirstOrDefault();
                aspNetEmployee.VirtualRoleId = VirtualId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetEmployee.UserId);
            ViewBag.VirtualRoleId = new SelectList(db.AspNetVirtualRoles, "Id", "Name", aspNetEmployee.VirtualRoleId);
            return View(aspNetEmployee);
        }

        // GET: AspNetEmployees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetEmployee aspNetEmployee = db.AspNetEmployees.Find(id);
            if (aspNetEmployee == null)
            {
                return HttpNotFound();
            }
            return View(aspNetEmployee);
        }

        // POST: AspNetEmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                AspNetEmployee aspNetEmployee = db.AspNetEmployees.Find(id);
                db.AspNetEmployees.Remove(aspNetEmployee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.data = "This Employee has salary data in it.It can't be deleted";
                return View("Delete", db.AspNetEmployees.Find(id));
            }
           
        }
        public ActionResult Enable(int id)
        {
            AspNetEmployee emp = db.AspNetEmployees.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            emp.Status = "True";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Disable(int id)
        {
           
                AspNetEmployee emp = db.AspNetEmployees.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
                emp.Status = "False";
                db.SaveChanges();
                return RedirectToAction("Index");
           
        }
        public ViewResult DisabledEmployee()
        {

            return View(db.AspNetEmployees.Where(x => x.Status == "False").Select(x => x).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult NonManagerialEmployees()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult NonManagerialEmployees(RegisterViewModel model)
        {
            if (1 == 1)

            {
                AspNetEmployee emp = new AspNetEmployee();
                emp.Name = model.Name;
                emp.Email = model.Email;
                emp.PositionAppliedFor = Request.Form["appliedFor"];
                emp.DateAvailable = Request.Form["dateAvailable"];
                emp.JoiningDate = Request.Form["JoiningDate"];
                emp.BirthDate = Request.Form["birthDate"];
                emp.Nationality = Request.Form["nationality"];
                emp.Religion = Request.Form["religion"];
                emp.Gender = Request.Form["gender"];
                emp.MailingAddress = Request.Form["mailingAddress"];
                emp.CellNo = Request.Form["cellNo"];
                emp.Landline = Request.Form["landLine"];
                emp.SpouseName = Request.Form["spouseName"];
                emp.SpouseHighestDegree = Request.Form["spouseHighestDegree"];
                emp.SpouseOccupation = Request.Form["spouseOccupation"];
                emp.SpouseBusinessAddress = Request.Form["spouseBusinessAddress"];

                //emp.GrossSalary = Convert.ToInt32(Request.Form["GrossSalary"]);
                //emp.BasicSalary = Convert.ToInt32(Request.Form["BasicSalary"]);
                //emp.MedicalAllowance = Convert.ToInt32(Request.Form["MedicalAllowance"]);
                //emp.Accomodation = Convert.ToInt32(Request.Form["Accomodation"]);
                //emp.ProvidedFund = Convert.ToInt32(Request.Form["ProvidedFund"]);
                //emp.Tax = Convert.ToInt32(Request.Form["Tax"]);
                //emp.EOP = Convert.ToInt32(Request.Form["EOP"]);

                emp.VirtualRoleId = db.AspNetVirtualRoles.Where(x => x.Name == "Non Directive Staff").Select(x => x.Id).FirstOrDefault();

                try
                {
                    db.AspNetEmployees.Add(emp);

                    if (db.SaveChanges() > 0)
                    {

                        Aspnet_Employee_Session ES = new Aspnet_Employee_Session();
                        ES.Session_Id = SessionID;
                        ES.Emp_Id = emp.Id;
                        db.Aspnet_Employee_Session.Add(ES);
                        db.SaveChanges();
                    }
                    TempData["sMessage"] = "Employee successfully saved.";
                    return RedirectToAction("Index", "AspNetEmployees");
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("", e.InnerException);
                    return View(model);
                }
                    
       
            }
            return View(model);
        }


        public ActionResult Excel_Data(HttpPostedFileBase excelfile)
        {
            var dbTransaction = db.Database.BeginTransaction();
            try
            {

                if (excelfile == null || excelfile.ContentLength == 0)
                {
                    TempData["Error"] = "Please select an excel file";
                    return RedirectToAction("Create", "AspNetEmployees");
                }
                else if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {

                    HttpPostedFileBase file = excelfile;   // Request.Files["excelfile"];

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        ApplicationDbContext context = new ApplicationDbContext();
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            AspNetEmployee Employee = new AspNetEmployee();
                            Employee.Email = workSheet.Cells[rowIterator, 1].Value.ToString();
                            Employee.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                            Employee.UserName = workSheet.Cells[rowIterator, 3].Value.ToString();
                            Employee.PositionAppliedFor = workSheet.Cells[rowIterator, 4].Value.ToString();
                            Employee.DateAvailable = workSheet.Cells[rowIterator, 5].Value.ToString();
                            Employee.JoiningDate = workSheet.Cells[rowIterator, 6].Value.ToString();
                            Employee.BirthDate = workSheet.Cells[rowIterator, 7].Value.ToString();
                            Employee.Nationality = workSheet.Cells[rowIterator, 8].Value.ToString();
                            Employee.Religion = workSheet.Cells[rowIterator, 9].Value.ToString();
                            Employee.Gender = workSheet.Cells[rowIterator, 10].Value.ToString();
                            Employee.MailingAddress = workSheet.Cells[rowIterator, 11].Value.ToString();
                            Employee.CellNo = workSheet.Cells[rowIterator, 12].Value.ToString();
                            Employee.Landline = workSheet.Cells[rowIterator, 13].Value.ToString();
                            Employee.SpouseName = workSheet.Cells[rowIterator, 14].Value.ToString();
                            Employee.SpouseHighestDegree = workSheet.Cells[rowIterator, 15].Value.ToString();
                            Employee.SpouseOccupation = workSheet.Cells[rowIterator, 16].Value.ToString();
                            Employee.SpouseBusinessAddress = workSheet.Cells[rowIterator, 17].Value.ToString();
                            Employee.Illness = workSheet.Cells[rowIterator, 18].Value.ToString();
                            Employee.GrossSalary = Convert.ToInt32(workSheet.Cells[rowIterator, 19].Value.ToString());
                            Employee.BasicSalary = Convert.ToInt32(workSheet.Cells[rowIterator, 20].Value.ToString());
                            Employee.MedicalAllowance = Convert.ToInt32(workSheet.Cells[rowIterator, 21].Value.ToString());
                            Employee.Accomodation = Convert.ToInt32(workSheet.Cells[rowIterator, 22].Value.ToString());
                            Employee.ProvidedFund = Convert.ToInt32(workSheet.Cells[rowIterator, 23].Value.ToString());
                            Employee.Tax = Convert.ToInt32(workSheet.Cells[rowIterator, 24].Value.ToString());
                            Employee.EOP = Convert.ToInt32(workSheet.Cells[rowIterator, 25].Value.ToString());
                            Employee.UserId = null;
                            Employee.VirtualRoleId = db.AspNetVirtualRoles.Where(x => x.Name == "Non Directive Staff").Select(x => x.Id).FirstOrDefault();
                            db.AspNetEmployees.Add(Employee);
                            db.SaveChanges();
                        }
                        
                    }
                    dbTransaction.Commit();
                    return RedirectToAction("Index", "AspNetEmployees");
                }
                else
                {
                    TempData["Error"] = "File type is incorrect";
                    return RedirectToAction("Create", "AspNetEmployees");
                }

            }
            catch
            {
                dbTransaction.Dispose();
                TempData["Error"] = "Incorrect data in file";
                return RedirectToAction("Create", "AspNetEmployees");
            }
        }

    }
}
