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
    public class StudentFeeMonthsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: StudentFeeMonths
        public ActionResult Index()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }
        //public ActionResult GetStudentFeeMonth()
        //{
        //    var start = DateTime.Now;
        //    var list = db.StudentFeeMonths.ToList();
        //    List<StudentMonthlyFee> monthlyfee = new List<StudentMonthlyFee>();
        //    foreach (var item in list)
        //    {
        //        StudentMonthlyFee fee = new StudentMonthlyFee();
        //        fee.Id = item.Id;
        //        fee.Date = item.IssueDate;
        //        fee.Name = item.AspNetStudent.AspNetUser.Name;
        //        fee.Month = item.Months;
        //        fee.Status = item.Status;
        //        fee.PayableFee = item.FeePayable;
        //        fee.MonthlyFee = item.InstalmentAmount;
        //        fee.Multiplier = item.Multiplier;
        //        monthlyfee.Add(fee);
        //    }
        //    var end = DateTime.Now;
        //    var diff = end - start;
        //    return Json(monthlyfee, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetStudentFeeMonth()
        {
            var list = (from fee in db.StudentFeeMonths
                        join std in db.AspNetStudents on fee.StudentId equals std.Id


                        select new { fee.Id, std.AspNetUser.Name,std.AspNetUser.UserName, fee.IssueDate, fee.Months, fee.Status,fee.Discount,fee.FeeReceived,fee.FeeType,
                                     fee.FeePayable, fee.InstalmentAmount }).OrderBy(x=>x.IssueDate).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StudentFeeByClass(int id)
        {
            int? SessionId =   db.AspNetClasses.Where(x => x.Id == id).FirstOrDefault().SessionID;

            var list = (from fee in db.StudentFeeMonths
                        join std in db.AspNetStudents on fee.StudentId equals std.Id
                        where (fee.SessionId== SessionId)
                        select new
                        {
                            fee.Id,
                            std.AspNetUser.Name,
                            std.AspNetUser.UserName,
                            fee.IssueDate,
                            fee.Months,
                            fee.Status,
                            fee.Discount,
                            fee.FeeReceived,
                            fee.FeeType,
                            fee.FeePayable,
                            fee.InstalmentAmount
                        }).OrderBy(x => x.IssueDate).ToList();


            return Json(list, JsonRequestBehavior.AllowGet);

        }



        public ActionResult GetStudentDetailDDL(string Month, string Status)
        {

            if (Month != null && Status == null)
            {

                var MonthList = db.StudentFeeMonths.Where(x => x.Months == Month).ToList();
                List<StudentMonthlyFee> monthlyfee = new List<StudentMonthlyFee>();
                foreach (var item in MonthList)
                {
                    StudentMonthlyFee fee = new StudentMonthlyFee();
                    fee.Id = item.Id;
                 //shahzad   fee.Date = item.IssueDate;
                    fee.Name = item.AspNetStudent.AspNetUser.Name;
                    fee.Month = item.Months;
                    fee.Status = item.Status;
                    fee.PayableFee = item.FeePayable;
                    fee.MonthlyFee = item.InstalmentAmount;
                    fee.Multiplier = item.Multiplier;
                    monthlyfee.Add(fee);
                }
                return Json(monthlyfee, JsonRequestBehavior.AllowGet);
            }

            else if (Month == "All" && Status == "All")
            {

                var MonthList = db.StudentFeeMonths.ToList();
                List<StudentMonthlyFee> monthlyfee = new List<StudentMonthlyFee>();
                foreach (var item in MonthList)
                {
                    StudentMonthlyFee fee = new StudentMonthlyFee();
                    fee.Id = item.Id;
                 //shahzad   fee.Date = item.IssueDate;
                    fee.Name = item.AspNetStudent.AspNetUser.Name;
                    fee.Month = item.Months;
                    fee.Status = item.Status;
                    fee.PayableFee = item.FeePayable;
                    fee.MonthlyFee = item.InstalmentAmount;
                    fee.Multiplier = item.Multiplier;
                    monthlyfee.Add(fee);
                }
                return Json(monthlyfee, JsonRequestBehavior.AllowGet);
            }
            else if (Month == "All" && Status != null)
            {

                var MonthList = db.StudentFeeMonths.Where(x => x.Status == Status).ToList();
                List<StudentMonthlyFee> monthlyfee = new List<StudentMonthlyFee>();
                foreach (var item in MonthList)
                {
                    StudentMonthlyFee fee = new StudentMonthlyFee();
                    fee.Id = item.Id;
                 //shahzad   fee.Date = item.IssueDate;
                    fee.Name = item.AspNetStudent.AspNetUser.Name;
                    fee.Month = item.Months;
                    fee.Status = item.Status;
                    fee.PayableFee = item.FeePayable;
                    fee.MonthlyFee = item.InstalmentAmount;
                    fee.Multiplier = item.Multiplier;
                    monthlyfee.Add(fee);
                }
                return Json(monthlyfee, JsonRequestBehavior.AllowGet);
            }
            else if (Month != null && Status == "All")
            {

                var MonthList = db.StudentFeeMonths.Where(x => x.Months == Month).ToList();
                List<StudentMonthlyFee> monthlyfee = new List<StudentMonthlyFee>();
                foreach (var item in MonthList)
                {
                    StudentMonthlyFee fee = new StudentMonthlyFee();
                    fee.Id = item.Id;
                  //shahzad  fee.Date = item.IssueDate;
                    fee.Name = item.AspNetStudent.AspNetUser.Name;
                    fee.Month = item.Months;
                    fee.Status = item.Status;
                    fee.PayableFee = item.FeePayable;
                    fee.MonthlyFee = item.InstalmentAmount;
                    fee.Multiplier = item.Multiplier;
                    monthlyfee.Add(fee);
                }
                return Json(monthlyfee, JsonRequestBehavior.AllowGet);
            }
            else if (Month != null && Status == "Printed")
            {

                var MonthList = db.StudentFeeMonths.Where(x => x.Months == Month && x.Status == Status).ToList();
                List<StudentMonthlyFee> monthlyfee = new List<StudentMonthlyFee>();
                foreach (var item in MonthList)
                {
                    StudentMonthlyFee fee = new StudentMonthlyFee();
                    fee.Id = item.Id;
                   //shahzad fee.Date = item.IssueDate;
                    fee.Name = item.AspNetStudent.AspNetUser.Name;
                    fee.Month = item.Months;
                    fee.Status = item.Status;
                    fee.PayableFee = item.FeePayable;
                    fee.MonthlyFee = item.InstalmentAmount;
                    fee.Multiplier = item.Multiplier;
                    monthlyfee.Add(fee);
                }
                return Json(monthlyfee, JsonRequestBehavior.AllowGet);
            }
            else if (Month != null && Status != null)
            {

                var MonthList = db.StudentFeeMonths.Where(x => x.Months == Month && x.Status == Status).ToList();
                List<StudentMonthlyFee> monthlyfee = new List<StudentMonthlyFee>();
                foreach (var item in MonthList)
                {
                    StudentMonthlyFee fee = new StudentMonthlyFee();
                    fee.Id = item.Id;
                  //shahzad  fee.Date = item.IssueDate;
                    fee.Name = item.AspNetStudent.AspNetUser.Name;
                    fee.Month = item.Months;
                    fee.Status = item.Status;
                    fee.PayableFee = item.FeePayable;
                    fee.MonthlyFee = item.InstalmentAmount;
                    fee.Multiplier = item.Multiplier;
                    monthlyfee.Add(fee);
                }
                return Json(monthlyfee, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var StatusList = db.StudentFeeMonths.Where(x => x.Status == Status).ToList();
                List<StudentMonthlyFee> Statusdata = new List<StudentMonthlyFee>();
                foreach (var item in StatusList)
                {
                    StudentMonthlyFee fee = new StudentMonthlyFee();
                    fee.Id = item.Id;
                    //shahzad  fee.Date = item.IssueDate;
                    fee.Name = item.AspNetStudent.AspNetUser.Name;
                    fee.Month = item.Months;
                    fee.Status = item.Status;
                    fee.PayableFee = item.FeePayable;
                    fee.MonthlyFee = item.InstalmentAmount;
                    fee.Multiplier = item.Multiplier;
                    Statusdata.Add(fee);
                }
                return Json(Statusdata, JsonRequestBehavior.AllowGet);
            }

        }

        public class StudentMonthlyFee  
        {
            public int Id { get; set; }
            public string Date { get; set; }
            public string Name { get; set; }
            public string Month { get; set; }
            public string Status { get; set; }
            public double? MonthlyFee { get; set; }
            public double? PayableFee { get; set; }
            public double? Multiplier { get; set; }

        }

        // GET: StudentFeeMonths/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentFeeMonth studentFeeMonth = db.StudentFeeMonths.Find(id);
            if (studentFeeMonth == null)
            {
                return HttpNotFound();
            }
            return View(studentFeeMonth);
        }

        // GET: StudentFeeMonths/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name");
            return View();
        }

        // POST: StudentFeeMonths/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,Months,Status,InstalmentAmount,Date")] StudentFeeMonth studentFeeMonth)
        {
            try
            {
                var checkbox = Request.Form["ingredients"];
                List<string> names = new List<string>(checkbox.Split(','));
                if (ModelState.IsValid)
                {
                    foreach (var item in names)
                    {
                        studentFeeMonth.Months = item;
                        db.StudentFeeMonths.Add(studentFeeMonth);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }

                ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", studentFeeMonth.StudentId);
                return View(studentFeeMonth);
            }
            catch
            {
                return RedirectToAction("Create");

            }

        }
        //public ActionResult GetStudentInstalment(int stdid)
        //{
        //   var amount= db.StudentFeeMultipliers.Where(x => x.StudentId == stdid).Select(x => x.SharePerInstalment).FirstOrDefault();
        //    return Json(amount,JsonRequestBehavior.AllowGet);
        //}
        // GET: StudentFeeMonths/Edit/5
        public ActionResult GetStatusRecord(string StatusRecord)
        {
            var dbTransaction = db.Database.BeginTransaction();
            string status = "error";
            if (StatusRecord != "")
            {
                var RecordId = StatusRecord.Split(',');
                for (int i = 0; i < RecordId.Count(); i++)
                {
                    double? Arrear = 0;
                    var sfmid = Int32.Parse(RecordId[i]);
                    StudentFeeMonth data = db.StudentFeeMonths.Where(x => x.Id == sfmid).FirstOrDefault();
                    if (data.Status == "Printed")
                    {
                        data.Status = "Clear";

                        ///////////////arrears////////////
                        var _SessionId = data.SessionId;
                        int _Id = data.Id - 1;
                        bool Flag = true;
                        
                        while (Flag)
                        {
                            try
                            {
                                double? payableamount = db.StudentFeeMonths.Where(x => x.StudentId == data.StudentId && x.SessionId == _SessionId && x.Status == "Pending" && x.Id == _Id).FirstOrDefault().FeePayable;

                                Arrear += payableamount;
                                _Id--;
                            }
                            catch (Exception ex)
                            {
                                Flag = false;
                            }
                        }
                        var totalreceivable = Arrear + data.FeePayable;
                        /////////End/////////
                        try
                        {
                            Ledger ledger_Assets = db.Ledgers.Where(x => x.Name == "Student Receivables").FirstOrDefault();
                            ledger_Assets.CurrentBalance -= Convert.ToDecimal(totalreceivable);
                            db.SaveChanges();

                            Ledger ledger_bank = db.Ledgers.Where(x => x.Name == "Meezan Bank" && x.LedgerGroup.Name == "Bank").FirstOrDefault();
                            ledger_bank.CurrentBalance += Convert.ToDecimal(totalreceivable);
                            db.SaveChanges();
                            status = "success";
                        }
                        catch
                        {
                            TempData["ErrorMessage"] = "No Cash in Student Receivables Account.";
                            return RedirectToAction("Edit", data);
                            //   return RedirectToAction("Edit?="+studentFeeMonth.StudentId);

                        }
                    }
                    else if (data.Status == "Clear")
                    {
                        data.Status = "Printed";
                        var totalreceivable = Arrear + data.FeePayable;

                        try
                        {
                            Ledger ledger_Assets = db.Ledgers.Where(x => x.Name == "Student Receivables").FirstOrDefault();
                            ledger_Assets.CurrentBalance += Convert.ToDecimal(totalreceivable);
                            db.SaveChanges();

                            Ledger ledger_bank = db.Ledgers.Where(x => x.Name == "Meezan Bank" && x.LedgerGroup.Name == "Bank").FirstOrDefault();
                            ledger_bank.CurrentBalance -= Convert.ToDecimal(totalreceivable);
                            db.SaveChanges();
                            status = "success";
                        }
                        catch
                        {
                            TempData["ErrorMessage"] = "No Cash in Meezan Bank Account.";
                            return RedirectToAction("Edit", data);
                            //   return RedirectToAction("Edit?="+studentFeeMonth.StudentId);

                        }

                    }

                    if (db.SaveChanges() > 0)
                    {
                        status = "success";
                    }
                   
                }
            }
            dbTransaction.Commit();
            return Content(status);

        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentFeeMonth studentFeeMonth = db.StudentFeeMonths.Find(id);
            if (studentFeeMonth == null)
            {
                return HttpNotFound();
            }
            ViewBag.Error = TempData["ErrorMessage"] as string;
            ViewBag.StudentId = new SelectList(db.AspNetStudents.Where(x => x.AspNetUser.Status != "False").Select(x => new { x.Id, x.AspNetUser.Name }), "Id", "Name", studentFeeMonth.StudentId);

            //  ViewBag.StudentId = new SelectList(db.AspNetUsers, "Id", "Name", studentFeeMonth.StudentId);
            return View(studentFeeMonth);
        }

        // POST: StudentFeeMonths/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,Months,Status,InstalmentAmount,IssueDate,Date")] StudentFeeMonth studentFeeMonth)
        {
            if (studentFeeMonth.Status == "Clear")
            {
                var dbTransaction = db.Database.BeginTransaction();
                StudentFeeMonth data = db.StudentFeeMonths.Where(x => x.StudentId == studentFeeMonth.StudentId && x.Months == studentFeeMonth.Months).FirstOrDefault();
                data.Status = "Clear";
                db.SaveChanges();

                try
                {
                    Ledger ledger_Assets = db.Ledgers.Where(x => x.Name == "Student Receivables").FirstOrDefault();
                    ledger_Assets.CurrentBalance -= Convert.ToDecimal(studentFeeMonth.InstalmentAmount);
                    db.SaveChanges();

                    Ledger ledger_bank = db.Ledgers.Where(x => x.Name == "Meezan Bank" && x.LedgerGroup.Name == "Bank").FirstOrDefault();
                    ledger_bank.CurrentBalance += Convert.ToDecimal(studentFeeMonth.InstalmentAmount);
                    db.SaveChanges();
                }
                catch
                {
                    TempData["ErrorMessage"] = "No Cash in Student Receivables Account.";
                    return RedirectToAction("Edit", studentFeeMonth);
                 //   return RedirectToAction("Edit?="+studentFeeMonth.StudentId);

                }

                dbTransaction.Commit();
            }

            return RedirectToAction("Index");
        }
        //else
        //{
        //    var checkbox1 = Request.Form["counter"];
        //    bool Flag = true;
        //    double? installmentamount = studentFeeMonth.InstalmentAmount;
        //    float multiplier;
        //    try
        //    {
        //        multiplier = float.Parse(Request.Form["multiplier"]);
        //    }
        //    catch (Exception ex)
        //    {
        //        multiplier = 1;
        //    }
        //    studentFeeMonth.Multiplier = Math.Round(multiplier, 2, MidpointRounding.AwayFromZero);

        //    double decimalvalues = multiplier - Math.Truncate(multiplier);
        //    double roundvalues = Math.Round(decimalvalues, 2, MidpointRounding.AwayFromZero);
        //    var total = float.Parse(Request.Form["total"]);
        //    studentFeeMonth.FeePayable = total;
        //    var round = Math.Ceiling(multiplier);
        //    var checkbox = Request.Form["counter"];
        //    if (checkbox == null)
        //    {
        //        ViewBag.Error = "Please Select Months.";
        //        ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", studentFeeMonth.StudentId);
        //        return View(studentFeeMonth);
        //    }
        //    List<string> names = new List<string>(checkbox.Split(','));
        //    int months = names.Count();

        //    if (!names.Contains(studentFeeMonth.Months))
        //    {
        //        if (round == months + 1)
        //        {
        //            if (roundvalues == 0)  // for int multiplier
        //            {
        //                double? value = (total - installmentamount) / months;

        //                for (var i = 0; i < names.Count; i++)
        //                {
        //                    string monthname = names[i];
        //                    StudentFeeMonth student = db.StudentFeeMonths.Where(x => x.Months == monthname && x.StudentId == studentFeeMonth.StudentId).FirstOrDefault();
        //                    student.FeePayable = student.FeePayable - value;
        //                    if (student.FeePayable >= 0)
        //                    {
        //                        db.SaveChanges();
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Error = "Invalid Transaction.";
        //                        ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", studentFeeMonth.StudentId);
        //                        return View(studentFeeMonth);
        //                    }
        //                }
        //            }
        //            else
        //            {   //for float multiplier

        //                var round2 = Math.Floor(multiplier);
        //                var evenprice = installmentamount * round2;
        //                double? value2 = (evenprice) / months;
        //                string Lastmonth = names.Last();

        //                for (var i = 0; i < names.Count - 1; i++)
        //                {
        //                    string monthname = names[i];
        //                    StudentFeeMonth student = db.StudentFeeMonths.Where(x => x.Months == monthname && x.StudentId == studentFeeMonth.StudentId).FirstOrDefault();
        //                    student.FeePayable = student.FeePayable - value2;
        //                    if (student.FeePayable >= 0)
        //                    {
        //                        db.SaveChanges();
        //                    }

        //                    else
        //                    {
        //                        ViewBag.Error = "Invalid Transaction.";
        //                        ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", studentFeeMonth.StudentId);
        //                        return View(studentFeeMonth);
        //                    }
        //                }
        //                //last month
        //                if (Lastmonth != null)
        //                {
        //                    var oddprice = total - evenprice;
        //                    StudentFeeMonth studentdata = db.StudentFeeMonths.Where(x => x.Months == Lastmonth && x.StudentId == studentFeeMonth.StudentId).FirstOrDefault();
        //                    studentdata.FeePayable = studentdata.FeePayable - oddprice;
        //                    if (studentdata.FeePayable >= 0)
        //                    {
        //                        db.SaveChanges();
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Error = "Invalid Transaction.";
        //                        ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", studentFeeMonth.StudentId);
        //                        return View(studentFeeMonth);
        //                    }
        //                }

        //            }

        //        }
        //        else
        //        {
        //            Flag = false;
        //            // months extends or decends
        //            ViewBag.Error = "Months Exceeds.";
        //            // ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", studentFeeMonth.StudentId);
        //            ViewBag.StudentId = new SelectList(db.AspNetStudents.Where(x => x.AspNetUser.Status != "False").Select(x => new { x.Id, x.AspNetUser.Name }), "Id", "Name", studentFeeMonth.StudentId);
        //            return View(studentFeeMonth);

        //        }
        //    }
        //    else
        //    {
        //        Flag = false;
        //        //invalid month selected
        //        ViewBag.Error = "Invalid Month Selected.";
        //        // ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", studentFeeMonth.StudentId);
        //        ViewBag.StudentId = new SelectList(db.AspNetStudents.Where(x => x.AspNetUser.Status != "False").Select(x => new { x.Id, x.AspNetUser.Name }), "Id", "Name", studentFeeMonth.StudentId);

        //        return View(studentFeeMonth);
        //    }

        //    //if (checkbox == null)
        //    //{
        //    //    ViewBag.ErrorMessage = "No Month Selected, Please Select Month";
        //    //}
        //    if (ModelState.IsValid && Flag == true)
        //    {
        //        studentFeeMonth.Status = studentFeeMonth.Status;
        //        studentFeeMonth.DueDate = studentFeeMonth.DueDate;
        //        studentFeeMonth.IssueDate = studentFeeMonth.IssueDate;
        //        db.Entry(studentFeeMonth).State = EntityState.Modified;
        //        db.SaveChanges();


        //        Month_Multiplier Multi = new Month_Multiplier();

        //        for (var i = 0; i < names.Count; i++)
        //        {

        //            string monthname = names[i];

        //            Multi.PaymentMonth = studentFeeMonth.Months;
        //            Multi.StudentId = studentFeeMonth.StudentId;
        //            Multi.AdjustedMonth = monthname;
        //            db.Month_Multiplier.Add(Multi);
        //            db.SaveChanges();

        //        }

        //        //if(studentFeeMonth.Status=="Clear")
        //        //{
        //        //    StudentFeeMultiplier stdfee = db.StudentFeeMultipliers.Where(x => x.StudentId == studentFeeMonth.StudentId).Select(x => x).FirstOrDefault();
        //        //    stdfee.PaidInstalments += 1;
        //        //    stdfee.PaidAmount += studentFeeMonth.InstalmentAmount;
        //        //    stdfee.RemainingInstalments -= 1;
        //        //    stdfee.RemainingAmount -= studentFeeMonth.InstalmentAmount;
        //        //    db.SaveChanges();

        //        //    Ledger ledger = db.Ledgers.Where(x => x.Name == "Student Receivables").FirstOrDefault();
        //        //    ledger.CurrentBalance -= Convert.ToDecimal(studentFeeMonth.InstalmentAmount);
        //        //    db.SaveChanges();                    
        //        //}


        //        return RedirectToAction("Index");
        //    }
        //}
        //ViewBag.StudentId = new SelectList(db.AspNetStudents, "Id", "Name", studentFeeMonth.StudentId);
        //return View(studentFeeMonth);
        //}

        // GET: StudentFeeMonths/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentFeeMonth studentFeeMonth = db.StudentFeeMonths.Find(id);
            if (studentFeeMonth == null)
            {
                return HttpNotFound();
            }
            return View(studentFeeMonth);
        }

        // POST: StudentFeeMonths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentFeeMonth studentFeeMonth = db.StudentFeeMonths.Find(id);
            db.StudentFeeMonths.Remove(studentFeeMonth);
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
