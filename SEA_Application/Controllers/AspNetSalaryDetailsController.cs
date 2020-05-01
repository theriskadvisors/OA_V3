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
    public class AspNetSalaryDetailController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        public JsonResult SalaryHold(int id)
        {
            string salary = "";
            try
            {
                var list = db.AspNetSalaryDetails.Where(x => x.EmployeeId == id && x.Status == "Hold").Select(x => new {x.Id, x.SalaryHold  , x.AspNetSalary.Month} ).ToList();
                if(list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        salary += "<option value = " + item.SalaryHold + "_"+ item.Id + ">" + item.SalaryHold + "-" + item.Month + "</option>";
                    }
                }else
                {
                    salary = "<option value=''>No hold Salary</option>";
                }
                
            }
            catch
            {
                salary = "<option value=''>No hold Salary</option>";
            }
            return Json(salary, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UsersByVirtualRole(int id , string Month)
        {
            if(id != 0)
            {
                var users = db.AspNetEmployees.Where(x => x.VirtualRoleId == id && x.Status != "False").Select(x => new
                {
                    x.Name,
                    x.UserName,
                    x.Email,
                    x.Id,
                    x.PositionAppliedFor,
                    x.Tax,
                    x.MedicalAllowance,
                    x.EOP,
                    x.BasicSalary,
                    x.Accomodation,
                    x.GrossSalary,
                    Fine = db.AspNetEmployee_Attendance.Where(y => y.EmployeeID == x.Id && y.Status == "Absent" && y.Month == Month).Count() * (x.GrossSalary / 30)
                }).ToList();

                return Json(users, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var users = db.AspNetEmployees.Where(x => x.Status != "False").Select(x => new
                {
                    x.Name,
                    x.UserName,
                    x.Email,
                    x.Id,
                    x.PositionAppliedFor,
                    x.Salary,
                    x.Tax,
                    x.MedicalAllowance,
                    x.EOP,
                    x.BasicSalary,
                    x.Accomodation,
                    x.GrossSalary,
                    Fine = db.AspNetEmployee_Attendance.Where(y => y.EmployeeID == x.Id && y.Status == "Absent" && y.Month == Month).Count() * (x.GrossSalary / 30)
                }).ToList();

                return Json(users, JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult AlreadyPresentSalary(string UserId)
        {
            var uid = Convert.ToInt32(UserId);
            var emp = db.AspNetEmployees.Where(x => x.Id == uid).Select(x => new
            {
                x.Id,
                x.GrossSalary,
                x.BasicSalary,
                x.MedicalAllowance,
                x.Accomodation,
                x.ProvidedFund,
                x.Tax,
                x.EOP
            }).FirstOrDefault();


            return Json(emp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalarySubmit()
        {
            return View();
        }

        public ActionResult SalaryPreview()
        {
            return View();
        }

        // GET: AspNetSalaryDetail
        public ActionResult Index(int? Id)
        {
            List<AspNetSalaryDetail> aspNetSalaryDetail = new List<AspNetSalaryDetail>();
            aspNetSalaryDetail = db.AspNetSalaryDetails.Where(x => x.SalaryId == Id).ToList();

            var title = db.AspNetSalaries.Find(Id);
            ViewBag.Title = title.Title;
            return View(aspNetSalaryDetail);
        }

        // GET: AspNetSalaryDetail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSalaryDetail aspNetSalaryDetail = db.AspNetSalaryDetails.Find(id);
            

            if (aspNetSalaryDetail == null)
            {
                return HttpNotFound();
            }
       

            return View(aspNetSalaryDetail);
        }

        // GET: AspNetSalaryDetail/Create
        public ActionResult Create(string Month, string Title, int VId, string Year)
        {
            ViewBag.VirtualRoleId = new SelectList(db.AspNetVirtualRoles, "Id", "Name");
            ViewBag.VId = VId;
            ViewBag.Month = Month;
            ViewBag.Title = Title;
            ViewBag.Year = Year;
            return View();
        }


        public JsonResult SaveSalary(Salary Salaries)
        {

            AspNetSalary salary = new AspNetSalary();
            salary.Month = Salaries.Month;
            salary.Title = Salaries.Title;
            salary.VirtualRoleID = Salaries.VId;
            salary.Year = Salaries.Year;

            db.AspNetSalaries.Add(salary);
            db.SaveChanges();
            var salaryID = db.AspNetSalaries.Select(x => x.Id).Max();
            foreach (var item in Salaries.EmployeeSalary)
            {
                AspNetSalaryDetail details = new AspNetSalaryDetail();
                details.GrossSalary = item.GrossSalary;
                details.AccomodationAllowance = null;
                details.AdvanceSalary = null;
                details.ATaxSalary = item.AfterTaxSalary;
                details.BasicSalary = null;
                details.Bonus = item.Bonus;
                details.EmployeeId = item.Id;
                details.MedicalAllowance = item.MedicalAllowance;
                details.TotalSalary = item.TotalSalary;
                details.AdvancePf = null;
                details.AfterCutSalary = item.AfterCutSalary;
                details.BTaxSalary = item.BeforeTaxSalary;
                details.EmployeeEOP = item.EmployeeEOP;
                details.EmployeePF = null;
                details.FineCut = item.FineCut;
                if(item.Status == "Hold")
                    details.SalaryHold = item.TotalSalary;
                else
                    if( item.HOLD != null)
                {
                    foreach (var holds in item.HOLD)
                    {
                        var row = db.AspNetSalaryDetails.Where(x => x.Id == holds.id).Select(x => x).FirstOrDefault();
                        row.Status = "Paid";
                    }
                    
                    details.SalaryHold = 0;
                }
                    
                details.Status = item.Status;
                details.Total = item.Total;
                details.SchoolEOP = null;
                details.Tax = item.Tax;
                details.SalaryId = salaryID;

                db.AspNetSalaryDetails.Add(details);
                
            }
            db.SaveChanges();
            return Json(salaryID, JsonRequestBehavior.AllowGet);
        }



        // POST: AspNetSalaryDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,BasicSalary,MedicalAllowance,AccomodationAllowance,GrossSalary,Bonus,EOPSchool,TotalSchoolPayble,FineCutOff,ADVSalary,PF,EOPEmployee,TotalDeduction,Salary,IncomeTax,ATaxSalary,SalaryHold,Total,Status")] AspNetSalaryDetail aspNetSalaryDetail)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.AspNetSalaryDetails.Add(aspNetSalaryDetail);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(aspNetSalaryDetail);
        //}



        // GET: AspNetSalaryDetail/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSalaryDetail aspNetSalaryDetail = db.AspNetSalaryDetails.Find(id);

            if (aspNetSalaryDetail == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSalaryDetail);
        }
        // POST: AspNetSalaryDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,BasicSalary,MedicalAllowance,AccomodationAllowance,GrossSalary,Bonus,EOPSchool,TotalSchoolPayble,FineCutOff,ADVSalary,PF,EOPEmployee,TotalDeduction,Salary,IncomeTax,ATaxSalary,SalaryHold,Total,Status")] AspNetSalaryDetail aspNetSalaryDetail)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(aspNetSalaryDetail).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(aspNetSalaryDetail);
        //}

        // GET: AspNetSalaryDetail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSalaryDetail aspNetSalaryDetail = db.AspNetSalaryDetails.Find(id);
            if (aspNetSalaryDetail == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSalaryDetail);
        }

        // POST: AspNetSalaryDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetSalaryDetail aspNetSalaryDetail = db.AspNetSalaryDetails.Find(id);
            db.AspNetSalaryDetails.Remove(aspNetSalaryDetail);
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

        public class Salary
        {
            public string Month { get; set; }
            public string Title { get; set; }
            public string Year { get; set; }
            public int VId { get; set; }
            public List<EmployeeSalary> EmployeeSalary { set; get; }
        }

        public class EmployeeSalary
        {
            public int Id { get; set; }
            public int GrossSalary { get; set; }
            public int MedicalAllowance { get; set; }
            public int Accomodation { get; set; }
            public int BasicSalary { get; set; }
            public int FineCut { get; set; }
            public int AfterCutSalary { get; set; }
            public int AdvancePF { get; set; }
            public int EmployeePF { get; set; }
            public int AdvanceSalary { get; set; }
            public int EmployeeEOP { get; set; }
            public int Total { get; set; }
            public int SchoolEOP { get; set; }
            public int BeforeTaxSalary { get; set; }
            public int AfterTaxSalary { get; set; }
            public int Bonus { get; set; }
            public List<Hold> HOLD { set; get; }
            public int TotalSalary { get; set; }
            public string Status { get; set; }
            public int Tax { get; set; }
            public int SalaryID { get; set; }

        }

        public class Hold
        {
            public int id { set; get; }
            public int value { set; get; }
        }
    }
}
